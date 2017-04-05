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
    None = -1,
    Push = 0,
    Pull = 1,
    Bouth = 2
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
    public GameObject[] Hands = new GameObject[2];
    public GameObject motiveData;
    public GameObject globalDef;
    HandStatus newStatus;
    HandStatus oldStatus;
    Material handMaterials;
    Material[] materials = new Material[2];
	// Use this for initialization
	void Start ()
    {
	    motiveData.GetComponent<DataReceived>().UpdateHandsStatus+= new ChangeHandsStatusHandler(OnUpdatedHandsStatusReceived);
        materials = globalDef.GetComponent<globalDefinitions>().materials;
    }

    public int getRightRotationType ()
    {
        //Si push
        
        if (  newStatus.handRotation[(int)Hand.Right].z > (Threshold.centerAnglePull+Threshold.angleOneHand) 
            && newStatus.handRotation[(int)Hand.Right].z < (Threshold.centerAnglePull - Threshold.angleOneHand))
        { 
        return (int)HandRotationType.Push;
        }
        if (true)
        { 
        return (int)HandRotationType.Pull;
        }
        if  (true)
        { 
        return (int)HandRotationType.Bouth;
        }
        return (int)HandRotationType.None;
    }

    public int getLeftRotationType()
    {

        return -1;
    }
    public void getColorMaterial()
    {
        //RIGHT
        //int handMovPos = getRightHandRotation();

        ///LEFT

    }
    
    private void OnUpdatedHandsStatusReceived(object sender, HandStatus newHandStatus)
    {
        getColorMaterial();
        oldStatus = newStatus;
        newStatus = newHandStatus;
        Debug.Log(newStatus.handRotation[(int)Hand.Right].z);
        if (newStatus.handMoving[(int)Hand.Right] != oldStatus.handMoving[(int)Hand.Right])
            return;
            //Hands[(int)Hand.Right].GetComponent<HandManager>().changeMaterial(getColorMaterial());
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    
}
