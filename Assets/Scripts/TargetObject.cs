using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour {
    public bool isViewed;
    public bool isSelected;
    Material originalMaterial;
    Material selectedMaterial;
    string currentMaterial;
    // Use this for initialization
    void Start () {
        isViewed = false;
        isSelected = false;
        currentMaterial = "originalMaterial";
        originalMaterial = GetComponent<Renderer>().material;
        selectedMaterial = GameObject.Find("GlobalDefinitions").GetComponent<globalDefinitions>().materials[(int)MaterialColors.Pink];
    }
	
	// Update is called once per frame
	void Update () {
        if (isSelected && currentMaterial == "originalMaterial")
        {
            GetComponent<Renderer>().material = selectedMaterial;
            currentMaterial = "selectedMaterial";
            return;
        }
        if (!isSelected && currentMaterial == "selectedMaterial")
        {
            GetComponent<Renderer>().material = originalMaterial;
            currentMaterial = "originalMaterial";
            return;
        }
    }
}
