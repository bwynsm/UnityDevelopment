using UnityEngine;
using System.Collections;


public class PlayerAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 5.0f;     // The time in seconds between each attack.
	public int attackDamage = 44;               // The amount of health taken away per attack.


	Animator anim;                              // Reference to the animator component.
	public GameObject target;                          // Reference to the player GameObject.
	public EnemyUnit enemyUnit;                  // Reference to the player's health.
	bool targetInRange;                         // Whether player is within the trigger collider and can be attacked.
	//float timer;                                // Timer for counting up to the next attack.
	Vector2 basePosition;
	//Rigidbody2D rbody;


	public bool startAttacking = false; // this could be some sort of command..
	bool attacking = false;
	//bool retreating = false;
	bool flashedScreen;

	public bool isActive = false; // set this in the instance creation. That way we can move functions into here


	/// <summary>
	/// Awake this instance. When we awaken, notice that the player is not in range, get access to that player
	/// for now. And get our animator
	/// </summary>
	void Awake ()
	{
		// Setting up the references.
		targetInRange = false;
		anim = this.GetComponent<Animator> ();

		//rbody = GetComponent<Rigidbody2D> ();
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		if (!attacking && startAttacking)
		{
			startAttacking = false;
			StartCoroutine( Attack ());
		}
	}


	/// <summary>
	/// Raises the trigger enter2 d event. If our other object is player and we are attacking,
	/// attack the player and change the animation to our next phase "Retreat"
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other)
	{

		// If the entering collider is the player...
		if (other.gameObject == target && attacking == true)
		{
			Debug.Log ("attacking" + attacking);

			attacking = false;
			Debug.Log ("we are in the trigger phase and are attacking");
			// ... the player is in range.
			targetInRange = true;



			anim.SetTrigger ("IsAttacking");



			// If the player has health to lose...
			if (enemyUnit.enemyHealth.currentHealth > 0)
			{
				// ... damage the player.
				enemyUnit.enemyHealth.TakeDamage (attackDamage);

			}



		} 

	}



	/// <summary>
	/// Raises the trigger exit2 d event.
	/// If we are leaving, player is no longer in range for attack
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D (Collider2D other)
	{
		// If the exiting collider is the player...
		if(other.gameObject == target)
		{
			// ... the player is no longer in range.
			targetInRange = false;
		}
	}



	/// <summary>
	/// Update this instance. On update, if the player is not in range
	/// and we are attacking and active, move towards player
	/// If we are in range and attacking, hit player
	/// Else if we are retreating, move back to start until we get there
	/// </summary>
	/*void Update ()
	{




		// if we are attacking and the player is not yet in range
		// then we are charging
		if (attacking && !targetInRange && isActive)
		{
			float xPos;
			float yPos;


			// get our movement vector towards the player
			if ((target.transform.position.x - rbody.position.x) > 0)
				xPos = 1;
			else if ((target.transform.position.x - rbody.position.x) < 0)
				xPos = -1;
			else
				xPos = 0;

			if ((target.transform.position.y - rbody.position.y) > 0)
				yPos = 1;
			else if ((target.transform.position.y - rbody.position.y) < 0)
				yPos = -1;
			else
				yPos = 0;


			// move towards player
			Vector2 angle = new Vector2 (xPos, yPos);
			rbody.MovePosition (rbody.position + (angle * 2.0f * Time.deltaTime));
		}

		// if we are retreating and not home yet, move towards home
		else if (retreating && !(basePosition.Equals (rbody.position)) && isActive)
		{
			float xPos;
			float yPos;


			// get our vector towards home
			if ((basePosition.x - rbody.position.x) > 0)
				xPos = 1;
			else if ((basePosition.x - rbody.position.x) < 0)
				xPos = -1;
			else
				xPos = 0;

			if ((basePosition.y - rbody.position.y) > 0)
				yPos = 1;
			else if ((basePosition.y - rbody.position.y) < 0)
				yPos = -1;
			else
				yPos = 0;


			// move towards home
			Vector2 angle = new Vector2 (xPos, yPos);
			rbody.MovePosition (rbody.position + (angle * 2.0f * Time.deltaTime));


			// latch onto home if we are close
			if (rbody.position.x <= basePosition.x + 0.2 && rbody.position.x >= basePosition.x - 0.2 && rbody.position.y <= basePosition.y + 0.2 && rbody.position.y >= basePosition.y - 0.2)
			{
				rbody.MovePosition (basePosition);
			}
		}


		// if we are retreating, and we are in our home spot, flip our character to face player
		// and set us at "has retreated"
		// say what attack we have done
		else if (retreating && isActive)
		{
			retreating = false;
			flashedScreen = false;
			anim.SetTrigger ("HasRetreated");
			Flip ();

			// tell our battle manager that we are done
			BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
			batMan.turnFinished = true;
			batMan.attackDone = "Charged at the Princess and ATTACKED WITH AN AXE";
			Toolbox.Instance.isLocked = false;

		}

	}*/







	/// <summary>
	/// Retreat. If we are starting to retreat, flip our sprite around and run away
	/// </summary>
	void retreat()
	{
		Debug.Log ("retreating..... from animation..");
		anim.SetTrigger ("IsRetreating");

		// while that's not done.

		//retreating = true;
		Flip();
	}


	/// <summary>
	/// Flip this sprite in the x axis
	/// </summary>
	public void Flip()
	{
		Vector3 theScale = gameObject.transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}





	/// <summary>
	/// Shakes the camera.
	/// </summary>
	public void ShakeCamera()
	{
		Debug.Log ("we are here in shaking camera");
		// set a screen shake
		Camera.main.GetComponent<CameraShake>().Shake();

		// let's also make it white!

	}


	/// <summary>
	/// Return to battle idle position
	/// </summary>
	public void returnToIdle()
	{

		anim.SetTrigger ("HasRetreated");


	}


	/// <summary>
	/// Flashes the screen to make attacks more dramatic
	/// </summary>
	public void screenFlash()
	{
		if (!flashedScreen)
		{
			flashedScreen = true;
			Debug.Log ("we are here in flashing white");
			// if we are in here, an object collided
			ScreenFader sf = GameObject.FindGameObjectWithTag ("Fader").GetComponent<ScreenFader> ();
			sf.FlashWhite ();
		}
	}









	/// <summary>
	/// Teleports the player either to attack or back to base position
	/// </summary>
	public void teleportPlayer()
	{

		// if not at base position, bring home. If at base position - go to enemy.
		if (new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y)  != basePosition)
		{
			attacking = false;

			// teleport home
			Debug.Log("teleporting home...");
			this.gameObject.transform.position = basePosition;

			anim.SetTrigger ("HasRetreated");
		}

		// teleporting to attack
		else
		{
			
			Debug.Log ("Teleporting to attack");
			// player transform
			this.gameObject.transform.position = enemyUnit.transform.position + new Vector3(-0.20f,0f,0f);

			// now that we have teleported the player, we can get ready to attack.
		}


	}



	/// <summary>
	/// Attack the player
	/// We won't do this like this eventually, but for now, this can be how it's done
	/// This needs to be fixed
	/// </summary>
	public IEnumerator Attack ()
	{

		basePosition = new Vector2 (this.transform.position.x, this.transform.position.y);

		/*// Reset the timer.
		//timer = 0f;
		Debug.Log("we are attacking in here : " + basePosition + " " + targetInRange + " " + attacking);


		anim.SetTrigger ("IsCharging");

		// we set animation for approaching
		while (!targetInRange)
		{
			yield return null;
		}

		anim.SetTrigger ("IsAttacking");
		attacking = true;

		// once that finishes...
		while (attacking)
		{
			yield return null;
		}

		// that should be set when animation is complete. Teleport should have been called already
		// though, so we should be in range.
		*/

		// not even going to go into an animation state for attacking just yet, but we are going to 
		// try and do a camera shake and a light flash
		ShakeCamera();
		screenFlash ();


		// range of 20% more and 20% less
		float damageGiven = Random.Range(attackDamage * 0.8f, attackDamage * 1.2f);

		enemyUnit.enemyHealth.TakeDamage (Mathf.RoundToInt(damageGiven));


		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.turnFinished = true;
		batMan.attackDone = " Attacked " + enemyUnit.playerName;
		Toolbox.Instance.isLocked = false;

		yield return new WaitForSeconds (0.01f);
	}

	/// <summary>
	/// Shoots the projectile like an arrow or something.
	/// Shoots the projectile and then starts the coroutine until it gets there
	/// Once our projectile has hit, we can have the hero take damage and
	/// then we can tell the game our turn has finished
	/// </summary>
	/// <returns>The projectile.</returns>
	public IEnumerator shootProjectile()
	{
		Debug.Log ("we have shot our projectile. go into animation cooldown round");
		anim.SetTrigger ("ShotProjectile");

		// then we have to start a new coroutine for shooting the arrow
		ProjectileAnimator sa = GameObject.Find("Arrow").GetComponent<ProjectileAnimator>();
		yield return StartCoroutine(sa.FireProjectile ());

		// okay let's kill it
		ShakeCamera();
		screenFlash ();


		enemyUnit.enemyHealth.TakeDamage (15);



		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.attackDone = " Fired an arrow at "  + enemyUnit.playerName;

		Toolbox.Instance.isLocked = false;
	}


	/// <summary>
	/// Casts the offensive spell.
	/// </summary>
	/// <param name="commandItem">Command item.</param>
	public void castOffensiveSpell(string commandItem)
	{
		// we'll just get the first part

		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.attackDone = " cast " + commandItem + " on " + enemyUnit.playerName;

		Toolbox.Instance.isLocked = false;
	}



	/// <summary>
	/// Deals the damage.
	/// </summary>
	/// <param name="commandItem">Command item.</param>
	public void dealDamage(string commandItem)
	{
		// we have some options with this command item. But we'll probably have to 
		// update some of these as "ITEM DESCRIPTIONS" later on.


		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.attackDone = " used FIREPOTION on " + enemyUnit.playerName;

		Toolbox.Instance.isLocked = false;
	}


	/// <summary>
	/// Buffs the player.
	/// </summary>
	/// <param name="commandItem">Command item.</param>
	/// <param name="playerBeingBuffed">Player being buffed.</param>
	public void buffPlayer(string commandItem, PlayerUnit playerBeingBuffed)
	{
		string command = (commandItem.Split ('#')) [0];

		if (command == "health")
		{
			playerBeingBuffed.playerHealth.HealCharacter (40);
		}

		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.attackDone = " cast heal on " + playerBeingBuffed.playerName;

		Toolbox.Instance.isLocked = false;
	}



	/// <summary>
	/// Run when the animation is complete to throw a boolean to indicate our finishing
	/// </summary>
	public void AnimationComplete()
	{
		targetInRange = true;

		// start attacking
		anim.SetTrigger("IsAttacking");
	}


}