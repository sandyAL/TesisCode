using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

    Vector3 originRay;
    Vector3 direction;
    GameObject target;
    RaycastHit hitInfo;
    public GameObject selectedObject;
    public GameObject globalDef;
    public string selectedObjectName;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Target");
        selectedObjectName = "";
    }
	
	// Update is called once per frame
	void Update () {
        originRay = transform.position;
        direction = transform.forward;
        this.transform.GetChild(0).position = originRay + direction;
        if (Physics.Raycast(originRay, direction, out hitInfo, 100.0f))
        {
            if (selectedObjectName == "") // Si no hay un objeto seleccionado, selecciono el que provoco el hit
            {
                selectedObject = hitInfo.transform.gameObject;
                globalDef.GetComponent<globalDefinitions>().selectedObject = selectedObject;
                //Debug.Log(selectedObject.name);
                selectedObjectName = selectedObject.name;
                globalDef.GetComponent<globalDefinitions>().selectedObjectName = selectedObjectName;
                selectedObject.GetComponent<TargetObject>().isSelected = true;
            }
            else if (selectedObjectName != hitInfo.transform.gameObject.name) // si tengo un objeto seleccionado y es diferente al que hice hit, deselecciono el primero y selecciono el segundo
            {
                selectedObject.GetComponent<TargetObject>().isSelected = false;
                selectedObject = hitInfo.transform.gameObject;
                selectedObjectName = selectedObject.name;
                globalDef.GetComponent<globalDefinitions>().selectedObject = selectedObject;
                globalDef.GetComponent<globalDefinitions>().selectedObjectName = selectedObjectName;
                selectedObject.GetComponent<TargetObject>().isSelected = true;
            }
        }
    }
}
