using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/// <summary>
/// Enemy character information: analogue of PlayerMovement
/// </summary>
public class EnemyCharacter : MonoBehaviour 
{

	Animator anim;
	public float movementSpeed = 0.6f; // movement speed of character
	public bool isMoving = false;

	Vector2 direction;
	Rigidbody2D rbody;

	float timer = 2.0f;

	// RANGING DISTANCE FOR NPC TO GO
	// one box is the square root of 2 - a box and a bit
	public float distanceX = 0.5f;
	public float distanceY = 0.5f;
	private Vector2 startLocation;

	public string gameObjectPlayerName;


	public PolygonCollider2D interactionTriggerCollider;
	public Vector2[] polygon;


	// our player unit
	EnemyUnit thisEnemyUnit;


	// Use this for initialization
	void Start () 
	{


		anim = GetComponent<Animator> ();
		rbody = GetComponent<Rigidbody2D> ();
		Transform temp = GetComponent<Transform> ();
		startLocation.x = temp.position.x;
		startLocation.y = temp.position.y;
		gameObject.name = gameObjectPlayerName;

		interactionTriggerCollider = gameObject.AddComponent<PolygonCollider2D> ();
		interactionTriggerCollider.pathCount = 1;
		thisEnemyUnit = gameObject.GetComponent<EnemyUnit> ();


		// create a polygon if we don't have one for collision triggering
		/*if (polygon == null)
		{
			polygon = new Vector2[] {
				new Vector2 (0.00f, 1.5f),
				new Vector2 (-1.5f, -0.1f),
				new Vector2 (0.00f, -1.5f),
				new Vector2 (1.5f, -0.1f)
			};
		}

		interactionTriggerCollider.points = polygon;
		interactionTriggerCollider.isTrigger = true;*/
	}



	// Update is called once per frame
	// here we update the character if not frozen with a little movement
	// we do some random calculation to determine movement information
	void Update () 
	{
		if (GetComponent<Renderer> ().isVisible) {
			// if we are frozen, we do not want to be walking.

			timer -= Time.deltaTime;
			float xRange;
			float yRange;

			int canGoWest = -1;
			int canGoEast = 2;
			int canGoSouth = -1;
			int canGoNorth = 2;

			// if our character is not frozen
			if (!thisEnemyUnit.freeze) 
			{



				// every time the timer hits, we do a movement
				// we do some more random numbers just to make movement
				// less boring
				if (timer <= 0) 
				{


					// if the character is moving, we stop them on the timer
					// running out. They need a rest.
					if (isMoving) 
					{
						isMoving = false;
						anim.SetBool ("isWalking", false);
						timer = Random.Range(2.0f, 4.0f);
					}

					// if the character is not moving, suddenly they feel like moving a bit!
					// pick a direction randomly, multiply by our movement speed
					// and go!
					else 
					{

						// if we are reaching the end of our tether, don't go a particular direction. Curl back around.
						// we don't want to get stuck.. but we could with this sort of function. That's okay.
						Transform currentLocation = GetComponent<Transform> ();

						//Vector3 goScreenPos = Camera.main.WorldToScreenPoint (currentLocation.position);
						//Debug.Log ("Location : " + currentLocation.position.x + " " + currentLocation.position.y + " " + startLocation.x + " " + startLocation.y + " " + distanceX + " " + distanceY);
						// just debug it

						// 100 units = screen width / float number of camera size 


						// can't keep going that way
						if (currentLocation.position.x >= startLocation.x + distanceX) 
						{
							canGoEast = 0;
							//Debug.Log("CAN'T GO EAST : " + currentLocation.position.x + ", " + currentLocation.localPosition.x);
						} 
						else if (currentLocation.position.x <= startLocation.x - distanceX) 
						{
							canGoWest = 0;
							//Debug.Log("CAN'T GO WEST : " + currentLocation.position.x + ", " + currentLocation.localPosition.x);
						}

						// can we travel north or south?
						if (currentLocation.position.y >= startLocation.y + distanceY) 
						{
							canGoNorth = 0;
							//Debug.Log("CAN'T GO NORTH : " + currentLocation.position.x + " " + currentLocation.localPosition.x);
						} 
						else if (currentLocation.position.y<= startLocation.y - distanceY) 
						{
							canGoSouth = 0;
							//Debug.Log("CAN'T GO SOUTH : " + currentLocation.position.x + " " + currentLocation.localPosition.x);
						}



						// take steps every couple seconds
						xRange = Random.Range (canGoWest, canGoEast);
						yRange = Random.Range (canGoSouth, canGoNorth);

						isMoving = true;
						direction = new Vector2 (xRange, yRange);
						direction = direction * movementSpeed;
						anim.SetBool ("isFrozen", false);

						// random timer between 1-3


						timer = Random.Range(0.5f, 3.0f);
					}
				}


				// if we are moving... then we want to move our rigid body
				// for now, we won't move if the player is in our space.
				// we'll kinda just stand there and let them chat with us
				if (isMoving && !rbody.isKinematic) 
				{

					// take steps every couple seconds
					xRange = Random.Range (canGoWest, canGoEast);
					yRange = Random.Range (canGoSouth, canGoNorth);


					// if we have not chosen a moveless-move..
					// movement in the x direction or y direction
					if (direction != Vector2.zero) 
					{
						anim.SetBool ("isWalking", true);
						anim.SetFloat ("input_x", direction.x);
						anim.SetFloat ("input_y", direction.y);

						// move in that direction with animation
						rbody.MovePosition (rbody.position + direction * Time.deltaTime);



					} 

					// not moving anymore.
					else 
					{
						anim.SetBool ("isWalking", false);

					}



				} 


			}

			// if we are frozen, we do not want to be walking
			else 
			{
				anim.SetBool ("isWalking", false);
			}
		}


	}



	/// <summary>
	/// Raises the collision enter2d event - are we walking into something? Err... stop
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionEnter2D(Collision2D col)
	{
		if (anim == null)
			return;
		
		// we collided with something. set walking to false
		anim.SetBool("isWalking", false);
		isMoving = false;
		//rbody.isKinematic = false;

		if (col.gameObject.tag == "PlayerCharacter")
		{
			rbody.isKinematic = true;	

			// start battle sequence
			//Debug.Log (gameObject.transform.position);
			Toolbox toolboxInstance = Toolbox.Instance;

			if (!toolboxInstance.sceneAlreadyLoaded)
			{
				toolboxInstance.sceneAlreadyLoaded = true;
				toolboxInstance.battlePosition = gameObject.transform.position;

				DontDestroyOnLoad (this);
				DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("PlayerCharacter"));

				SceneManager.LoadScene ("BattleScene");
			}
		} 


	}
		



	/// <summary>
	/// Raises the collision exit2 d event. If we turned the enemy into a wall,
	/// return character into dynamic type collider object
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionExit2D(Collision2D col)
	{
		rbody.isKinematic = false;
	}


	/// <summary>
	/// Late update that does a little more rendering in case something is in front of us or behind us
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
	/// Sets the character to frozen
	/// </summary>
	public void setFrozen()
	{
		anim.SetBool ("isFrozen", true);
	}





	/////////////////////////////////////////////////////////////////////////////////////////////////////
	/// EVERYTHING BELOW HERE GETS MOVED
	/// - screenflash to screenflash object? this isn't a character thing
	/// - shootProjectile - let's move that to something enemyattack works with
	/// - I think we'll have enemy attack always attached to a unit that can attack, but
	/// it will be turned off until battle engages
	/////////////////////////////////////////////////////////////////////////////////////////////////////





	// flipping a battle animation is more of a character thing.
	void Flip()
	{
		Vector3 theScale = gameObject.transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
