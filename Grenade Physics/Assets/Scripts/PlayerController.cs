using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
[AddComponentMenu("Player Controller")]
public class PlayerController : NetworkBehaviour
{
    private NetworkStartPosition[] spawnPoints;
    public bool active = false;
	public float speed = 2.0f;
	public float rotateSpeed = 3.0f;
    public float knockBackFalloffRate = 1.00f;
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;
    public bool lockCursor;
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;
    public Vector3 velocity = Vector3.zero;
    public Vector3 knockBackVel = Vector3.zero;

    [SyncVar(hook = "OnChangeHealth")]
    public float health = 100;
    public const int maxHealth = 100;
    public bool destroyOnDeath;

    
    public RectTransform healthBar;
    
<<<<<<< HEAD
=======
  

>>>>>>> e0960c38485cebfc22f0bcf72ba28dea97b10f08
    private float grenadeWindUp = 0;

    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.
    public GameObject characterBody = null;
    public GameObject characterEyes = null;
    public NetworkIdentity characterBodyIdentity = null;
    public PlayerControllerSpawner objectSpawner = null;
    public Camera playerCamera = null;


    // weaponSwitching
    public  int selectedWeapon = 0;
    public int toldWeapons;

    void Start()
    {
        if (isLocalPlayer)
        
            {
<<<<<<< HEAD
           
=======

>>>>>>> e0960c38485cebfc22f0bcf72ba28dea97b10f08
            if(GameObject.Find("Ui_ingame_Canvas") == null){
                SceneManager.LoadScene("UI", LoadSceneMode.Additive);
            }
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            }
            targetDirection = transform.localRotation.eulerAngles;
        if (characterBody)
        {
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
            characterBodyIdentity = characterBody.GetComponent<NetworkIdentity>();
            if (characterBodyIdentity == null)
                Debug.LogError("Character Body did not have a Network Identity.");
            objectSpawner = characterBody.GetComponent<PlayerControllerSpawner>();
            if (objectSpawner == null)
                Debug.LogError("Character Body did not have a PlayerControllerSpawner.");
        }
       
    }

  

    //currently unused
    /*
	void FixedUpdate()
	{
		
	}
    */

    void Update()
    {
      
        if (!characterBodyIdentity.isLocalPlayer)
        {
            return;
        }
   
        if (!playerCamera.enabled)
        {
            if (Camera.main != null)
            {
                Camera.main.enabled = false;
            }
            
            playerCamera.enabled = true;
            playerCamera.tag = "MainCamera";
        }
        playerCamera.enabled = true;
        playerCamera.tag = "MainCamera";
        // Ensure the cursor is always locked when set
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
        _mouseAbsolute += _smoothMouse;
 
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        characterEyes.transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
 
        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
        if (this.transform.position.y <= -3.6f)
        {
            RpcRespawn();
        }
      

		CharacterController controller = characterBody.GetComponent<CharacterController>();
		Vector3 forward = characterBody.transform.TransformDirection(Vector3.forward);
		Vector3 right = characterBody.transform.TransformDirection(Vector3.right);
		Vector3 jump = Vector3.zero;
		float gravity = 9.8f;
        velocity -= velocity * Time.deltaTime;
        velocity.y -= gravity * Time.deltaTime;

        if (controller.isGrounded)
        {
            velocity.y = 0;
            //extra knockback falloff when grounded.
            knockBackVel *= 1.0f - (knockBackFalloffRate * Time.deltaTime);
        }
        //constant knockback falloff (air resistance)
        knockBackVel *= 1.0f - (knockBackFalloffRate * Time.deltaTime);

        if (controller.isGrounded&&Input.GetButton("Jump"))
		{
            //give it a little leap in the direction we are moving.
            velocity.x *= 1.5f;
            velocity.z *= 1.5f;

            velocity.y=6.0f;
		}



		float curSpeedV = speed * Input.GetAxis("Vertical");
		float curSpeedH = speed * Input.GetAxis("Horizontal");
        float crouchMult = 1.0f;
        if(Input.GetButton("Crouch"))
        {
            crouchMult = 0.25f;
            controller.height -= 10.0f*Time.deltaTime;
            if (controller.height < 0.99f)
                controller.height = 0.99f;
        }
        else
        {
            controller.height += 10.0f * Time.deltaTime;
            if(controller.height>1.99f)
                controller.height = 1.99f;
        }
       
        if(Input.GetAxis("Mouse ScrollWheel")>0f)
        {

            if (selectedWeapon >= toldWeapons - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {

            if (selectedWeapon <= 0 )
                selectedWeapon = toldWeapons - 1;
            else
                selectedWeapon--;
        }
       

        float sprintMult = 1.0f;
        if (Input.GetButton("Sprint"))
        {
            sprintMult = 1.5f;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            grenadeWindUp += Time.deltaTime;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            objectSpawner.Cmd_throwGrenade(grenadeWindUp);
            grenadeWindUp = 0;
        }

        if (active) {
			controller.Move (((forward * curSpeedV * crouchMult * sprintMult) + (right * curSpeedH * crouchMult) + velocity + knockBackVel) * Time.deltaTime);
		}
       
    
       
        if (health <= 0)
        {
            Debug.Log("Health dropped, respawn rcp?");
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                health = maxHealth;
                knockBackVel = Vector3.zero;
                // called on the Server, invoked on the Clients
                RpcRespawn();
            }
        }

    }
    //heathbar
    void OnChangeHealth(float health)
    {
        this.health = health;
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    //respawning
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoint;
            knockBackVel = Vector3.zero;
        }
    }


  
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
        //make sure we are using the local camera if we are spawned.
        if (Camera.main != playerCamera)
        {
            if (Camera.main != null)
            {
                Camera.main.enabled = false;
            }
            playerCamera.enabled = true;
            playerCamera.tag = "MainCamera";
        }
    }
   

}
