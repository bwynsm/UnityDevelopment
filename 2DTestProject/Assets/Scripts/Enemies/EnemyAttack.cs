using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 5.0f;     // The time in seconds between each attack.
    public int attackDamage = 15;               // The amount of health taken away per attack.


    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
	Vector2 basePosition;
	Rigidbody2D rbody;

	bool attacking = false;
	bool retreating = false;
	// charging

    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag ("PlayerCharacter");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
		playerInRange = false;
		anim = this.GetComponent<Animator> ();

		timer = 5.0f;
		rbody = GetComponent<Rigidbody2D> ();
    }
		


	void OnTriggerEnter2D (Collider2D other)
    {
        // If the entering collider is the player...
		if (other.gameObject == player && attacking == true)
		{
			Debug.Log ("we are in the trigger phase");
			// ... the player is in range.
			playerInRange = true;
			attacking = false;

			anim.SetTrigger ("IsAttacking");



			// If the player has health to lose...
			if (playerHealth.currentHealth > 0)
			{
				// ... damage the player.
				playerHealth.TakeDamage (attackDamage);
			}



		} else
		{
			// what are we colliding with
			Debug.Log(other.gameObject.name);
		}
    }


	void OnTriggerExit2D (Collider2D other)
    {
        // If the exiting collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;




		if (attacking && !playerInRange)
		{
			float xPos;
			float yPos;

			if ((player.transform.position.x - rbody.position.x) > 0)
				xPos = 1;
			else if ((player.transform.position.x - rbody.position.x) < 0)
				xPos = -1;
			else
				xPos = 0;

			if ((player.transform.position.y - rbody.position.y) > 0)
				yPos = 1;
			else if ((player.transform.position.y - rbody.position.y) < 0)
				yPos = -1;
			else
				yPos = 0;


			Vector2 angle = new Vector2 (xPos, yPos);
			rbody.MovePosition (rbody.position + (angle * 2.0f * Time.deltaTime));
		}
		else if (retreating && !(basePosition.Equals (rbody.position)))
		{
			float xPos;
			float yPos;

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


			Vector2 angle = new Vector2 (xPos, yPos);
			rbody.MovePosition (rbody.position + (angle * 2.0f * Time.deltaTime));

			if (rbody.position.x <= basePosition.x + 0.2 && rbody.position.x >= basePosition.x - 0.2 && rbody.position.y <= basePosition.y + 0.2 && rbody.position.y >= basePosition.y - 0.2)
			{
				rbody.MovePosition (basePosition);
			}
		}
		else if (retreating)
		{
			retreating = false;
			anim.SetTrigger ("HasRetreated");
			Flip ();

			// tell our battle manager that we are done
			BattleManager batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
			batMan.turnFinished = true;
			batMan.attackDone = "Charged at the Princess and ATTACKED WITH AN AXE";
			Toolbox.Instance.isLocked = false;

		}

    }


	public IEnumerator Attack ()
    {
        // Reset the timer.
        //timer = 0f;

		string attackDone = "";


		// if player name is grue, we'll do this
		// otherwise, let's do this in a moment
		if (gameObject.name == "Grue")
		{
			

			GameObject spellObject = GameObject.FindGameObjectWithTag ("Spell");
			spellObject.GetComponent<SpriteRenderer> ().enabled = false;
			SpellAnimator sf = spellObject.GetComponent<SpellAnimator> ();
			yield return StartCoroutine (sf.CastSpell ());

			// If the player has health to lose...
			if (playerHealth.currentHealth > 0)
			{
				// ... damage the player.
				playerHealth.TakeDamage (attackDamage);
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


	void retreat()
	{
		anim.SetTrigger ("IsRetreating");

		Flip ();
		retreating = true;


	}

	void Flip()
	{
		Vector3 theScale = gameObject.transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}



}