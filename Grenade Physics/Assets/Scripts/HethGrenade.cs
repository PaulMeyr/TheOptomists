using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class HethGrenade : Grenade
{
    private float dicstans;
    public GameObject heththing;
    // Use this for initialization
    void Start()
    {
        lifeTime = 6;
        fuseTime = 0;
        knockBackForce = 0;
        damageRadius = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (fuseTime <= lifeTime)
        {
            hasTriggered = true;
           
        }
        else
        {
            hasTriggered = false;
        }
        if (hasTriggered == true)
        {
            Cmd_helth();
        }
        else
        {
            Cmd_loadHeth();
        }
        if(fuseTime <= 0)
        {
            Cmd_reNew();
        }
    }

    [ClientRpc] public void Rpc_reNew()
    {
        lifeTime = 6;
        damageRadius = 0.5f;
        heththing.SetActive(true);
    }
    [Command]
    public void Cmd_reNew()
    {
        Rpc_reNew();
    }
    [Command]
    public void Cmd_loadHeth()
    {
        Rpc_loadHeth();
    }
    [Command]
    public void Cmd_helth()
    {
        Rpc_helth();
    }

    [ClientRpc]
    public void Rpc_loadHeth()
    {
        heththing.SetActive(false);
        damageRadius = 10;
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, damageRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject taker = hitColliders[i].gameObject;
            PlayerController takerPC = taker.GetComponentInChildren<PlayerController>();
            Rigidbody takerRB = taker.GetComponent<Rigidbody>();
            if (takerRB == null && takerPC == null)
            {
                i++;
                continue;//short circuit to the next object since we aren't going to do anything.
            }
            dicstans= (damageRadius - (taker.transform.position - gameObject.transform.position).magnitude) / damageRadius;
            float distRatio = (damageRadius - (taker.transform.position - gameObject.transform.position).magnitude) / damageRadius;
            if (takerPC != null)
            {
                //hurt them and throw them.               
                takerPC.knockBackVel += (taker.transform.position - gameObject.transform.position) * knockBackForce * distRatio;
                if (isServer&& distRatio<=0)
                {
                    fuseTime -=1 ;
                }
            }
            if (takerRB)
            {
                //throw rigidbodies.
                takerRB.AddExplosionForce(knockBackForce * 10.0f, gameObject.transform.position, damageRadius);
            }

            i++;
        }
    }

    [ClientRpc]
    public void Rpc_helth()
        {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, damageRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject taker = hitColliders[i].gameObject;
            PlayerController takerPC = taker.GetComponentInChildren<PlayerController>();
            Rigidbody takerRB = taker.GetComponent<Rigidbody>();
            if (takerRB == null && takerPC == null)
            {
                i++;
                continue;//short circuit to the next object since we aren't going to do anything.
            }

            float distRatio = (damageRadius - (taker.transform.position - gameObject.transform.position).magnitude) / damageRadius;
            if (takerPC != null)
            {
                //hurt them and throw them.               
                takerPC.knockBackVel += (taker.transform.position - gameObject.transform.position) * knockBackForce * distRatio;
                if (isServer)
                {
                    if (takerPC.health <= 150)
                    {
                        takerPC.health += damage;
                    }
                    fuseTime = 6;
                    lifeTime = 0;
                }
            }
            if (takerRB)
            {
                //throw rigidbodies.
                takerRB.AddExplosionForce(knockBackForce * 10.0f, gameObject.transform.position, damageRadius);
            }

            i++;
        }
    }
}
