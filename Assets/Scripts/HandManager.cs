using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    public GameObject renderedHand;
	// Use this for initialization
	void Start () {
		
	}
	
    public void changeMaterial(Material newMaterial)
    {
        GameObject hand = this.transform.GetChild(0).gameObject;
        int numChildren = hand.transform.childCount;
        int numC;
        for (int k = 0; k < numChildren; k += 1)
        {
            Transform child = hand.transform.GetChild(k);
            child.gameObject.GetComponent<Renderer>().material = newMaterial;
            numC = child.transform.childCount;
            for (int i = 0; i < numC; i += 1)
            {
                Transform grandChild = child.transform.GetChild(i);
                grandChild.gameObject.GetComponent<Renderer>().material = newMaterial;
            }
        }
        
    }


	// Update is called once per frame
	void Update () {
		
	}
}
