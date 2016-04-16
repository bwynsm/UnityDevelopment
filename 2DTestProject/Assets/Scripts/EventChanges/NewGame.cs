using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NewGame
{
	public int memoryGameSlot = 0;

	public NewGame()
	{
	}

	public NewGame(int memoryGameSlotNumber)
	{
		memoryGameSlot = memoryGameSlotNumber;
	}

	// create a new game instance
	public void CreateNewGame()
	{
		Game.current = new Game ();
		SaveLoad.Save(memoryGameSlot);

		Toolbox.Instance.currentSaveSlot = memoryGameSlot;

		// LOAD THE GAME
		SceneManager.LoadScene("LoadingScene");

		Debug.Log ("NUMBER OF GAMES : " + SaveLoad.savedGames.Count);

	}

}
