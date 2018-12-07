using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Grenade : NetworkBehaviour {

    public bool hasTriggered = false;
    

    public float fuseTime = 5.0f; //the amount of time we can live before going boom.
    public float lifeTime = 0; // the amount of time we have lived.
    public float damage = 10.0f;
    public float knockBackForce = 10.0f;
    public float damageRadius = 10.0f;
    public bool isContactTriggered = false;

	void Start () {
		
	}
	
	void Update () {
        lifeTime += Time.deltaTime;
        doMovement();
        doTriggerCheck();
    }

    // Thunk.
    void OnCollisionEnter(Collision col)
    {
        onContact();
    }

    // Our check to see if we should detonate
    public void doTriggerCheck()
    {
        if (fuseTime < lifeTime)
            hasTriggered = true;

        if(hasTriggered)
        {
            doExplosion();
            doEffect();
            Destroy(gameObject);
        }
    }

    // Any special movement information that needs a per-frame update.
    public void doMovement()
    {

    }

    // The effect we want to do when we bounce.
    public void onContact()
    {
        if (isContactTriggered && !hasTriggered)
        {
            hasTriggered = true;
        }
    }

    // The damage sphere for the grenade.
    void doExplosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, damageRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GameObject taker = hitColliders[i].gameObject;
            PlayerController takerPC = taker.GetComponentInChildren<PlayerController>();

            Rigidbody takerRB = taker.GetComponent<Rigidbody>();
            if(takerRB==null&&takerPC==null)
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
                    takerPC.health -= damage * distRatio;
                 
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

    // The special effect we want do display on detonation.
    void doEffect()
    {

    }

}
