using UnityEngine;
using System.Collections;


/// <summary>
/// Player unit : controls the whole instance of player variables
/// </summary>
[System.Serializable]
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

	public int baseDamage;
	public int weaponDamage;

	// Use this for initialization
	void Awake () 
	{
		playerHealth = this.GetOrAddComponent<PlayerHealth> ();
		playerAttack = this.GetOrAddComponent<PlayerAttack> ();
		playerCharacter = gameObject.AddComponent<PlayerMovement> ();
		freeze = false;
		PlayerStats playerStats = Game.current.playerStats;
		playerAttack.attackDamage = playerStats.attackDamage;

		playerHealth.currentHealth = playerStats.currentHealth;
		playerHealth.maxHealth = playerStats.maxHealth;
		playerExperience = playerStats.experience;

		isPlayerCharacter = true;
	}
		




}
