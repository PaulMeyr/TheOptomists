using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerControllerSpawner : NetworkBehaviour
{

    public Transform munitionSpawnLocation = null;
    public GameObject baseGrenadePrefab = null;

   
    // Use this for initialization
    void Start () {
     
    }
	
	// Update is called once per frame
	void Update () {
		
	}
  


[Command]
    public void Cmd_throwGrenade(float cook)
    {      
        GameObject grenade = (GameObject)Instantiate(baseGrenadePrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);
        grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * (1.0f + cook) * 10.0f;
        grenade.GetComponent<Grenade>().lifeTime += cook;//we cooked the grenade a bit.
        NetworkServer.Spawn(grenade);
    }
}
