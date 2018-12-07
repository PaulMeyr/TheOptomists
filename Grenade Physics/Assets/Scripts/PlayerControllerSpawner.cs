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
    private int toldWeapon;
 
  

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
     


       



    }

    


    [ClientRpc] public void Rpc_throwGrenade(float cook)
    {
     
           
           
                grenade = (GameObject)Instantiate(baseGrenadePrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);
        
          
            grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * (1.0f + cook) * 15.0f;
            grenade.GetComponent<Grenade>().lifeTime += cook;//we cooked the grenade a bit.
            if (NetworkServer.active)
            {
             NetworkServer.Spawn(grenade);
            }
      
    
   }

    [Command]public void Cmd_throwGrenade(float cook){

            Rpc_throwGrenade(cook);
       
       
    }



}
