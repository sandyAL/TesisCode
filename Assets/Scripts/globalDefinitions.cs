
#define DEBUG_MEMORY

using UnityEngine;
using System.Collections;



enum MaterialColors : int {
    White = 0,
    BlueMate,
    Blue ,
    BrownMate,
    Brown,
    CyanMate,
    Cyan,
    GreenMate,
    Green,
    LavanderMate,
    Lavander,
    LimeMate,
    Lime,
    MagentaMate,
    Magenta,
    OrangeMate,
    Orange,
    PinkMate,
    Pink,
    PurpleMate,
    Purple,
    RedMate,
    Red,
    SilverMate,
    Silver,
    YellowMate,
    Yellow,
    WhiteMate,
    

};
 

public struct Threshold
{
    //VARIANCE
    public const double limInferiorVariance = 0.00005; //0.0001 //Original
    public const double limSuperiorVariance = 0.0007; //7*10^-4

    //DISTANCE
    public const double limInferiorDistance = -0.01;
    public const double limSuperiorDistance = 0.01;

    //PUSH
    public const double limInferiorPushAngleZ = 260;
    public const double limSuperiorPushAngleZ = 320;

    public const double limInferiorPushAngleY = 0;
    public const double limSuperiorPushAngleY = 40;

    public const double limInferiorPushAngleX = 340; // -20;
    public const double limSuperiorPushAngleX= 20;

    //PULL
    public const double limInferiorPullAngleZ = 230;
    public const double limSuperiorPullAngleZ = 320;
    
    public const double limInferiorPullAngleY = 160;
    public const double limSuperiorPullAngleY = 200; 

    public const double limInferiorPullAngleX = 90; 
    public const double limSuperiorPullAngleX = 120;


    //TOP
    public const double limInferiorTopAngleZ = 340; //-20
    public const double limSuperiorTopAngleZ = 20;

    //public const double limInferiorTopAngleY = 0;
    //public const double limSuperiorTopAngleY = 0;

    public const double limInferiorTopAngleX = 340; // -20;
    public const double limSuperiorTopAngleX = 20;

    //BOTTOM
    public const double limInferiorBottomAngleZ = 160;
    public const double limSuperiorBottomAngleZ = 200;

    //public const double limInferiorBottomAngleY = 0;
    //public const double limSuperiorBottomAngleY = 0;

    public const double limInferiorBottomAngleX = 340;
    public const double limSuperiorBottomAngleX = 20;


    //ZOOM IN 
    public const float limInferiorZoomInAngleRightHandX = 50;
    public const float limSuperiorZoomInAngleRightHandX = 100;
    public const float limInferiorZoomInAngleLeftHandX = 270;
    public const float limSuperiorZoomInAngleLeftHandX = 300;

    public const float limInferiorZoomInAngleRightHandYZ = 0;
    public const float limSuperiorZoomInAngleRightHandYZ = 20;
    public const float limInferiorZoomInAngleLeftHandYZ = 0;
    public const float limSuperiorZoomInAngleLeftHandYZ = 20;

    //public const float limInferiorZoomInAngleRightHandZ = 0;
    //public const float limSuperiorZoomInAngleRightHandZ = 0;
    //public const float limInferiorZoomInAngleLeftHandZ = 0;
    //public const float limSuperiorZoomInAngleLeftHandZ = 0;

    //ZOOM OUT

    public const float limInferiorZoomOutAngleRightHandX = 290;
    public const float limSuperiorZoomOutAngleRightHandX = 340;
    public const float limInferiorZoomOutAngleLeftHandX = 20;
    public const float limSuperiorZoomOutAngleLeftHandX = 60;

    public const float limInferiorZoomOutAngleRightHandYZ = 0;
    public const float limSuperiorZoomOutAngleRightHandYZ = 40;
    public const float limInferiorZoomOutAngleLeftHandYZ = 0;
    public const float limSuperiorZoomOutAngleLeftHandYZ = 40;

    //public const float limInferiorZoomOutAngleRightHandZ = 0;
    //public const float limSuperiorZoomOutAngleRightHandZ = 0;
    //public const float limInferiorZoomOutAngleLeftHandZ = 0;
    //public const float limSuperiorZoomOutAngleLeftHandZ = 0;

    //REST
    public const double limInferiorRestAngleZ = 60;
    public const double limSuperiorRestAngleZ = 120;

    public const double limInferiorRestAngleX = 330; // -20;
    public const double limSuperiorRestAngleX = 30;

    public const int angleOneHand =40;
    public const int angleTwoHands = 30;
    public const int moveDirection = 45;

    public const int anglePushMoveInferior = 360 - moveDirection;
    public const int anglePushMoveSuperior = 0 + moveDirection;
    public const int anglePullMoveInferior = 180 - moveDirection;
    public const int anglePullMoveSuperior = 180 + moveDirection;
    public const int angleTopMoveInferior = 90 - moveDirection;
    public const int angleTopMoveSuperior = 90 + moveDirection;
    public const int angleBottomMoveInferior = 90 - moveDirection;
    public const int angleBottomMoveSuperior = 90 + moveDirection;
    public const int angleTranslateMoveInferior = 90 - moveDirection;
    public const int angleTranslateMoveSuperior = 90 + moveDirection;

}

public struct constant
{
    public const int factor = 1;
    public const int windowSize = 10;
    public const int sameFrameWindowSize = 10;
    public const double headSize = 0.15;
}



public delegate void ChangeHandsStatusHandler(object sender, HandStatus handStatus);
public delegate void idDetectedMovementHandler(object sender, int idMoveDetected);

public class globalDefinitions : MonoBehaviour {
    public GameObject Head;
    public GameObject HeadCamera;
    public GameObject Target;
    public GameObject[] Hands = new GameObject[2];
    public HandStatus currentHandStatus;
    public Material[] materials =new Material[28];
    public GameObject selectedObject;
    public string selectedObjectName = "";
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


///////////////////////////////
//PRUEBAS
///////////////////////////////
/*
float angleYZ = (360 + newStatus.handRotation[(int)Hand.Right].y - newStatus.handRotation[(int)Hand.Right].z) % 360;
angleYZ = (angleYZ + 270) % 360;
angleYZ = angleYZ > 180 ? angleYZ - 360 : angleYZ;
float angle = Mathf.Abs(angleYZ - newStatus.angleNormal);
Debug.Log(angle);
int a1=20, a2=20, a3=20, a4=20, a5=50;
if (angle>a1 && angle<a2)
{
    handTypePosition[(int)Hand.Right] = (int)HandRotationType.Push;
    handTypePosition[(int)Hand.Left] = (int)HandRotationType.None;
    return;
}
if (angle >a2 && angle <a3)
{
    handTypePosition[(int)Hand.Right] = (int)HandRotationType.Pull;
    handTypePosition[(int)Hand.Left] = (int)HandRotationType.None;
    return;
}
if (angle >a3 && angle <a4)
{
    handTypePosition[(int)Hand.Right] = (int)HandRotationType.ZoomIn;
    handTypePosition[(int)Hand.Left] = (int)HandRotationType.ZoomIn;
    return;
}
if (angle >a4 && angle <a5)
{
    handTypePosition[(int)Hand.Right] = (int)HandRotationType.ZoomOut;
    handTypePosition[(int)Hand.Left] = (int)HandRotationType.ZoomOut;
    return;
}
handTypePosition[(int)Hand.Right] = (int)HandRotationType.None;
return;//*/
///////////////////////////////
///////////////////////////////