﻿using System.Collections;
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
	

    private void OnMoveDetectedReceived(object sender, int idMoveDetected)
    {

        Vector3 direction = globalDef.GetComponent<globalDefinitions>().currentHandStatus.handDirection[(int)Hand.Right].normalized;
        switch (idMoveDetected)
        {
            case (int)HandMoves.LeftHandPush:
                {
                    //TODO START SELECTION
                    //Debug.Log("LeftHandPush");
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
            return;
        if (globalDef.GetComponent<globalDefinitions>().selectedObjectName != selectedObjectName)
        {
            selectedObjectName = globalDef.GetComponent<globalDefinitions>().selectedObjectName;
            selectedObject = globalDef.GetComponent<globalDefinitions>().selectedObject;
        }
        switch (idMoveDetected)
        {
            case (int)HandMoves.RightHandPush:
                {
                    selectedObject.transform.position += new Vector3(direction.x / 50.0F, 0, direction.z / 50.0F);
                    return;
                }
            case (int)HandMoves.RightHandPull:
                {
                    selectedObject.transform.position += new Vector3(direction.x / 20.0F, 0, direction.z / 20.0F);
                    return;
                }
            case (int)HandMoves.ZoomIn:
                {
                    selectedObject.transform.localScale = 1.01F*selectedObject.transform.localScale;
                    return;
                }
            case (int)HandMoves.ZoomOut:
                {
                    selectedObject.transform.localScale = 0.9F * selectedObject.transform.localScale;
                    return;
                }
            case (int)HandMoves.Traslate:
                {
                    if (globalDef.GetComponent<globalDefinitions>().selectedObjectName == "")
                        return;

                    return;
                }
            case (int)HandMoves.None:
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