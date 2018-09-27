using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ui : NetworkBehaviour
{
    public Image BaseGrenadeJPG, holygranagJPG;

    private int myWeaponis;
   private int toldWeapon;
    public GameObject player;
    // Use this for initialization
    void Start()
    {
        BaseGrenadeJPG.enabled = false;
        holygranagJPG.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
       
        toldWeapon = player.GetComponent<PlayerController>().toldWeapons;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {

            if (myWeaponis >= toldWeapon - 1)
                myWeaponis = 0;
            else
                myWeaponis++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {

            if (myWeaponis <= 0)
                myWeaponis = toldWeapon - 1;
            else
                myWeaponis--;
        }

        if (myWeaponis == 0)
        {
            BaseGrenadeJPG.enabled = true;
            holygranagJPG.enabled = false;
        }
        if (myWeaponis == 1)
        {
            BaseGrenadeJPG.enabled = false;
            holygranagJPG.enabled = true;
        }
    }
}
