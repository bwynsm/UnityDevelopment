using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class SaveLoad 
{
	public static List<Game> savedGames = new List<Game>();


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


}
