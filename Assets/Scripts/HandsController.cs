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
    Push,
    Pull,
    ZoomIn ,
    ZoomOut ,
    Top,
    Bottom,
    Translate,
    Rest
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
    RightHandDown=6,
    RightHandUp=7,
    LeftHandDown=8,
    LeftHandUp=9,
    RighHandSide=10,
    LeftHandSide=11
        
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

        //TODO move to threshold
    /*public float limInferiorBothAngleRightHandX;
    public float limSuperiorBothAngleRightHandX;
    public float limInferiorBothAngleLeftHandX;
    public float limSuperiorBothAngleLeftHandX;
    public float limInferiorZoomInAngleRightHandX;
    public float limSuperiorZoomInAngleRightHandX;
    public float limInferiorZoomInAngleLeftHandX;
    public float limSuperiorZoomInAngleLeftHandX;
    public float limInferiorZoomOutAngleRightHandX;
    public float limSuperiorZoomOutAngleRightHandX;
    public float limInferiorZoomOutAngleLeftHandX;
    public float limSuperiorZoomOutAngleLeftHandX;*/
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

    public void getRightRotationType()
    {

#if DEBUG_MEMORY
        Debug.Log("getRightRotationType_Start");
#endif
        //Debug.Log(newStatus.handRotation[(int)Hand.Right]);
        //Debug.Log(newStatus.handRotation[(int)Hand.Right].x);
        //Debug.Log(newStatus.handRotation[(int)Hand.Right].y);
        //Debug.Log(newStatus.handRotation[(int)Hand.Right].z);
        float X = newStatus.handRotation[(int)Hand.Right].x;
        float Y = newStatus.handRotation[(int)Hand.Right].y;
        float Z = newStatus.handRotation[(int)Hand.Right].z;

        float angleY;
        angleY = (Y + 270) % 360;
        angleY = angleY > 180 ? angleY - 360  : angleY; 
        float diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
        //Debug.Log(diferenceYNormal);
        //Debug.Log(angleY.ToString()+" + " + newStatus.angleNormal.ToString() + " = " + diferenceYNormal.ToString()
        //PUSH
        if (Z > Threshold.limInferiorPushAngleZ && Z < Threshold.limSuperiorPushAngleZ
            && diferenceYNormal > Threshold.limInferiorPushAngleY && diferenceYNormal < Threshold.limSuperiorPushAngleY
            && (X > Threshold.limInferiorPushAngleX || X < Threshold.limSuperiorPushAngleX)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Push;
            return;
        }
        //PULL
        if (Z > Threshold.limInferiorPullAngleZ && Z < Threshold.limSuperiorPullAngleZ
            && diferenceYNormal > Threshold.limInferiorPullAngleY && diferenceYNormal < Threshold.limSuperiorPullAngleY
            && (X > Threshold.limInferiorPullAngleX || X < Threshold.limSuperiorPullAngleX)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Pull;  
            return;
        }
        
        //TOP
        if((X > Threshold.limInferiorTopAngleX || X < Threshold.limSuperiorTopAngleX)
            && (Z > Threshold.limInferiorTopAngleZ || Z < Threshold.limSuperiorTopAngleZ)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Top;
            return;
        }
        //BOTTOM
        if ((X > Threshold.limInferiorBottomAngleX || X < Threshold.limSuperiorBottomAngleX)
            && (Z > Threshold.limInferiorBottomAngleZ && Z < Threshold.limSuperiorBottomAngleZ)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Bottom;
            return;
        }

        //SIDE
        /*Debug.Log(diferenceYNormal);
        if (Z > Threshold.limInferiorPushAngleZ && Z < Threshold.limSuperiorPushAngleZ
            && diferenceYNormal > Threshold.limInferiorPushAngleY && diferenceYNormal < Threshold.limSuperiorPushAngleY
            && (X > Threshold.limInferiorPushAngleX || X < Threshold.limSuperiorPushAngleX)
            )
        {
            ;
        }*/
            //ZoomIn
            if (X > Threshold.limInferiorZoomInAngleRightHandX && X < Threshold.limSuperiorZoomInAngleRightHandX)
        {
            angleY = (360 + Y - Z) % 360;
            angleY = (angleY + 270) % 360;
            angleY = angleY > 180 ? angleY - 360 : angleY;
            diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
            if (diferenceYNormal > Threshold.limInferiorZoomInAngleRightHandYZ && diferenceYNormal < Threshold.limSuperiorZoomInAngleRightHandYZ)
            {
                handTypePosition[(int)Hand.Right] = (int)HandRotationType.ZoomIn;
                return;
            }

        }
        //ZoomOut
        if (X > Threshold.limInferiorZoomOutAngleRightHandX && X < Threshold.limSuperiorZoomOutAngleRightHandX)
        {
            //Debug.Log("Zoom out Right x");
            angleY = (Y + Z) % 360;
            angleY = (angleY + 270) % 360;
            angleY = angleY > 180 ? angleY - 360 : angleY;
            diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
            //Debug.Log(diferenceYNormal);
            if (diferenceYNormal > Threshold.limInferiorZoomOutAngleRightHandYZ && diferenceYNormal < Threshold.limSuperiorZoomOutAngleRightHandYZ)
            {
                //Debug.Log("Zoom out right yz");
                handTypePosition[(int)Hand.Right] = (int)HandRotationType.ZoomOut;
                return;
            }
           
        }
        //Rest
        if ((X > Threshold.limInferiorRestAngleX || X < Threshold.limSuperiorRestAngleX)
            && (Z > Threshold.limInferiorRestAngleZ && Z < Threshold.limSuperiorRestAngleZ)
            )
        {
            handTypePosition[(int)Hand.Right] = (int)HandRotationType.Rest;
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

        //Debug.Log(newStatus.handRotation[(int)Hand.Left].x);
        //Debug.Log(newStatus.handRotation[(int)Hand.Left].y);
        //Debug.Log(newStatus.handRotation[(int)Hand.Left].z);
        float X = newStatus.handRotation[(int)Hand.Left].x;
        float Y = newStatus.handRotation[(int)Hand.Left].y;
        float Z = newStatus.handRotation[(int)Hand.Left].z;
        float angleY;
        angleY = (newStatus.handRotation[(int)Hand.Left].y + 270) % 360;
        angleY = angleY > 180 ? angleY - 360 : angleY;
        float diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
        //Debug.Log(diferenceYNormal);
        //Debug.Log(Threshold.limInferiorPushAngleX.ToString  Threshold.anglePushX)
        //Debug.Log(angleY.ToString()+" + " + newStatus.angleNormal.ToString() + " = " + diferenceYNormal.ToString());
        
        //PUSH
        if (Z > Threshold.limInferiorPushAngleZ && Z < Threshold.limSuperiorPushAngleZ
            && diferenceYNormal > Threshold.limInferiorPushAngleY && diferenceYNormal < Threshold.limSuperiorPushAngleY
            && (X > Threshold.limInferiorPushAngleX || X <Threshold.limSuperiorPushAngleX)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Push;
            return;
        }
        
        //PULL
        if (Z > Threshold.limInferiorPullAngleZ && Z < Threshold.limSuperiorPullAngleZ
            && diferenceYNormal > Threshold.limInferiorPullAngleY && diferenceYNormal < Threshold.limSuperiorPullAngleY
            && (X > Threshold.limInferiorPullAngleX || X < Threshold.limSuperiorPullAngleX)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Pull;
            return;
        }
        //*
        //TOP
        if ((X > Threshold.limInferiorTopAngleX || X < Threshold.limSuperiorTopAngleX)
            && (Z > Threshold.limInferiorTopAngleZ || Z < Threshold.limSuperiorTopAngleZ)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Top;
            return;
        }
        //BOTTOM
        if ((X > Threshold.limInferiorBottomAngleX || X < Threshold.limSuperiorBottomAngleX)
            && (Z > Threshold.limInferiorBottomAngleZ && Z < Threshold.limSuperiorBottomAngleZ)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Bottom;
            return;
        }
        //*/
        //TRANSLATE


        //ZOOMIN
        if (X > Threshold.limInferiorZoomInAngleLeftHandX && X < Threshold.limSuperiorZoomInAngleLeftHandX)
        {
            angleY = (Y + Z) % 360;
            angleY = (angleY + 270) % 360;
            angleY = angleY > 180 ? angleY - 360 : angleY;
            diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
            if (diferenceYNormal > Threshold.limInferiorZoomInAngleLeftHandYZ && diferenceYNormal < Threshold.limSuperiorZoomInAngleLeftHandYZ)
            {
                handTypePosition[(int)Hand.Left] = (int)HandRotationType.ZoomIn;
                return;
            }

        }


        //ZOOMOUT
        if (X > Threshold.limInferiorZoomOutAngleLeftHandX && X < Threshold.limSuperiorZoomOutAngleLeftHandX)
        {
            //Debug.Log("Zoom out Left x ");
            angleY = (360 + Y - Z) % 360;
            angleY = (angleY + 270) % 360;
            angleY = angleY > 180 ? angleY - 360 : angleY;
            diferenceYNormal = Mathf.Abs(angleY - newStatus.angleNormal);
            //Debug.Log(diferenceYNormal);
            if (diferenceYNormal > Threshold.limInferiorZoomOutAngleLeftHandYZ && diferenceYNormal < Threshold.limSuperiorZoomOutAngleLeftHandYZ)
            {
                //Debug.Log("Zoom out Left yz");
                handTypePosition[(int)Hand.Left] = (int)HandRotationType.ZoomOut;
                return;
            }
        }

        if ((X > Threshold.limInferiorRestAngleX || X < Threshold.limSuperiorRestAngleX)
            && (Z > Threshold.limInferiorRestAngleZ && Z < Threshold.limSuperiorRestAngleZ)
            )
        {
            handTypePosition[(int)Hand.Left] = (int)HandRotationType.Rest;
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
                case (int)HandMoves.ZoomIn:
                    {
                        return materials[(int)MaterialColors.Magenta];
                    }
                case (int)HandMoves.ZoomOut:
                    {
                        return materials[(int)MaterialColors.Pink];
                    }
                
                case (int)HandMoves.RightHandDown:
                    {
                        return materials[(int)MaterialColors.Lavander];
                    }
                case (int)HandMoves.RightHandUp:
                    {
                        return materials[(int)MaterialColors.Purple];
                    }
                case (int)HandMoves.LeftHandDown:
                    {
                        return materials[(int)MaterialColors.Lime];
                    }
                case (int)HandMoves.LeftHandUp:
                    {
                        return materials[(int)MaterialColors.Green];
                    }
                /*case (int)HandMoves.RighHandSide:
                    {
                        return materials[(int)MaterialColors.Brown];
                    }
                case (int)HandMoves.LeftHandSide:
                    {
                        return materials[(int)MaterialColors.Brown];
                    }*/
                default:
                    {
                        return materials[(int)MaterialColors.White];
                    }
            }
        }
        else
        {
            //Debug.Log("Not moving");
            //Debug.Log(handTypePosition[(int)Hand.Right].ToString() + " : " + handTypePosition[(int)Hand.Left].ToString());
            //RH PUSS
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Push && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest))
            {
                return materials[(int)MaterialColors.BlueMate];
            }
            //RH PULL
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Pull && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest))
            {
                return materials[(int)MaterialColors.CyanMate];
            }
            //LH PUSH
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Rest && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Push))
            {
                return materials[(int)MaterialColors.RedMate];
            }
            //LH PULL
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Rest && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Pull))
            {
                return materials[(int)MaterialColors.OrangeMate];
            }
            // ZOOM IN
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.ZoomIn && handTypePosition[(int)Hand.Left] == (int)HandRotationType.ZoomIn)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.MagentaMate];
            }
            //ZOOM OUT
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.ZoomOut && handTypePosition[(int)Hand.Left] == (int)HandRotationType.ZoomOut)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.PinkMate];
            }
            
            // RH UP
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Bottom && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.PurpleMate];
            }
            // RH DOWN
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Top && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.LavanderMate];
            }
            // RH TRANSLATE
            /*if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.ZoomIn && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.BrownMate];
            }*/

            // LH UP
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Rest && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Bottom)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.GreenMate];
            }
            // LH DOWN
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Rest && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Top)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.LimeMate];
            }
            // LH TRANSLATE
            /*if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.None && handTypePosition[(int)Hand.Left] == (int)HandRotationType.ZoomIn)// && (handTypePosition[(int)Hand.Left] == (int)HandRotationType.None))
            {
                return materials[(int)MaterialColors.BrownMate];
            }
            */

            /*// REST
            if (handTypePosition[(int)Hand.Right]==(int)HandRotationType.Rest || handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest)
            {
                return materials[(int)MaterialColors.BrownMate];
            }//*/

                return materials[(int)MaterialColors.WhiteMate];        
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
            //if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Both && handTypePosition[(int)Hand.Left] == (int)HandRotationType.Both)
            //{
                switch (newStatus.distance)
                {
                    case (int)Distance.Bigger:
                        {
                            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.ZoomOut && handTypePosition[(int)Hand.Left] == (int)HandRotationType.ZoomOut)
                                { 
                                idMoveDetected = (int)HandMoves.ZoomOut;
                                }
                            break;
                        }
                    case (int)Distance.Shorter:
                        {
                        if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.ZoomIn && handTypePosition[(int)Hand.Left] == (int)HandRotationType.ZoomIn)
                            {
                            idMoveDetected = (int)HandMoves.ZoomIn;
                            }
                            break;
                        }
                    
                }
            //}
        }

        // RIGHT HAND
        if(newStatus.handMoving[(int)Hand.Right] && !newStatus.handMoving[(int)Hand.Left])
        {
            moveDetected = true;
            if (handTypePosition[(int)Hand.Left] == (int)HandRotationType.Rest)
            {
                float angleMovement = newStatus.angleMovement[(int)Hand.Right];
                switch (handTypePosition[(int)Hand.Right])
                {
                    case (int)HandRotationType.Push:
                        {
                            if (angleMovement > Threshold.anglePushMoveInferior ||
                               angleMovement < Threshold.anglePushMoveSuperior)
                               {
                                   idMoveDetected = (int)HandMoves.RightHandPush;
                               }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            if (angleMovement > Threshold.anglePullMoveInferior &&
                                angleMovement < Threshold.anglePullMoveSuperior)
                              {
                                idMoveDetected = (int)HandMoves.RightHandPull;
                              }
                            break;
                        }
                    
                case (int)HandRotationType.Top:
                    {
                        if (angleMovement > Threshold.angleTopMoveInferior &&
                            angleMovement < Threshold.angleTopMoveSuperior
                            && newStatus.distance== (int)Distance.Shorter
                            )
                        {
                            idMoveDetected = (int)HandMoves.RightHandDown;
                        }
                        break;
                    }
                    
                    
            case (int)HandRotationType.Bottom:
                {
                    if (angleMovement > Threshold.angleTopMoveInferior &&
                        angleMovement < Threshold.angleTopMoveSuperior
                        && newStatus.distance == (int)Distance.Bigger
                        )
                    {
                        idMoveDetected = (int)HandMoves.RightHandUp;
                    }
                        break;
                }

                    /* 
                case (int)HandRotationType.Translate:
                case (int)HandRotationType.ZoomIn:
                    {
                        if (angleMovement > Threshold.angleTranslateMoveInferior &&
                            angleMovement < Threshold.angleTranslateMoveSuperior
                            && newStatus.distance == (int)Distance.Shorter
                            )
                        {
                            idMoveDetected = (int)HandMoves.RighHandSide;

                        }
                        break;
                    }
                    //*/
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
            if (handTypePosition[(int)Hand.Right] == (int)HandRotationType.Rest)
            {
                float angleMovement = newStatus.angleMovement[(int)Hand.Left];
                switch (handTypePosition[(int)Hand.Left])
                {
                    case (int)HandRotationType.Push:
                        {
                            if (angleMovement > Threshold.anglePushMoveInferior ||
                               angleMovement < Threshold.anglePushMoveSuperior)
                            {
                                idMoveDetected = (int)HandMoves.LeftHandPush;
                            }
                            break;
                        }
                    case (int)HandRotationType.Pull:
                        {
                            if (angleMovement > Threshold.anglePullMoveInferior &&
                                angleMovement < Threshold.anglePullMoveSuperior)
                            {
                                idMoveDetected = (int)HandMoves.LeftHandPull;
                            }
                            break;
                        }
                    //*
                case (int)HandRotationType.Top:
                    {
                        if (angleMovement > Threshold.angleTopMoveInferior &&
                            angleMovement < Threshold.angleTopMoveSuperior
                            && newStatus.distance == (int)Distance.Shorter
                            )
                        {
                            idMoveDetected = (int)HandMoves.LeftHandDown;
                        }
                        break;
                    }
                    //*/
                    //*
                case (int)HandRotationType.Bottom:
                    {
                        if (angleMovement > Threshold.angleTopMoveInferior &&
                            angleMovement < Threshold.angleTopMoveSuperior
                            && newStatus.distance == (int)Distance.Bigger
                            )
                        {
                            idMoveDetected = (int)HandMoves.LeftHandUp;
                        }
                        break;
                    } //*/
                    /*
                    case (int)HandRotationType.Translate:
                    case (int)HandRotationType.ZoomIn:
                        {
                            if (angleMovement > Threshold.angleTranslateMoveInferior &&
                                angleMovement < Threshold.angleTranslateMoveSuperior
                                && newStatus.distance == (int)Distance.Shorter
                                )
                            {
                                idMoveDetected = (int)HandMoves.LeftHandSide;

                            }
                            break;
                        }
                        //*/
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
        //Debug.Log(handTypePosition[0].ToString() + ":" + handTypePosition[1].ToString());
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
