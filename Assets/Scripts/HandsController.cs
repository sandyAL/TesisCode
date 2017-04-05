using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Hand : int
{
    Right = 0,
    Left = 1
};

enum Distance : int
{
    Shorter = -1,
    Same = 0,
    Bigger = 1
}

enum HandRotationType : int
{
    None = 0,
    Push = 1,
    Pull = 2,
    Booth = 3
}

enum HandMoves: int 
{
    None = -1,
    RightHandPush,
    RightHandPull,
    LeftHandPush,
    LeftHandPull,
    ZoomIn,
    ZoomOut,
    Traslate
}
public class HandStatus
{
    public int id;
    public bool[] handMoving { get; set; }
    public int distance { get; set; }
    public float[] angleMovement;
    public Vector3[] handDirection { get; set; }
    public Vector3[] handPosition { get; set; }
    public Vector3[] handRotation { get; set; }
    public Vector3 normal;
    public float angleNormal;

    public HandStatus()
    {
        id = 0;
        handMoving = new bool[2] { false, false };
        distance = 0;
        angleMovement = new float[2] { -1.0F, -1.0F };
        handDirection = new Vector3[2] { Vector3.zero, Vector3.zero };
        handPosition = new Vector3[2] { Vector3.zero, Vector3.zero };
        handRotation = new Vector3[2] { Vector3.zero, Vector3.zero };

    }

    public void update(HandStatus newStatus)
    {
        id = newStatus.id;
        distance = newStatus.distance;
        for (int i = 0; i < 2; i++)
        {
            handMoving[i] = newStatus.handMoving[i];
            handDirection[i] = newStatus.handDirection[i];
            handPosition[i] = newStatus.handPosition[i];
            handRotation[i] = newStatus.handRotation[i];
        }
    }
}

public class HandsController : MonoBehaviour {
    //GAME OBJECTS
    public GameObject[] Hands = new GameObject[2];
    public GameObject motiveData;
    public GameObject globalDef;

    //STATUS
    HandStatus newStatus;
    HandStatus oldStatus;
    public bool moveDetected;
    public int idMoveDetected;
    public int[] handTypePosition;

    //COLORS
    Material handMaterials;
    Material[] materials = new Material[2];
    

    //variable
    float limInferiorPushAngleZ = Threshold.centerAnglePushZ - Threshold.angleOneHand;
    float limSuperiorPushAngleZ = Threshold.centerAnglePushZ + Threshold.angleOneHand;

    // Use this for initialization
    void Start ()
    {
	    motiveData.GetComponent<DataReceived>().UpdateHandsStatus+= new ChangeHandsStatusHandler(OnUpdatedHandsStatusReceived);
        materials = globalDef.GetComponent<globalDefinitions>().materials;
        moveDetected = false;
        handTypePosition = new int[2];
}

    public void getRightRotationType ()
    {
        bool orientationYPos = newStatus.handRotation[(int)Hand.Right].y<180;
        bool angleNormalPos = newStatus.angleNormal < 180;
        //Debug.Log(limInferiorPushAngleZ.ToString()+"-" +newStatus.handRotation[(int)Hand.Right].z.ToString()+ "-"+ limSuperiorPushAngleZ.ToString());
        if (newStatus.handRotation[(int)Hand.Right].z > limInferiorPushAngleZ && newStatus.handRotation[(int)Hand.Right].z < limSuperiorPushAngleZ)
        {
            if (orientationYPos == !angleNormalPos)
            {
                handTypePosition[(int)Hand.Right] = (int)HandRotationType.Push;
            }
            else
            {
                handTypePosition[(int)Hand.Right] = (int)HandRotationType.Pull;
            }
            return;
        }
        /* //TODO add two hands type rotation
        if  (true)
        { 
        return (int)HandRotationType.Bouth;
        }*/
        handTypePosition[(int)Hand.Right] = (int)HandRotationType.None;
    }

    public void getLeftRotationType()
    {
        bool orientationYPos = newStatus.handRotation[(int)Hand.Left].y < 180;
        bool angleNormalPos = newStatus.angleNormal < 180;
        if (newStatus.handRotation[(int)Hand.Left].z > limInferiorPushAngleZ && newStatus.handRotation[(int)Hand.Left].z < limSuperiorPushAngleZ)
        {
            if (orientationYPos == !angleNormalPos)
            {
                handTypePosition[(int)Hand.Left] = (int)HandRotationType.Push;
            }
            else
            {
                handTypePosition[(int)Hand.Left] = (int)HandRotationType.Pull;
            }
            return;
        }
        /* //TODO add two hands type rotation
        if  ()
        { 
        return (int)HandRotationType.Bouth;
        }*/
        handTypePosition[(int)Hand.Left] = (int)HandRotationType.None;
    }

    //In this functions the colors for the movements are determined
    public Material getHandMaterial()
    {
        if (moveDetected)
        {
            Debug.Log("Movement");
            switch (idMoveDetected)
            {
                case (int)HandMoves.None:
                case (int)HandMoves.ZoomIn:
                case (int)HandMoves.ZoomOut:
                case (int)HandMoves.Traslate:
                    {
                        return materials[(int)MaterialColors.Silver];
                    }
                case (int)HandMoves.RightHandPush:
                    {
                        Debug.Log("Push");
                        return materials[(int)MaterialColors.Blue];
                    }
                case (int)HandMoves.RightHandPull:
                    {
                        return materials[(int)MaterialColors.Cyan];
                    }
                case (int)HandMoves.LeftHandPush:
                    {
                        return materials[(int)MaterialColors.Red];
                    }
                case (int)HandMoves.LeftHandPull:
                    {
                        return materials[(int)MaterialColors.Orange];
                    }

            }
        }
        else
        {
            Debug.Log("Not moving");
            if (handTypePosition[(int)Hand.Right]== (int)HandRotationType.Push  && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
                return materials[(int)MaterialColors.Purple];
                //return materials[(int)MaterialColors.LightBlue];
                if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Pull && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
                    return materials[(int)MaterialColors.Brown];
                    //return materials[(int)MaterialColors.LightCyan];
                if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Push))
                    return materials[(int)MaterialColors.Yellow];
                    //return materials[(int)MaterialColors.LightRed];
                if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Pull))
                    return materials[(int)MaterialColors.Green];
                    //return materials[(int)MaterialColors.LightOrange];
        }
        return materials[(int)MaterialColors.Silver];
    }
    
    public void detectMove()
    {
        moveDetected = false;
        idMoveDetected = (int)HandMoves.None;
        //  TWO HANDS
        if (newStatus.handMoving[(int)Hand.Right] && newStatus.handMoving[(int)Hand.Left])
        {
            //TODO Add two hands detectiono move
            moveDetected = true;
            return;
        }

        // RIGHT HAND
        if(newStatus.handMoving[(int)Hand.Right] && !newStatus.handMoving[(int)Hand.Left])
        {
            moveDetected = true;
            if (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None)
            {
                switch (handTypePosition[(int)Hand.Right])
                {
                    case (int)HandRotationType.Push:
                        {
                            if (newStatus.angleMovement[(int)Hand.Right] > (360 - Threshold.moveDirection) &&
                               newStatus.angleMovement[(int)Hand.Right] < (0 + Threshold.moveDirection))
                               {
                                   idMoveDetected = (int)HandMoves.RightHandPush;
                               }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            if (newStatus.angleMovement[(int)Hand.Right] > (180 - Threshold.moveDirection) ||
                                newStatus.angleMovement[(int)Hand.Right] < (180 + Threshold.moveDirection))
                              {
                                idMoveDetected = (int)HandMoves.RightHandPull;
                              }
                            break;
                        }
                    default:
                        { 
                            break;
                        }

                }
            }
            
            return;
        }

        //LEFT HAND
        if (!newStatus.handMoving[(int)Hand.Right] && newStatus.handMoving[(int)Hand.Left])
        {
            moveDetected = true;
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None)
            {
                switch (handTypePosition[(int)Hand.Left])
                {
                    case (int)HandRotationType.Push:
                        {
                            if (newStatus.angleMovement[(int)Hand.Left] > (180 - Threshold.moveDirection) &&
                               newStatus.angleMovement[(int)Hand.Left] < (180 + Threshold.moveDirection))
                            {
                                idMoveDetected = (int)HandMoves.LeftHandPush;
                            }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            if (newStatus.angleMovement[(int)Hand.Left] > (360 - Threshold.moveDirection) ||
                                   newStatus.angleMovement[(int)Hand.Left] < (0 + Threshold.moveDirection))
                            {
                                idMoveDetected = (int)HandMoves.LeftHandPull;
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
            }
            return;
        }
    }
    /*
    DEBUG
    */
    private void OnUpdatedHandsStatusReceived(object sender, HandStatus newHandStatus)
    {
        oldStatus = newStatus;
        newStatus = newHandStatus;
        getRightRotationType();
        getLeftRotationType();
        detectMove();
        //Debug.Log(handTypePosition[0].ToString()+" "+ handTypePosition[1].ToString());
        //Debug.Log(idMoveDetected);
        //Debug.Log("-----------------------------");
        Material newHandMaterial = getHandMaterial();
        if (newHandMaterial != handMaterials)
        {
            Hands[(int)Hand.Right].GetComponent<HandManager>().changeMaterial(newHandMaterial);
            Hands[(int)Hand.Left].GetComponent<HandManager>().changeMaterial(newHandMaterial);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    
}
