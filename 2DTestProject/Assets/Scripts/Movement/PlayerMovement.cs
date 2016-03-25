using UnityEngine;
using System.Collections;


/// <summary>
/// Player movement.
/// 
/// Controls the movement of the main character.
/// Also helps control the main camera for the character
/// </summary>
public class PlayerMovement : CharacterConversable
{

	Rigidbody2D rbody;
	Animator anim;
	float movementSpeed = 1.2f;

	float timer = 3.0f;

	private float runMultiplier;
	public string gameObjectPlayerName;

	// Use this for initialization
	void Start () 
	{
		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		gameObject.name = gameObjectPlayerName;
		//renderer = GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () 
	{

		timer -= Time.deltaTime;
		runMultiplier = 1.0f;
		//int pos = Mathf.RoundToInt(this.transform.parent.transform.position.z);
		//pos /= 5; //Remember division of an INT and the modulus operator %? This isn't a float. We WANT to get rid of the remainder.
		//spriteRenderer.sortingOrder = (pos * -1) + OrderOffset;

		// what is my FREEEEEEZE?
		// if we are frozen, we do not want to be walking.
		if (!freeze) 
		{
			

			Vector2 movementVector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			// we're going to assume for the sake of argument that running is twice the speed of walking.
			if (Input.GetKey (KeyCode.LeftShift))
			{
				runMultiplier = 2.0f;
			}
			movementVector = movementVector * movementSpeed * runMultiplier;
			anim.SetBool ("isFrozen", false);

			// movement in the x direction or y direction
			if (movementVector != Vector2.zero) 
			{
				anim.SetBool ("isWalking", true);
				anim.SetFloat ("input_x", movementVector.x);
				anim.SetFloat ("input_y", movementVector.y);

				// move in that direction with animation

			} 

			// not moving anymore.
			else 
			{
				anim.SetBool ("isWalking", false);

			}
			

			// move the character to designated spot
			rbody.MovePosition (rbody.position + movementVector * Time.deltaTime);
		}

		// if we are frozen, we do not want to be walking
		else 
		{
			anim.SetBool ("isWalking", false);
		}





	}



	/// <summary>
	/// Are we walking into something? Make us stop - I don't think this is actually working.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter2D(Collision2D col)
	{
		// we collided with something. set walking to false
		anim.SetBool("isWalking", false);

	}


	/// <summary>
	/// Late Update runs after update and does a little more rendering for us regarding being behind or in front of objects on screen
	/// </summary>
	void LateUpdate()
	{
		
		GetComponent<Renderer> ().sortingOrder = (int)Camera.main.WorldToScreenPoint(GetComponent<SpriteRenderer>().bounds.min).y * -1;
	}


	/// <summary>
	/// Updates the movement speed.
	/// </summary>
	/// <param name="newMovementSpeed">New movement speed.</param>
	void updateMovementSpeed(float newMovementSpeed)
	{
		movementSpeed = newMovementSpeed;
	}

	/// <summary>
	/// Sets the frozen animation to true.
	/// </summary>
	public void setFrozen()
	{
		freeze = true;
	}



}