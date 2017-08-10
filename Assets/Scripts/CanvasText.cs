using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasText : MonoBehaviour {

    // Use this for initialization
    Text X, Y, Z;
    public GameObject goX, goY, goZ;
    public GameObject RHand, LHand;
    public GameObject globalDef;
    public GameObject DataReceived;
    public bool isRH;
    public bool variance;
    HandStatus currentHandStatus;
    
	void Start () {
        X = goX.GetComponent<Text>();
        Y = goY.GetComponent<Text>();
        Z = goZ.GetComponent<Text>();
        
        //DataReceived
    }
	
	// Update is called once per frame
	void Update () {
        //currentHandStatus = globalDef.GetComponent<globalDefinitions>().currentHandStatus;
        if (variance)
        {
            X.text = "";
            Y.text = ((int)(DataReceived.GetComponent<DataReceived>().rightHandVariance*100000000)).ToString();
            Z.text = "";
            return;
        }
        if (isRH)
        {
            X.text = RHand.transform.rotation.eulerAngles.x.ToString();
            Y.text = RHand.transform.rotation.eulerAngles.y.ToString();
            Z.text = RHand.transform.rotation.eulerAngles.z.ToString();
        }
        else
        {
            X.text = LHand.transform.rotation.eulerAngles.x.ToString();
            Y.text = LHand.transform.rotation.eulerAngles.y.ToString();
            Z.text = LHand.transform.rotation.eulerAngles.z.ToString();
        }
        

    }
}
