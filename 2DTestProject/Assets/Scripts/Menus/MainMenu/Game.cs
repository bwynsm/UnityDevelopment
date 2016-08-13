using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game 
{ 

	public static Game current;
	public PlayerStats playerStats;


	/// <summary>
	/// Initializes a new instance of the <see cref="Game"/> class.
	/// </summary>
	public Game () {
		//player = new CharacterConversable ();
		playerStats = new PlayerStats();
	}


	/// <summary>
	/// Updates the game object
	/// </summary>
	public void UpdateGame()
	{
		PlayerUnit mainCharacter = GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerUnit> ();


		// update the stats for our player.
		playerStats.currentHealth = mainCharacter.playerHealth.currentHealth;
		playerStats.experience = mainCharacter.playerExperience;
		playerStats.maxHealth = mainCharacter.playerHealth.maxHealth;
		playerStats.playerLocationX = mainCharacter.transform.position.x;
		playerStats.playerLocationY = mainCharacter.transform.position.y;
	}

}