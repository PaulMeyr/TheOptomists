using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPitchRotation : MonoBehaviour {

    public GameObject target = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target!=null)
        {
            this.transform.localRotation = Quaternion.Euler(target.transform.localRotation.eulerAngles.x, 0, 0);
        }
	}
}
