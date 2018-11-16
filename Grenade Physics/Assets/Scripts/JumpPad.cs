using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    public Vector3 PushDirection = Vector3.up;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            //Debug.Log("entered");
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
                player.knockBackVel = PushDirection;
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("player collider operating");
        }

    }*/
}
