//#define DEBUG_MEMORY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveAction : MonoBehaviour {
    public GameObject handsController;
    public GameObject globalDef;
    GameObject target;
    GameObject selector;
    GameObject selectedObject;
    string selectedObjectName;
	// Use this for initialization
	void Start () {
        handsController.GetComponent<HandsController>().UpdateIdDetected += new idDetectedMovementHandler(OnMoveDetectedReceived);
        selector = globalDef.GetComponent<globalDefinitions>().HeadCamera;
        target = globalDef.GetComponent<globalDefinitions>().Target;
    }

    public float stepPosition;
    public Vector3 stepScale;
    private void OnMoveDetectedReceived(object sender, int idMoveDetected)
    {
        //Debug.Log(idMoveDetected);
#if DEBUG_MEMORY
        Debug.Log("OnMoveDetected");
#endif
        Vector3 direction = globalDef.GetComponent<globalDefinitions>().currentHandStatus.handDirection[(int)Hand.Right].normalized;
        switch (idMoveDetected)
        {
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
                
        }
        if (globalDef.GetComponent<globalDefinitions>().selectedObjectName == "")
        {
#if DEBUG_MEMORY
            Debug.Log("OnMoveDetected_End_NoObject");
#endif
            return;
        }
        if (globalDef.GetComponent<globalDefinitions>().selectedObjectName != selectedObjectName)
        {
            selectedObjectName = globalDef.GetComponent<globalDefinitions>().selectedObjectName;
            selectedObject = globalDef.GetComponent<globalDefinitions>().selectedObject;
        }
        switch (idMoveDetected)
        {
            case (int)HandMoves.RightHandPush:
                {
                    selectedObject.transform.position += new Vector3(selector.transform.forward.x / stepPosition, 0, selector.transform.forward.z / stepPosition);

                    return;
                }
            case (int)HandMoves.RightHandPull:
                {
                    selectedObject.transform.position += new Vector3(-selector.transform.forward.x / stepPosition, 0, -selector.transform.forward.z / stepPosition);//new Vector3(direction.x / stepPosition, 0, direction.z / stepPosition);
                    return;
                }
            case (int)HandMoves.ZoomIn:
                {
                    selectedObject.transform.localScale = 0.99F * selectedObject.transform.localScale; //- stepScale;
                    return;
                }
            case (int)HandMoves.ZoomOut:
                {
                    selectedObject.transform.localScale = stepScale + selectedObject.transform.localScale;
                    return;
                }
                
            case (int)HandMoves.RightHandDown:
                {
                    selectedObject.transform.position += Vector3.down/ stepPosition;
                    return;
                }
            case (int)HandMoves.RightHandUp:
                {
                    selectedObject.transform.position += Vector3.up/ stepPosition;
                    return;
                }
                
            case (int)HandMoves.LeftHandDown:
                {
                    selectedObject.transform.RotateAround(selectedObject.transform.position, new Vector3(selector.transform.right.x, 0, selector.transform.right.y), -1.0f);
                    return;
                }
            case (int)HandMoves.LeftHandUp:
                {
                    selectedObject.transform.RotateAround(selectedObject.transform.position, new Vector3(selector.transform.right.x,0, selector.transform.right.y),1.0f);
                    return;
                }
            case (int)HandMoves.RighHandSide:
                {
                    selectedObject.transform.position += -selector.transform.right/ stepPosition  ;
                    return;
                }
            case (int)HandMoves.LeftHandSide:
                {
                    selectedObject.transform.position += selector.transform.right/ stepPosition ;
                    return;
                }
            case (int)HandMoves.None:
                {
                    
                    return;
                }
            default:
                {
                    break;
                }
        }
#if DEBUG_MEMORY
        Debug.Log("OnMoveDetected_End");
#endif
    }
    // Update is called once per frame
    void Update () {
		
	}
}
