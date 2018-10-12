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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        toldWeapon = player.GetComponent<PlayerController>().toldWeapons;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {

            if (myWeaponis >= toldWeapon - 1)
            {
                myWeaponis = 0;
                Debug.Log("GANDE 1");
            }

            else
            {
                myWeaponis++;
                Debug.Log("GANDE 2");
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {

            if (myWeaponis <= 0)
            {
                myWeaponis = toldWeapon - 1;
                Debug.Log("GANDE 2");
            }

            else
            {
                myWeaponis--;
                Debug.Log("GANDE 1");
            }

        }






    }
  


		
    [ClientRpc]
    public void Rpc_throwGrenade(float cook)
    {

            //Switch/case?
            if (myWeaponis == 0)
        {
            grenade = (GameObject)Instantiate(baseGrenadePrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);

        }
        else if (myWeaponis == 1)
        {
            grenade = (GameObject)Instantiate(holygranagPrefab, munitionSpawnLocation.position, munitionSpawnLocation.rotation);

        }

        grenade.GetComponent<Rigidbody>().velocity = grenade.transform.forward * (1.0f + cook) * 10.0f;
        grenade.GetComponent<Grenade>().lifeTime += cook;//we cooked the grenade a bit.
        if (NetworkServer.active)
            NetworkServer.Spawn(grenade);

    
}

    [Command]public void Cmd_throwGrenade(float cook){

            Rpc_throwGrenade(cook);
        }


}
