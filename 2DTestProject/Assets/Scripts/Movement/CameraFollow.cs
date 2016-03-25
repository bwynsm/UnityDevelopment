using UnityEngine;
using System.Collections;


/**
 * CameraFollow
 * This is the camera that we have for our game
 * This camera follows the player ost of the time.
 */
public class CameraFollow : MonoBehaviour 
{

	// stores the position data, scale data, rotation data of an object
	public Transform target;

	// we'll set this at 1 to start to instantly move towards the character
	public float camSpeed = 0.1f; // control the speed as a percentage of the camera follow

	// reference to the camera object - not public
	Camera mycam;



	// Use this for initialization
	void Start () 
	{
		//Cursor.lockState =  CursorLockMode.Locked;
		//Cursor.visible = false;

		mycam = GetComponent<Camera> ();

		// if the camera has a target, update to move towards it
		// we'll temporarily set the speed at 100% so we are immediately there
		float tempCamSpeed = camSpeed;
		camSpeed = 1.0f;

		// once we move to our target...
		followTarget();

		// set the camera back to its original speed
		camSpeed = tempCamSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{


		// some simple math
		// set the sze of the camera
		// 32 pixels on camera is 32 pixels on scene

		// screen height divided by 100 float and divided by arbitrary number
		mycam.orthographicSize = (Screen.height / 100f) / 3f;

		followTarget ();
	}


	// function that follows the target of the camera at the camera speed
	void followTarget()
	{
		if (target)
		{
			transform.position = Vector3.Lerp (transform.position, target.position, camSpeed) + new Vector3 (0, 0, -10);
		}
	}
}
