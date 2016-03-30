using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 5.0f;     // The time in seconds between each attack.
    public int attackDamage = 15;               // The amount of health taken away per attack.


    //Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    //float timer;                                // Timer for counting up to the next attack.


    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag ("PlayerCharacter");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
		playerInRange = true;
    }


    void OnTriggerEnter (Collider other)
    {
        // If the entering collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
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


    /*void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
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
        }
    }*/


	public IEnumerator Attack ()
    {
        // Reset the timer.
        //timer = 0f;

        // If the player has health to lose...
        if(playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage (attackDamage);
        }

		Debug.Log ("enemy is attacking!");

		GameObject spellObject = GameObject.FindGameObjectWithTag("Spell");
		spellObject.GetComponent<SpriteRenderer> ().enabled = false;
		SpellAnimator sf = spellObject.GetComponent<SpellAnimator> ();
		yield return StartCoroutine (sf.CastSpell());

		sf.StopSpell ();

		// tell our battle manager that we are done
		BattleManager batMan = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BattleManager>();
		batMan.turnFinished = true;
		batMan.attackDone = "casts Lightning on the Princess";
		Toolbox.Instance.isLocked = false;

    }
}