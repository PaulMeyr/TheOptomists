using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class bloodfadeing : NetworkBehaviour
{

    public Image blood;
    public float fadeTime = 3.0f;
    public float bloodAmunt, fadeinblood = 0, Amunt;
    public float heath;
    private GameObject player;
    void Update()
    {
       
        player = GameObject.Find("TestPlayer(Clone)");

        bloodAmunt = ((100 - heath) * 0.007f);
        if (fadeinblood < bloodAmunt && fadeinblood <= 0.7f)
        {
            fadeinblood += Amunt;
        }
        if (fadeinblood > bloodAmunt) {
            fadeinblood -= Amunt;
        }

        heath = player.GetComponentInChildren<PlayerController>().health;

        if (fadeinblood< 0){
         blood.enabled = false;
        }
        if (bloodAmunt >= 0.7)
        {
            fadeinblood = 0;
            blood.enabled = false;
        }
        StartCoroutine(startfading());

    }
        IEnumerator startfading()
    {
     

        yield return new WaitForSeconds( 4.0f);

        //Removes some of the trans that is used to lerp the alpha of the material.
        float invFadeTime = 1.0f / fadeTime;
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            fadin();
        }

        //Destroys the object when the material is completly transparent.
     
    }void fadin()
    {
        Color bloodColor = blood.color;
        bloodColor.a = fadeinblood;
        blood.color = bloodColor;
    }

  
}
