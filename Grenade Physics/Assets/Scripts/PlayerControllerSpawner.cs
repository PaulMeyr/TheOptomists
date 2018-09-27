using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerControllerSpawner : NetworkBehaviour
{

    public Transform munitionSpawnLocation = null;
    public GameObject baseGrenadePrefab = null;
    public GameObject holygranagPrefab = null;
   private GameObject grenade = null;
    public GameObject player;
   public int myWeaponis;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }

        myWeaponis = player.GetComponent<PlayerController>().selectedWeapon;
       
    }
  


[Command]
    public void Cmd_throwGrenade(float cook)
    {

        if (myWeaponis == 0)
        {
        
           grenade = (GameObject)Instantiate(baseGrenadePrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);
        }
        if (myWeaponis == 1)
        {
            grenade = (GameObject)Instantiate(holygranagPrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);
        }
        grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * (1.0f + cook) * 10.0f;
        grenade.GetComponent<Grenade>().lifeTime += cook;//we cooked the grenade a bit.
        NetworkServer.Spawn(grenade);
    }
}
