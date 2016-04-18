using UnityEngine;
using System.Collections;


[System.Serializable]
public class PlayerStats 
{
	// health
	public int currentHealth;
	public int maxHealth;

	// strength
	public int strength;

	// attack damage
	public int attackDamage;

	// stats
	public int intelligence;
	public int lovedScore;
	public int dexterity;


	// experience
	public int experience;

	// transform
	public float playerLocationX;
	public float playerLocationY;

	// what have we leveled up?
	// public list of skills learned at different levels based on class

	// class for character
	public string characterClass;

	public PlayerStats()
	{
		// set up a basic character
		intelligence = 10;
		dexterity = 10;
		experience = 0;
		lovedScore = 10;
		attackDamage = 10;
		strength = 10;
		currentHealth = 100;
		maxHealth = 100;
	}

	// public list of game objects this player has in inventory
}
