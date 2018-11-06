using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heathroat : MonoBehaviour {
    
    private float rotspeedY;
    private float ypox;
 
    private float speedy = 0;
    private float smooth = 5.0f;
    private float tiltAngle = 60.0f;
    public float speed = 5.0f;
    public  bool shouldContinue=true;
    public float hight,minhight;
    
    // Use this for initialization
    void Start () {
		
	}
    // Update is called once per frame
    void Update()
    {
       

        speedy += 3;
        float rotspeedY = speedy * speed;
        Quaternion target = Quaternion.Euler(0, rotspeedY,0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, tiltAngle * smooth);
        tiltAngle = tiltAngle + Time.deltaTime;
        this.transform.position = new Vector3(this.transform.position.x, ypox, this.transform.position.z);

        if (ypox < hight && shouldContinue == true)
        {
            ypox += 0.0005f;

        }
        if (ypox >= hight || shouldContinue == false)
        {
            shouldContinue = false;
            ypox -= 0.005f;
        }
        if(ypox <= minhight)
        {
            shouldContinue = true;
        }
        
       

    }
   
}
