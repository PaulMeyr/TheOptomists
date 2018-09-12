using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Player Controller")]
public class PlayerController : MonoBehaviour {
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
    
    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.
    public GameObject characterBody;
 
    void Start()
    {
        targetDirection = transform.localRotation.eulerAngles;
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
    }

    //puts us back on the terrain if we fall off.
	void resetPositionOnTerrain()
	{
		characterBody.GetComponent<CharacterController> ().enabled = false;
		active = false;
		Vector3 origin = characterBody.transform.position;
		origin.y = 1000.0f;
		Vector3 direction = new Vector3 (0, -1.0f, 0);
		RaycastHit hit;
		if (Physics.Raycast (origin, direction, out hit)) {
			characterBody.transform.position = hit.point + new Vector3(0, 2, 0);
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
 
        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;
 
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

		if (transform.position.y < -500) {
			resetPositionOnTerrain ();
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
        float sprintMult = 1.0f;
        if (Input.GetButton("Sprint"))
        {
            sprintMult = 1.5f;
        }

		if (active) {
			controller.Move (((forward * curSpeedV * crouchMult * sprintMult) + (right * curSpeedH * crouchMult) + velocity + knockBackVel) * Time.deltaTime);

		}
    }

    
}
