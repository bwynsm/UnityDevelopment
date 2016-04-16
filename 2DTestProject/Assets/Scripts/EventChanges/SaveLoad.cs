using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class SaveLoad 
{
	public static List<Game> savedGames = new List<Game>(3);


	/// <summary>
	/// Save the game
	/// </summary>
	public static void Save() {
		savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);
	}	


	/// <summary>
	/// Save the game
	/// </summary>
	public static void Save(int overwriteGame) 
	{

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);

		savedGames[overwriteGame] = Game.current;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);
	}	


	/// <summary>
	/// Loads an instance of a game. 
	/// </summary>
	public static void Load()
	{

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);
		if (File.Exists (Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveLoad.savedGames = (List<Game>)bf.Deserialize (file);
			file.Close ();
		}
	}


	/// <summary>
	/// Loads an instance of a game. 
	/// </summary>
	public static void Load(int loadGame)
	{

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);
		if (File.Exists (Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveLoad.savedGames = (List<Game>)bf.Deserialize (file);
			file.Close ();
		}

		Game.current = SaveLoad.savedGames [loadGame];
		Toolbox.Instance.currentSaveSlot = loadGame;
		Toolbox.Instance.positionInLastScene = new Vector2(Game.current.playerStats.playerLocationX, Game.current.playerStats.playerLocationY);
	}

	/// <summary>
	/// Createds the saved games array.
	/// </summary>
	public static void CreatedSavedGamesArray()
	{
		// checks if our save load has any capacity
		if (SaveLoad.savedGames.Count < 3)
		{
			// start at the end and make new games
			for (var i = SaveLoad.savedGames.Count; i < 3; i++)
			{
				SaveLoad.savedGames.Add (null);
			}
		}
	}


}
