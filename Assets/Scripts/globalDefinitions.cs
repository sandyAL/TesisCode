using UnityEngine;
using System.Collections;


enum MaterialColors : int {
    White = 0,
    Blue = 1,
    Brown =2,
    Cyan = 3,
    Green =4,
    Orange =5,
    Purple = 6,
    Red =7,
    Silver =8,
    Yellow = 9,
    LightBlue = 10,
    LightBrown = 2,
    LightCyan = 3,
    LightGreen = 4,
    LightOrange = 5,
    LightPurple = 6,
    LightRed = 7,
    LightYellow = 9,
};
 

public struct Threshold
{
    public const double limInferiorVariance = 0.0001;
    public const double limSuperiorVariance = 0.0007;
    public const double limInferiorDistance = 0.001;
    public const double limSuperiorDistance = 0.01;
    public const int angleOneHand =40;
    public const int angleTwoHands = 30;
    public const int moveDirection = 45;
    public const int centerAnglePushZ = 315;
    public const int centerAnglePullZ = 240; //TODO calculate
    public const int centerAngleBouth = 0; //TODO calculate
}

public struct constant
{
    public const int factor = 1;
    public const int windowSize = 10;
    public const int sameFrameWindowSize = 10;
    public const double headSize = 0;
}



public delegate void ChangeHandsStatusHandler(object sender, HandStatus handStatus);


public class globalDefinitions : MonoBehaviour {
    public GameObject Head;
    public GameObject HeadCamera;
    public GameObject[] Hands = new GameObject[2];
    public Material[] materials =new Material[10];
    public string selectedObjectName = "";
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
