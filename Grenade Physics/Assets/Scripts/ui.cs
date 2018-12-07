using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui  : MonoBehaviour
{
    public Image BaseGrenadeJPG, holygranagJPG;
    public Image blood;
    public float fadeTime = 3.0f;
    public float bloodAmunt, fadeinblood = 0, Amunt;
    public float heath;
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
       //TODO We should have an array or list of grenade objects, this isn't easily expandable and is bad practice. - Dan
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
        heath = player.GetComponentInChildren<PlayerController>().health;
        bloodAmunt = ((100 - heath) * 0.007f);
        if (fadeinblood < bloodAmunt && fadeinblood <= 0.7f)
        {
            fadeinblood += Amunt;
        }
        if (fadeinblood > bloodAmunt)
        {
            fadeinblood -= Amunt;
        }

        

        if (fadeinblood < 0)
        {
            blood.enabled = false;
        }else{
            blood.enabled = true;
        }
        if (bloodAmunt >= 0.7)
        {
            fadeinblood = 0;
            blood.enabled = false;
        }else{
            blood.enabled = true;
        }
StartCoroutine(startfading());

    }
    IEnumerator startfading()
    {


        yield return new WaitForSeconds(4.0f);

        //Removes some of the trans that is used to lerp the alpha of the material.
        float invFadeTime = 1.0f / fadeTime;
        for (float t = 0.0f; t < fadeTime; t += Time.deltaTime)
        {
            fadin();
        }

        //Destroys the object when the material is completly transparent.

    }
    void fadin()
    {
        Color bloodColor = blood.color;
        bloodColor.a = fadeinblood;
        blood.color = bloodColor;
    }
}
