using UnityEngine;
using System.Collections;

public class PlayerUnit : MonoBehaviour 
{

	PlayerHealth playerHealth;
	PlayerMovement playerCharacter;
	PlayerAttack playerAttack;


	// PUBLIC VARIABLES FOR SETTING INFORMATION ABOUT AN ENEMY
	public int playerCurrentHealth;
	public int playerMaxHealth;
	public int playerSpeed;
	public string playerName;
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
		playerCharacter.freeze = false;


		playerHealth.currentHealth = playerCurrentHealth;
		playerHealth.maxHealth = playerMaxHealth;
		playerCharacter.speed = playerSpeed;
		playerCharacter.gameObjectPlayerName = "Player";
		playerCharacter.isPlayerCharacter = true;
	}
	

}
