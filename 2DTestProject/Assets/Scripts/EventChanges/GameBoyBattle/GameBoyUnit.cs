using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameBoyUnit : MonoBehaviour 
{





	public int currentHealth;
	public int maxHealth;
	public int attackDamageBase;
	public bool isPlayerCharacter;
	public string attackDone;
	public string playerName;
	public GameObject prefabDamage;
	public Text healthLeft;
	public Sprite healthBarLostTick;
	public Sprite healthBarTick;
	public bool underAttack;
	public int experience;


	//bool damaged;
	bool isDead;
	Animator anim;
	bool attackFinished = false;
	bool hasRetreated = false;
	List<GameObject> healthBar; 
	bool attackDodged;
	bool attackCritical;

	public ARMOR_TYPE armorType;
	public DAMAGE_TYPE damageType;

	public GameBoyUnit targetUnit;                  	// Reference to the player's health.
	//bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
	Vector2 basePosition;

	//bool attacking = false;
	//bool retreating = false;
	bool flashedScreen;
	bool damaged = false;
	int damageDealt;
	int losingHealthCurrent;
	DamageData damageCalculations;



	// Use this for initialization
	void Start () 
	{
		//damaged = false;
		isDead = false;
		//playerInRange = false;
		anim = GetComponent <Animator> ();
		PrepareForBattle ();
		damaged = false;
		damageDealt = 0;
		losingHealthCurrent = currentHealth;
		healthBar = new List<GameObject> ();
		underAttack = false;
		damageCalculations = Toolbox.Instance.damageCalculations;
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if (!damaged || damageDealt == 0)
		{
			return;
		}



		// at losing health current, swap the image.
		// then, once we swap the image, decrement damageDealt
		// if damageDealt is 0, damage is turned off
		Image currentImage = healthBar[losingHealthCurrent - 1].GetComponent<Image>();
		currentImage.sprite = healthBarLostTick;


		// otherwise, let's lower the health until we have the proper health
		damageDealt--;
		losingHealthCurrent--;

		if (losingHealthCurrent <= 0)
		{
			damageDealt = 0;
			damaged = false;
		}


	}




	/// <summary>
	/// Attack the player
	/// We won't do this like this eventually, but for now, this can be how it's done
	/// This needs to be fixed
	/// </summary>
	public IEnumerator Attack ()
	{
		Debug.Log ("we are calculating hit chance : " + targetUnit.armorType.ToString() + " " + damageType.ToString());
		float hitChance = damageCalculations.calculateHitChance (targetUnit.armorType, damageType);
		bool criticalStrike = false;
		float criticalDamage = 1.0f;

		// tell our battle manager that we are done
		GameBoyBattleManager batMan = Camera.main.GetComponent<GameBoyBattleManager> ();



		double randomNum = Random.Range (0, 101) / 100.00;

		Debug.Log ("MISS CHANCE : " + hitChance + " AND RANDOM ROLL :  " + randomNum);

		if (randomNum > hitChance)
		{
			attackDodged = true;


			// start dodging
			attackDone = "'s attack is dodged by " + targetUnit.playerName + "!";
		} 

		float critChance = damageCalculations.calculateCritChance (targetUnit.armorType, damageType);

		// if we hit, then calculate to see if we got our crit
		if (!attackDodged && (randomNum <= hitChance * critChance))
		{
			criticalStrike = true;
			// otherwise, calculate crit chance

			anim.SetTrigger ("IsAttacking");
			attackFinished = false;
			hasRetreated = false;
			criticalDamage = 1.5f;

		} 
		else
		{

			// otherwise, calculate crit chance

			anim.SetTrigger ("IsAttacking");
			attackFinished = false;
			hasRetreated = false;
		}


		// until the animation is done.... keep moving
		while (!attackFinished)
			yield return null;


		// if critical strike, increase the damage dealt


		damageDealt = Mathf.RoundToInt (Random.Range (attackDamageBase * 0.8f, attackDamageBase * 1.2f) * criticalDamage);


			// If the player has health to lose...
		if (targetUnit.currentHealth > 0 && attackDodged == false)
		{
			// ... damage the player.
			targetUnit.underAttack = true;
			StartCoroutine (targetUnit.TakeDamage (damageDealt));
			while (targetUnit.underAttack)
			{
				yield return null;
			}


			attackDone = " attacks " + targetUnit.playerName;

			if (criticalStrike)
			{
				attackDone += ". Critical.";
			}
		}



		// reset our bool
		attackDodged = false;


		anim.SetTrigger ("IsRetreating");


		while (!hasRetreated)
		{
			yield return null;
		}

		anim.SetTrigger ("HasRetreated");

		


		batMan.turnFinished = true;
		batMan.attackDone = attackDone;
		Toolbox.Instance.isLocked = false;

		yield return new WaitForSeconds (0.3f);
	}

	/// <summary>
	/// Updates the health.
	/// </summary>
	public void UpdateHealth()
	{
		healthLeft.text = currentHealth.ToString();
	
	}




	public void CreateHealth()
	{
		healthLeft.text = currentHealth.ToString();
		GameObject healthPanel;

		// if we are a player character, set parent to right side
		// if we are not a player character, set damage to left side
		if (isPlayerCharacter)
		{
			healthPanel = GameObject.Find ("LeftHealthBarPanel");
		} 
		else
		{
			healthPanel = GameObject.Find ("RightHealthBarPanel");
		}

		// panel for health that we'll update with a horizontal panel?
		// max health of 60 or 90
		for (int x = 0; x < maxHealth; x++) 
		{
			GameObject healthItem = new GameObject ();
			healthItem.transform.SetParent (healthPanel.transform);
			healthBar.Add (healthItem);
			Image healthImage = healthItem.AddComponent<Image> ();
			healthImage.sprite = healthBarTick;
			healthImage.transform.localScale = new Vector3 (3, 3, 1);

		}
	}

	/// <summary>
	/// Takes the damage given it and updates slider and text
	/// </summary>
	/// <param name="amount">Amount.</param>
	public IEnumerator TakeDamage (int amount)
	{

		DamageNumbers damageNumbers = gameObject.AddComponent<DamageNumbers>();
		damageNumbers.battleCanvas = GameObject.Find ("HUD").GetComponent<RectTransform>();
		damageNumbers.prefabDamage = prefabDamage;
		damageNumbers.CreateDamagePopup (amount, transform.position);
		ShakeCamera ();

		// Set the damaged flag so the screen will flash.
		//damaged = true;

		// Reduce the current health by the damage amount.
		losingHealthCurrent = currentHealth;
		currentHealth -= amount;
		damaged = true;
		damageDealt = amount;

		while (damageDealt > 0)
		{
			yield return null;
		}
		damaged = false;


		if (currentHealth <= 0)
		{
			currentHealth = 0;
		}

		UpdateHealth ();

		// Set the health bar's value to the current health.
		//healthSlider.value = currentHealth;
		//healthField.text = "<color='yellow'>" + currentHealth + "</color><color='white'> / " + maxHealth + "</color>";

		// Play the hurt sound effect.
		//playerAudio.Play ();

		// If the player has lost all it's health and the death flag hasn't been set yet...
		if(currentHealth <= 0 && !isDead)
		{
			// ... it should die.
			Death ();
		}

		underAttack = false;
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



	/// <summary>
	/// Shakes the camera on attack or animation or just for fancyness
	/// </summary>
	public void ShakeCamera()
	{

		if (!attackDodged)
		{
			// set a screen shake
			Camera.main.GetComponent<CameraShake> ().Shake ();
		} 



	}


	/// <summary>
	/// Dodges the attack.
	/// </summary>
	public void DodgeAttack()
	{
		Debug.Log ("we are dodging with " + playerName);
		// do our dodge animation
		anim.SetTrigger("Dodge");
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
	/// The attack is complete
	/// </summary>
	public void attackComplete()
	{
		if (attackDodged)
		{
			targetUnit.DodgeAttack ();
		}

		attackFinished = true;
	}


	/// <summary>
	/// Finished with something when the animation is complete
	/// </summary>
	public void AnimationComplete()
	{
		hasRetreated = true;
	}



	/// <summary>
	/// Prepares for battle ( gets into battle animations )
	/// </summary>
	public void PrepareForBattle()
	{

		anim = GetComponent<Animator> ();
		anim.SetBool ("IsFighting", true);
	}











	/// <summary>
	/// Calculates the hit damage.
	/// </summary>
	/// <returns>The hit damage.</returns>
	public int calculateHitDamage()
	{

		return 0;
	}


	/// <summary>
	/// Has dodged attack trigger animation.
	/// </summary>
	public void hasDodgedAttack()
	{
		anim.SetTrigger ("HasDodged");
	}
}
