using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Diagnostics;
public class DataReceived : MonoBehaviour {

    //Public GameObjects
    public GameObject SlipStreamObject;
    public GameObject globalDef;
    GameObject Head;
    GameObject[] Hands;
    GameObject HeadCamera;
    //public GameObject handsController;
        
    // Default variables
    int numHands = 2;
    int headId = 2;
    
    // Events
    public event ChangeHandsStatusHandler UpdateHandsStatus;

        //Status
    HandStatus currentStatus = new HandStatus();
    bool sendUpdate = false;
    
    //Variables
    Vector3 newPosition;
    Vector3 oldPosition;
    Vector3 headPosition;
    Quaternion newOrientation;
    int positionOnSampleArray=-1; //Va recorriendo el arreglo de samplePosition actualizando de forma circular //Se actualiza al recibir un nuevo frame // indica la posicion actualizada 
    Vector3[,] samplePositions; //Guarda las ultimas windowSize posiciones de las manos [2,windowSize]
    Vector3[] averagePosition = new Vector3[3]; //Average of the saved positions
    double sampleVariance;
    int countFramesEquals = 0;
    int IDStatus = 0;
    float initialDistanceHands;
    float finalDistanceHands;
    float differenceDistance;
    float tempAngle;
    //public int speed = 20;
    /*
    DEBUG
    */
    Stopwatch timeDataReceived;

    void Start()
    {
        //Initialice data receiver
        SlipStreamObject.GetComponent<SlipStream>().PacketNotification += new PacketReceivedHandler(OnPacketReceived);
        //globalDef = GameObject.Find("GlobalDefinitions");
        Hands = globalDef.GetComponent<globalDefinitions>().Hands;
        Head = globalDef.GetComponent<globalDefinitions>().Head;
        HeadCamera = globalDef.GetComponent<globalDefinitions>().HeadCamera;
        samplePositions = new Vector3[3, constant.windowSize];
        timeDataReceived = new Stopwatch();
    }

    // Packet received
    void OnPacketReceived(object sender, string Packet)
    {
        //timeDataReceived = Stopwatch.StartNew(); //.Reset() .StartNew();
        //Update de index for the sample array
        positionOnSampleArray = (positionOnSampleArray + 1) % constant.windowSize;
        
        //Get document with the data from motive
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Packet);
        XmlNodeList rigidBodiesList = xmlDoc.GetElementsByTagName("Body");
        
        //HANDS POSITIONING
        for (int k = 0; k < numHands; k += 1)
        {
            int id = System.Convert.ToInt32(rigidBodiesList[k].Attributes["ID"].InnerText);
            //POSITION
            float x = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["x"].InnerText) * constant.factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["y"].InnerText) * constant.factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["z"].InnerText) * constant.factor;
            newPosition = new Vector3(-x, y, z);
            oldPosition = samplePositions[k, positionOnSampleArray];
            //ROTATION
            float qx = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qx"].InnerText);
            float qy = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qy"].InnerText);
            float qz = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qz"].InnerText);
            float qw = (float)System.Convert.ToDouble(rigidBodiesList[k].Attributes["qw"].InnerText);
            newOrientation = new Quaternion(qx, -qy, -qz, qw);
              
            //VARIANCE
            //Calculate the variance from the last windowSize frames         
            averagePosition[k] = averagePosition[k] + (newPosition - oldPosition) / constant.windowSize;
            samplePositions[k, positionOnSampleArray] = newPosition;
            sampleVariance = 0.0;
            for (int i = 0; i < constant.windowSize; i += 1)
            {
                sampleVariance += Vector3.SqrMagnitude(samplePositions[k, i] - averagePosition[k]);
            }
            sampleVariance /= constant.windowSize;

            //UPDATE body values
            Hands[k].transform.position = newPosition;
            Hands[k].transform.rotation = newOrientation;
            
            //UPDATE current status
            currentStatus.handPosition[k] = newPosition;
            currentStatus.handRotation[k] = newOrientation.eulerAngles;

            //UPDATE Hand moving
            
            if (sampleVariance > Threshold.limInferiorVariance)
            {
                //hand is now moving
                if (!currentStatus.handMoving[k])
                {
                    //hand wasnt moving
                    currentStatus.handMoving[k] = true;
                    countFramesEquals = 0;
                    sendUpdate = true;
                }
            }
            else
            {
                //hand is not moving
                if(currentStatus.handMoving[k])
                {
                    //hand was moving
                    currentStatus.handMoving[k] = false;
                    countFramesEquals = 0;
                    sendUpdate = true;
                }
            }
        }
        countFramesEquals += 1;

   
        //UPDATE CAMERA
        if (rigidBodiesList.Count > headId)
        {
            //Debug.Log("update camera position");
            int id = System.Convert.ToInt32(rigidBodiesList[headId].Attributes["ID"].InnerText);
            //POSITION
            float x = (float)System.Convert.ToDouble(rigidBodiesList[headId].Attributes["x"].InnerText) * constant.factor;
            float y = (float)System.Convert.ToDouble(rigidBodiesList[headId].Attributes["y"].InnerText) * constant.factor;
            float z = (float)System.Convert.ToDouble(rigidBodiesList[headId].Attributes["z"].InnerText) * constant.factor;
            headPosition = new Vector3(-x, (float)(y - constant.headSize), z);
            Head.transform.position = headPosition;
            //TODO revisar cual de las dos instrucciones se ve mejor
            //head.transform.position = Vector3.MoveTowards(head.transform.position, position, speed * Time.deltaTime);
        }
        //UPDATE id
        currentStatus.id = IDStatus;
        IDStatus += 1;
        //UPDATE normal to the point of view
        currentStatus.normal = HeadCamera.transform.forward;
        int signAngle;
        if ((Mathf.Acos(Vector3.Dot(new Vector3(1, 0, 0), currentStatus.normal) / Vector3.Magnitude(currentStatus.normal))) * 180 / Mathf.PI > 90.0)
            signAngle = -1;
        else
            signAngle = 1;
        currentStatus.angleNormal = signAngle * (Mathf.Acos(Vector3.Dot(new Vector3(0, 0, 1), currentStatus.normal) / Vector3.Magnitude(currentStatus.normal))) * 180 / Mathf.PI;

        // Si he tenido el mismo estado por sameFrameWindowSize envio un actualizo el movimiento de las manos
        if (sendUpdate && countFramesEquals > constant.sameFrameWindowSize && UpdateHandsStatus != null)
        {
            //UPDATE distance
            initialDistanceHands = Vector3.Distance(samplePositions[(int)Hand.Right, (positionOnSampleArray + 1) % constant.windowSize], samplePositions[(int)Hand.Left, (positionOnSampleArray + 1) % constant.windowSize]);
            finalDistanceHands = Vector3.Distance(samplePositions[(int)Hand.Right, positionOnSampleArray], samplePositions[(int)Hand.Left, positionOnSampleArray]);
            differenceDistance = initialDistanceHands - finalDistanceHands;
            if (differenceDistance < Threshold.limInferiorDistance)
            {
                currentStatus.distance = (int)Distance.Shorter;
            }
            else if (differenceDistance > Threshold.limSuperiorDistance)
            {
                currentStatus.distance = (int)Distance.Bigger;
            }
            else
            {
                currentStatus.distance = (int)Distance.Same;
            }

            //TO-DO Update hand direction 
            //RIGHT HAND
            if (currentStatus.handMoving[(int)Hand.Right])
            {
                currentStatus.handDirection[(int)Hand.Right] = samplePositions[(int)Hand.Right, positionOnSampleArray] - samplePositions[(int)Hand.Right, (positionOnSampleArray + 1) % constant.windowSize];
                tempAngle = Mathf.Acos(Vector3.Dot(currentStatus.handDirection[(int)Hand.Right], currentStatus.normal) / (Vector3.Magnitude(currentStatus.handDirection[(int)Hand.Right]) * Vector3.Magnitude(currentStatus.normal)));
                currentStatus.angleMovement[(int)Hand.Right] = (int)(tempAngle * 180 / Mathf.PI);
            }
            else
            {
                currentStatus.handDirection[(int)Hand.Right] = Vector3.zero;
                currentStatus.angleMovement[(int)Hand.Right] = 0;
            }

            //LEFTHAND
            if (currentStatus.handMoving[(int)Hand.Left])
            {
                currentStatus.handDirection[(int)Hand.Left] = samplePositions[(int)Hand.Left, positionOnSampleArray] - samplePositions[(int)Hand.Left, (positionOnSampleArray + 1) % constant.windowSize];
                tempAngle = Mathf.Acos(Vector3.Dot(currentStatus.handDirection[(int)Hand.Left], currentStatus.normal) / (Vector3.Magnitude(currentStatus.handDirection[(int)Hand.Left]) * Vector3.Magnitude(currentStatus.normal)));
                currentStatus.angleMovement[(int)Hand.Left] = (int)(tempAngle * 180 / Mathf.PI);
            }
            else
            {
                currentStatus.handDirection[(int)Hand.Left] = Vector3.zero;
                currentStatus.angleMovement[(int)Hand.Left] = 0;
            }
            sendUpdate = false;
        }
        UpdateHandsStatus(this, currentStatus);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnPostRender()
    {
        
    }

}

