using UnityEngine;
using System.Collections;

public class PlayerUnit : CharacterConversable 
{

	public PlayerHealth playerHealth;
	public PlayerMovement playerCharacter;
	public PlayerAttack playerAttack;


	// PUBLIC VARIABLES FOR SETTING INFORMATION ABOUT AN ENEMY
	public int playerCurrentHealth;
	public int playerMaxHealth;
	public int playerArmor;
	public int playerSpellResistance;
	public int playerExperience;
	public int playerGold;

	// Use this for initialization
	void Awake () 
	{
		playerHealth = this.GetOrAddComponent<PlayerHealth> ();
		playerAttack = this.GetOrAddComponent<PlayerAttack> ();
		playerCharacter = this.GetOrAddComponent<PlayerMovement> ();
		freeze = false;


		playerHealth.currentHealth = playerCurrentHealth;
		playerHealth.maxHealth = playerMaxHealth;

		isPlayerCharacter = true;
	}
	

}
