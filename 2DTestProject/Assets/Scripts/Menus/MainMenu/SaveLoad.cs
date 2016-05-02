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

	}	


	/// <summary>
	/// Save the game
	/// </summary>
	public static void Save(int overwriteGame) 
	{


		savedGames[overwriteGame] = Game.current;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		bf.Serialize(file, SaveLoad.savedGames);
		file.Close();

	}	


	/// <summary>
	/// Loads an instance of a game. 
	/// </summary>
	public static void Load()
	{

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

		if (File.Exists (Application.persistentDataPath + "/savedGames.gd"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			SaveLoad.savedGames = (List<Game>)bf.Deserialize (file);
			file.Close ();
		}

		Game.current = SaveLoad.savedGames [loadGame];
		Toolbox.Instance.currentSaveSlot = loadGame;
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


	/// <summary>
	/// Returns if there is any saved game
	/// </summary>
	/// <returns><c>true</c>, if saved game was ised, <c>false</c> otherwise.</returns>
	public static bool isAnySavedGame()
	{
		int index = 0;
		bool isAnySavedGame = false;

		// check until we find a saved game or until we are done looking at all the slots
		while (index < 3 && !isAnySavedGame)
		{
			if (index >= savedGames.Count)
				break;

			if (savedGames [index] != null)
			{
				isAnySavedGame = true;
			}

			index++;
		}

		return isAnySavedGame;
	}






}
