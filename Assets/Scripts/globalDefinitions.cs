using UnityEngine;
using System.Collections;


enum MaterialColors : int {
    White = 0,
    Blue,
    Brown,
    Cyan,
    Green,
    Orange,
    Purple,
    Red,
    Silver,
    Yellow
};
 

public struct Threshold
{
    public const double limInferiorVariance = 0.0001;
    public const double limSuperiorVariance = 0.0007;
    public const double limInferiorDistance = 0.001;
    public const double limSuperiorDistance = 0.01;
    public const int angleOneHand =40;
    public const int angleTwoHands = 30;
    public const int direction = 45;
    public const int centerAnglePush = 277;
    public const int centerAnglePull = 260; //TODO calculate
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
