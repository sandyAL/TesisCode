//#define DEBUG_MEMORY

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
    Both = 3
}

enum HandMoves: int 
{
    None = -1,
    RightHandPush=0,
    RightHandPull=1,
    LeftHandPush=2,
    LeftHandPull=3,
    ZoomIn=4,
    ZoomOut=5,
    Traslate=6
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
    //float limInferiorPushAngleX = Threshold.centerAnglePushX -Threshold.anglePushX;
    //float limSuperiorPushAngleX = Threshold.centerAnglePushX + Threshold.anglePushX;
    //float limInferiorPushAngleZ = Threshold.centerAnglePushZ - Threshold.angleOneHand;
    //float limSuperiorPushAngleZ = Threshold.centerAnglePushZ + Threshold.angleOneHand;
    //float limInferiorPullAngleX = Threshold.centerAnglePullX - Threshold.anglePullX;
    ///float limSuperiorPullAngleX = Threshold.centerAnglePullX - Threshold.anglePullX;
    //float limInferiorPullAngleZ = Threshold.centerAnglePullZ - Threshold.angleOneHand;
    //float limSuperiorPullAngleZ = Threshold.centerAnglePullZ + Threshold.angleOneHand;
    
    public float limInferiorBothAngleRightHandX;
    public float limSuperiorBothAngleRightHandX;
    public float limInferiorBothAngleLeftHandX;
    public float limSuperiorBothAngleLeftHandX;
    //public float limInferiorPullAngleZ ;
    //public float limSuperiorPullAngleZ ;

    //Events
    public event idDetectedMovementHandler UpdateIdDetected;

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
#if DEBUG_MEMORY
        Debug.Log("getRightRotationType_Start");
#endif
        bool orientationYPos = newStatus.handRotation[(int)Hand.Right].y > 270 || newStatus.handRotation[(int)Hand.Right].y < 90;
        bool angleNormalPos = newStatus.angleNormal > 0;
        //Debug.Log(newStatus.handRotation[(int)Hand.Right].y.ToString() + "  +  " + newStatus.angleNormal.ToString()+ "  =  "+ (newStatus.handRotation[(int)Hand.Right].y + newStatus.angleNormal).ToString() );
        //Debug.Log(Threshold.limInferiorPushAngleX.ToString() +"<"+ newStatus.handRotation[(int)Hand.Right].x.ToString() +"<"+ Threshold.limSuperiorPushAngleX.ToString());
        //PUSH
        if (newStatus.handRotation[(int)Hand.Right].z > Threshold.limInferiorPushAngleZ && newStatus.handRotation[(int)Hand.Right].z < Threshold.limSuperiorPushAngleZ 
            && (orientationYPos != angleNormalPos)
            && (newStatus.handRotation[(int)Hand.Right].x > Threshold.limInferiorPushAngleX && newStatus.handRotation[(int)Hand.Right].x < Threshold.limSuperiorPushAngleX)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Push;
            return;
        }
        //PULL
        if (newStatus.handRotation[(int)Hand.Right].z > Threshold.limInferiorPullAngleZ && newStatus.handRotation[(int)Hand.Right].z < Threshold.limSuperiorPullAngleZ 
            && (orientationYPos == angleNormalPos)
            && (newStatus.handRotation[(int)Hand.Right].x > Threshold.limInferiorPullAngleX || newStatus.handRotation[(int)Hand.Right].x < Threshold.limSuperiorPullAngleX)
            )
        {
            //Debug.Log((newStatus.handRotation[(int)Hand.Right].x > Threshold.limInferiorPullAngleX) + " || " + (newStatus.handRotation[(int)Hand.Right].x < Threshold.limSuperiorPullAngleX));
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Pull;  
            return;
        }
        //BOTH
        if  (newStatus.handRotation[(int)Hand.Right].x > limInferiorBothAngleRightHandX && newStatus.handRotation[(int)Hand.Right].x < limSuperiorBothAngleRightHandX)
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Both;
            return; 
        }
        handTypePosition[(int)Hand.Right] = (int)HandRotationType.None;
#if DEBUG_MEMORY
        Debug.Log("getRightRotationType_End");
#endif
    }

    public void getLeftRotationType()
    {
#if DEBUG_MEMORY
        Debug.Log("getLeftRotationType_Start");
#endif
        bool orientationYPos = newStatus.handRotation[(int)Hand.Left].y > 270 || newStatus.handRotation[(int)Hand.Left].y < 90;
        bool angleNormalPos = newStatus.angleNormal > 0;
        //Debug.Log(Threshold.limInferiorPushAngleX.ToString  Threshold.anglePushX)
        //PUSH
        if (newStatus.handRotation[(int)Hand.Left].z > Threshold.limInferiorPushAngleZ && newStatus.handRotation[(int)Hand.Left].z < Threshold.limSuperiorPushAngleZ 
            && (orientationYPos != angleNormalPos)
            &&  (newStatus.handRotation[(int)Hand.Left].x > Threshold.limInferiorPushAngleX && newStatus.handRotation[(int)Hand.Left].x <Threshold.limSuperiorPushAngleX)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Push;
            return;
        }
        //PULL
        if (newStatus.handRotation[(int)Hand.Left].z > Threshold.limInferiorPullAngleZ && newStatus.handRotation[(int)Hand.Left].z < Threshold.limSuperiorPullAngleZ 
            && (orientationYPos == angleNormalPos)
            && (newStatus.handRotation[(int)Hand.Left].x > Threshold.limInferiorPullAngleX || newStatus.handRotation[(int)Hand.Left].x < Threshold.limSuperiorPullAngleX)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Pull;
            return;
        }
        //BOTH
        if  (newStatus.handRotation[(int)Hand.Left].x > limInferiorBothAngleLeftHandX && newStatus.handRotation[(int)Hand.Left].x < limSuperiorBothAngleLeftHandX)
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Both;
            return; 
        }
        handTypePosition[(int)Hand.Left] = (int)HandRotationType.None;
#if DEBUG_MEMORY
        Debug.Log("getLeftRotationType_End");
#endif
    }

    //In this functions the colors for the movements are determined
    public Material getHandMaterial()
    {
        if (moveDetected)
        {
            switch (idMoveDetected)
            {
                //case (int)HandMoves.RightHandPush:
                //case (int)HandMoves.RightHandPull:
                //case (int)HandMoves.LeftHandPush:
                //case (int)HandMoves.LeftHandPull:
                //case (int)HandMoves.ZoomIn:
                //case (int)HandMoves.ZoomOut:
                //case (int)HandMoves.Traslate:
                case (int)HandMoves.None:
                    {
                        return materials[(int)MaterialColors.Silver];
                    }
                                   
                //Debug not moving
                
                case (int)HandMoves.RightHandPush:
                    {
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
                case (int)HandMoves.Traslate:
                    {
                        return materials[(int)MaterialColors.Brown];
                    }
                case (int)HandMoves.ZoomIn:
                    {
                        return materials[(int)MaterialColors.Pink];
                    }
                case (int)HandMoves.ZoomOut:
                    {
                        return materials[(int)MaterialColors.Purple];
                    }

            }
        }
        else
        {
            //Debug.Log("Not moving");
            //Debug.Log(handTypePosition[(int)Hand.Right].ToString() + " : " + handTypePosition[(int)Hand.Left].ToString());
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Push && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.MateBlue];
            }
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Pull && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.MateCyan];
            }

            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Push))
            {
                return materials[(int)MaterialColors.MateRed];
            }

            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Pull))
            {
                return materials[(int)MaterialColors.MateOrange];
            }
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Both && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Both)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.MateBrown];
            }
            return materials[(int)MaterialColors.MateWhite];        
        }
        return materials[(int)MaterialColors.White];
    }
    
    public void detectMove() 
    {
#if DEBUG_MEMORY
        Debug.Log("detectMove_Start");
#endif
        moveDetected = false;
        idMoveDetected = (int)HandMoves.None;
        //idMoveDetected = (int)HandMoves.None;
        //  TWO HANDS
        if (newStatus.handMoving[(int)Hand.Right] && newStatus.handMoving[(int)Hand.Left])
        {
            moveDetected = true;
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Both && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Both)
            {
                switch (newStatus.distance)
                {
                    case (int)Distance.Bigger:
                        {
                            idMoveDetected = (int)HandMoves.ZoomOut;
                            break;
                        }
                    case (int)Distance.Shorter:
                        {
                            idMoveDetected = (int)HandMoves.ZoomIn;
                            break;
                        }
                    case (int)Distance.Same:
                        {
                            idMoveDetected = (int)HandMoves.Traslate;
                            break;
                        }
                }
            }
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
                            if (newStatus.angleMovement[(int)Hand.Right] > (360 - Threshold.moveDirection) ||
                               newStatus.angleMovement[(int)Hand.Right] < (0 + Threshold.moveDirection))
                               {
                                   idMoveDetected = (int)HandMoves.RightHandPush;
                               }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            //Debug.Log("Pull");
                            if (newStatus.angleMovement[(int)Hand.Right] > (180 - Threshold.moveDirection) &&
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
                            if (newStatus.angleMovement[(int)Hand.Left] > (360 - Threshold.moveDirection) ||
                               newStatus.angleMovement[(int)Hand.Left] < (0 + Threshold.moveDirection))
                            {
                                idMoveDetected = (int)HandMoves.LeftHandPush;
                            }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            if (newStatus.angleMovement[(int)Hand.Left] > (180 - Threshold.moveDirection) &&
                                newStatus.angleMovement[(int)Hand.Left] < (180 + Threshold.moveDirection))
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
        }
        UpdateIdDetected(this, idMoveDetected);
#if DEBUG_MEMORY
        Debug.Log("detectMove_End");
#endif
    }
    /*
    
    */
    private void OnUpdatedHandsStatusReceived(object sender, HandStatus newHandStatus)
    {
#if DEBUG_MEMORY
        Debug.Log("OnUpdatedHandsStatusReceive_Start");
#endif
        globalDef.GetComponent<globalDefinitions>().currentHandStatus = newHandStatus;
        oldStatus = newStatus;
        newStatus = newHandStatus;
        getRightRotationType();
        getLeftRotationType();
        detectMove();

        Material newHandMaterial = getHandMaterial();
        if (newHandMaterial != handMaterials)
        {
            Hands[(int)Hand.Right].GetComponent<HandManager>().changeMaterial(newHandMaterial);
            Hands[(int)Hand.Left].GetComponent<HandManager>().changeMaterial(newHandMaterial);
        }
#if DEBUG_MEMORY
        Debug.Log("OnUpdatedHandsStatusReceive_End");
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

    
}
