using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour {
    public GameObject handsController;
    public GameObject globalDef;
    public GameObject target;
    public GameObject selector;
    GameObject selectedObject;
	// Use this for initialization
	void Start () {
        handsController.GetComponent<HandsController>().UpdateIdDetected += new idDetectedMovementHandler(OnMoveDetectedReceived);
        selector = globalDef.GetComponent<globalDefinitions>().HeadCamera;
        target = globalDef.GetComponent<globalDefinitions>().Target;
    }
	

    private void OnMoveDetectedReceived(object sender, int idMoveDetected)
    {

        Vector3 direction = globalDef.GetComponent<globalDefinitions>().currentHandStatus.handDirection[(int)Hand.Right].normalized;
        switch (idMoveDetected)
        {
            case (int)HandMoves.RightHandPush:
                {
                    // TODO PUSH SELECTED OBJECT
                    selectedObject = globalDef.GetComponent<globalDefinitions>().selectedObject;
                    selectedObject.transform.position += new Vector3(direction.x/10.0F,0, direction.z/10.0F);
                    //testObject.transform.position += direction.normalized;
                    return;
                }
            case (int)HandMoves.RightHandPull:
                {
                    //TODO PULL SELECTED OBJECT
                    selectedObject = globalDef.GetComponent<globalDefinitions>().selectedObject;
                    selectedObject.transform.position += new Vector3(direction.x / 10.0F, 0, direction.z / 10.0F);
                    return;
                }
            case (int)HandMoves.LeftHandPush:
                {
                    //TODO START SELECTION
                    selector.GetComponent<Selector>().enabled = false;
                    target.SetActive(false);
                    return;
                }
            case (int)HandMoves.LeftHandPull:
                {
                    //TODO END SELECTION
                    selector.GetComponent<Selector>().enabled = true;
                    target.SetActive(true);
                    return;
                }
            case (int)HandMoves.None:
            case (int)HandMoves.ZoomIn:
            case (int)HandMoves.ZoomOut:
            case (int)HandMoves.Traslate:
            default:
                {
                    break;
                }
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
