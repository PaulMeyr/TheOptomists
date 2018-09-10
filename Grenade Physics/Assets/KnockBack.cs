using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour {

    Collider gameObjectCollider;                                                                        //Creating the gameObjectCollider varible
    

    void Start()
    {
         
          gameObjectCollider = GetComponent<Collider>();                                                //Getting the gameObject's collider and adding it as the gameObjectCollider for later use
    }
    
    void OnCollisionEnter(Collision col )                                                               //Testing for inital Collision
    {
        
        gameObjectCollider.isTrigger = true;                                                            //turning the gameObject's collider into a trigger. This prevents the inital collision detection from happening multiple times and allows us to use the onTrigger stay function 
                                                                                                        //A current bug is that if the gameobject collides with 2 objects at the same time the gameobject is scaled up in size twice (possible solution is hard setting game object size instead of scaling)
        //  Destroy(this.gameObject);
        transform.localScale += new Vector3(1f, .5f, 1f);                                               //expanding grenade object into an explosion (instead of creating a new object that is a collision the grenade becomes the explosion )
        gameObjectCollider.attachedRigidbody.useGravity = false;                                        //stops using gravity
        gameObjectCollider.attachedRigidbody.constraints = RigidbodyConstraints.FreezePosition;         //fixes object in place

    }

    void OnTriggerStay (Collider other)                                                                 //The function that adds the forces to move an object away from the explosion (This is  a little messy and could be cleaned up)
    {
        Vector3 direction = other.transform.position - gameObjectCollider.transform.position;           //gets the direction in which the objects should be pushed

        if (other.attachedRigidbody)
            other.attachedRigidbody.AddForce(direction * 100);                                          //pushes the objects
    }


}
