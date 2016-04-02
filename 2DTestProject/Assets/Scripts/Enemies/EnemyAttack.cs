using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 5.0f;     // The time in seconds between each attack.
    public int attackDamage = 15;               // The amount of health taken away per attack.


    Animator anim;                              // Reference to the animator component.
    GameObject target;                          // Reference to the player GameObject.
    PlayerUnit targetUnit;                  	// Reference to the player's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    //float timer;                                // Timer for counting up to the next attack.
	Vector2 basePosition;
	Rigidbody2D rbody;

	bool attacking = false;
	bool retreating = false;
	bool flashedScreen;

	public bool isActive = false; // set this in the instance creation. That way we can move functions into here


	/// <summary>
	/// Awake this instance. When we awaken, notice that the player is not in range, get access to that player
	/// for now. And get our animator
	/// </summary>
    void Awake ()
    {
        // Setting up the references.
        target = GameObject.FindGameObjectWithTag ("PlayerCharacter");
		targetUnit = target.GetComponent <PlayerUnit> ();
		playerInRange = false;
		anim = this.GetComponent<Animator> ();

		rbody = GetComponent<Rigidbody2D> ();
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
			
			attacking = false;
			Debug.Log ("we are in the trigger phase and are attacking");
			// ... the player is in range.
			playerInRange = true;



			anim.SetTrigger ("IsAttacking");



			// If the player has health to lose...
			if (targetUnit.playerHealth.currentHealth > 0)
			{
				// ... damage the player.
				targetUnit.playerHealth.TakeDamage (attackDamage);
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
            playerInRange = false;
        }
    }



	/// <summary>
	/// Update this instance. On update, if the player is not in range
	/// and we are attacking and active, move towards player
	/// If we are in range and attacking, hit player
	/// Else if we are retreating, move back to start until we get there
	/// </summary>
    void Update ()
    {




		// if we are attacking and the player is not yet in range
		// then we are charging
		if (attacking && !playerInRange && isActive)
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

    }



	/// <summary>
	/// Attack the player
	/// We won't do this like this eventually, but for now, this can be how it's done
	/// This needs to be fixed
	/// </summary>
	public IEnumerator Attack ()
    {
        // Reset the timer.
        //timer = 0f;
		Debug.Log("we are attacking");

		string attackDone = "";


		// if player name is grue, we'll do this
		// otherwise, let's do this in a moment
		if (gameObject.name == "Grue")
		{
			

			GameObject spellObject = GameObject.FindGameObjectWithTag ("Spell");
			spellObject.GetComponent<SpriteRenderer> ().enabled = false;
			ProjectileAnimator sf = spellObject.GetComponent<ProjectileAnimator> ();
			yield return StartCoroutine (sf.CastSpell ());

			// If the player has health to lose...
			if (targetUnit.playerHealth.currentHealth > 0)
			{
				// ... damage the player.
				targetUnit.playerHealth.TakeDamage (attackDamage);
			}

			sf.StopSpell ();



			attackDone = "casts Lightning on the Princess";

			// tell our battle manager that we are done
			BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
			batMan.turnFinished = true;
			batMan.attackDone = attackDone;
			Toolbox.Instance.isLocked = false;

		} 

		// otherwise, we are in here
		else if (gameObject.name == "Archer")
		{
			// there needs to be a way to launch projectile in this game instead of following the traditional set
			// of animations.
			// So there should be a way to "skip" attack animation.
			// because during that, the arrow should just be flying.
			// in here, we'll charge to attack
			// we'll transition into projectile status
			// and then we'll be done.
			// set our anim to...!
			anim.SetTrigger("IsCharging");

			// once we are done charging, we fire our array
			// animation is charging fires off an arrow in the enemy character item for now
			// then once that is done, we can do the rest of our animations and go back into idle state.
			// we do not relinquish our turn until the arrow hits
		}

		else
		{

			// first we want to gallop in that direction.
			// then we want to attack
			// then we want to retreat
			basePosition = new Vector2(transform.position.x, transform.position.y);


			// set our anim to...!
			anim.SetTrigger("IsCharging");
			attacking = true;


			// once we are done setting our trigger, let's keep moving forward until we hit our
			// collision zone


			attackDone = "NO REAL ATTACKS DONE! MIND GAMES!";


		}


    }



	/// <summary>
	/// Retreat. If we are starting to retreat, flip our sprite around and run away
	/// </summary>
	void retreat()
	{
		anim.SetTrigger ("IsRetreating");

		Flip ();
		retreating = true;


	}


	/// <summary>
	/// Flip this sprite in the x axis
	/// </summary>
	void Flip()
	{
		Vector3 theScale = gameObject.transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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


		targetUnit.playerHealth.TakeDamage (15);



		// for now we'll relinquish our turn here
		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		batMan.turnFinished = true;
		batMan.attackDone = " Fired an arrow at " + targetUnit.playerName;
		Toolbox.Instance.isLocked = false;
	}


	public void ShakeCamera()
	{
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


	public void screenFlash()
	{
		if (!flashedScreen)
		{
			flashedScreen = true;
			// if we are in here, an object collided
			ScreenFader sf = GameObject.FindGameObjectWithTag ("Fader").GetComponent<ScreenFader> ();
			sf.FlashWhite ();
		}
	}

}