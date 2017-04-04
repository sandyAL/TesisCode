using UnityEngine;
using System.Collections;

public class HandStatus
{
    public int id;
    public bool[] handMoving { get; set; }
    public int distance { get; set; }
    public float [] angleMovement;
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
        angleMovement = new float [2] { -1.0F,-1.0F};
        handDirection = new Vector3[2] { Vector3.zero, Vector3.zero };
        handPosition = new Vector3[2] { Vector3.zero, Vector3.zero };
        handRotation = new Vector3[2] { Vector3.zero, Vector3.zero };

    }
    public void update(HandStatus newStatus)
    {
        id = newStatus.id;
        distance = newStatus.distance;
        for(int i=0;i<2;i++)
        {
            handMoving[i] = newStatus.handMoving[i];
            handDirection[i] = newStatus.handDirection[i];
            handPosition[i] = newStatus.handPosition[i];
            handRotation[i] = newStatus.handRotation[i];
        }
    }
}

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
 
enum Hand : int
{
    Right = 0,
    Left = 1
};

enum Distance: int
{
    Shorter = -1,
    Same = 0,
    Bigger = 1
}
public struct Threshold
{
    public const double limInferiorVariance = 0.0001;
    public const double limSuperiorVariance = 0.0007;
    public const double limInferiorDistance = 0.001;
    public const double limSuperiorDistance = 0.01;
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
    public Material[] materials =new Material[10];
    public GameObject Head;
    public GameObject HeadCamera;
    public GameObject[] Hands;
    public string selectedObjectName = "";
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
