using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
	// create a new game instance
	public void CreateNewGame()
	{
		Game.current = new Game ();
		SaveLoad.Save();


		// LOAD THE GAME
		SceneManager.LoadScene("LoadingScene");

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);

	}

}
