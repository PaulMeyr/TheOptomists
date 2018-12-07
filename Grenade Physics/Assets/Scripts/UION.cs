using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UION : MonoBehaviour {
    public GameObject UI;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UI.gameObject.SetActive(true);
    }
}
