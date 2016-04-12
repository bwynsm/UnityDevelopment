using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class EnemyHealth : MonoBehaviour
{
	public int maxHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
	public AudioClip deathClip;                                 // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

	// let's also update the text field just for fun
	public Text healthField;

	Animator anim;                                              // Reference to the Animator component.
	AudioSource playerAudio;                                    // Reference to the AudioSource component.
	//EnemyCharacter playerMovement;                              // Reference to the player's movement.
	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.


	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		anim = GetComponent <Animator> ();
	}



	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		// If the player has just been damaged...
		if(damaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			//damageImage.color = flashColour;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			// damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		damaged = false;
	}




	/// <summary>
	/// Takes the damage given it and updates slider and text
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void TakeDamage (int amount)
	{

		gameObject.AddComponent<DamageNumbers>();
		DamageNumbers damageNumbers = gameObject.GetComponent<DamageNumbers> ();
		damageNumbers.prefabDamage = (GameObject)Resources.Load("Damage", typeof(GameObject));
		damageNumbers.CreateDamagePopup (amount, transform.position);
		 
		// Set the damaged flag so the screen will flash.
		damaged = true;

		// Reduce the current health by the damage amount.
		currentHealth -= amount;

		if (currentHealth <= 0)
		{
			currentHealth = 0;
		}

		// Set the health bar's value to the current health.
		healthSlider.value = currentHealth;
		healthField.text = "<color='yellow'>" + currentHealth + "</color><color='white'> / " + maxHealth + "</color>";

		// Play the hurt sound effect.
		//playerAudio.Play ();

		// If the player has lost all it's health and the death flag hasn't been set yet...
		if(currentHealth <= 0 && !isDead)
		{
			// ... it should die.
			Death ();
		}
	}




	/// <summary>
	/// If this character dies, we want to destroy our panels and change scene
	/// and also prepare to have this unit deleted
	/// </summary>
	void Death ()
	{

		// Set the death flag so this function won't be called again.
		isDead = true;

		// for now, win condition
		//Destroy(GameObject.Find("BattlePanel").GetComponent<BattleMenu>());
		//GameObject.Find ("BattlePanel").SetActive (false);


		// Tell the animator that the player is dead.
		anim.SetTrigger ("Die");

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		//playerAudio.clip = deathClip;
		//playerAudio.Play ();

		// Turn off the movement and shooting scripts.
		//playerMovement.enabled = false;




	}       
}
