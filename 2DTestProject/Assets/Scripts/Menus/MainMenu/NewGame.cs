using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


/// <summary>
/// New game - creates a new game in the panel and saves it
/// </summary>
public class NewGame
{
	public int memoryGameSlot = 0;

	/// <summary>
	/// Initializes a new instance of the <see cref="NewGame"/> class.
	/// </summary>
	public NewGame()
	{
	}


	/// <summary>
	/// Initializes a new instance of the <see cref="NewGame"/> class.
	/// </summary>
	/// <param name="memoryGameSlotNumber">Memory game slot number.</param>
	public NewGame(int memoryGameSlotNumber)
	{
		memoryGameSlot = memoryGameSlotNumber;
	}


	/// <summary>
	/// Creates the new game and swaps our scene
	/// </summary>
	// create a new game instance
	public void CreateNewGame()
	{
		Game.current = new Game ();
		SaveLoad.Save(memoryGameSlot);

		Toolbox.Instance.currentSaveSlot = memoryGameSlot;



		// LOAD THE GAME
		SceneManager.LoadScene("LoadingScene");


	}

}
