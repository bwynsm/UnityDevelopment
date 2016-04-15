using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game 
{ 

	public static Game current;
	public PlayerStats playerStats;

	public Game () {
		//player = new CharacterConversable ();
		playerStats = new PlayerStats();
	}

}