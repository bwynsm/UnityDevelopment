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
	Transform basePosition;
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
			bool retreating = true;

			anim.SetTrigger ("IsAttacking");





		} else
		{
			// what are we colliding with
			Debug.Log(other.gameObject.name);
		}
    }


	void OnTriggerExit (Collider other)
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

		// if our animation trigger is charging.

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
       /* if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
			Debug.Log ("we are here attacking");
            // ... attack.
			StartCoroutine( Attack ());
        }

        // If the player has zero or less health...
        if(playerHealth.currentHealth <= 0)
        {
            // ... tell the animator the player is dead.
            //anim.SetTrigger ("PlayerDead");
        }*/

		if (attacking && !playerInRange)
		{
			Vector2 angle = new Vector2 (player.transform.position.x - rbody.position.x, rbody.position.y - player.transform.position.y);
			rbody.MovePosition (rbody.position + angle * Time.deltaTime);
		} 
		else if (retreating)
		{
			Debug.Log (basePosition.position.x + ", " + basePosition.position.y);
			Vector2 angle = new Vector2 (basePosition.position.x - rbody.position.x, rbody.position.y - basePosition.position.y);
			rbody.MovePosition (rbody.position + angle * Time.deltaTime);
		}

    }


	public IEnumerator Attack ()
    {
        // Reset the timer.
        //timer = 0f;

		string attackDone = "";
		basePosition = this.transform;


		// if player name is grue, we'll do this
		// otherwise, let's do this in a moment
		if (gameObject.name == "Grue")
		{


			Debug.Log ("enemy is attacking!");

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
		else
		{
			Debug.Log ("PLAYER OBJECT NAME : " + gameObject.name);
			Debug.Log ("WE ATTACKED!");

			// first we want to gallop in that direction.
			// then we want to attack
			// then we want to retreat
			basePosition = transform;

			// set our anim to...!
			anim.SetTrigger("IsCharging");
			attacking = true;


			// once we are done setting our trigger, let's keep moving forward until we hit our
			// collision zone



			timer = 10.0f;





			// If the player has health to lose...
			if (playerHealth.currentHealth > 0)
			{
				// ... damage the player.
				playerHealth.TakeDamage (attackDamage);
			}

			attackDone = "NO REAL ATTACKS DONE! MIND GAMES!";


		}


    }


	void retreat()
	{
		Debug.Log ("we are retreating");
		// return to base position
		float step = 1.0f * Time.deltaTime;
		anim.SetTrigger ("Retreat");

		retreating = true;


	}
}