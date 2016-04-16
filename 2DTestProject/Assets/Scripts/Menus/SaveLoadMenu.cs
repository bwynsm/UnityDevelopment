using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SaveLoadMenu : MonoBehaviour 
{

	public bool isSaving;
	public GameObject loadGamePanels;
	public GameObject mainMenuPanels;

	private Button[] saveFilesItems;
	private Button[] mainMenuItems;

	Toolbox instanceItem;

	public enum MENU_STATES
	{
		START = 0,
		NEW_GAME,
		LOAD,
		SETTINGS,
		EXIT
	};

	public MENU_STATES currentState;

	public int selectedItem;


	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		instanceItem = Toolbox.Instance;
		currentState = MENU_STATES.START;
		selectedItem = 0;
		mainMenuItems = mainMenuPanels.GetComponentsInChildren<Button> ();

		mainMenuItems [0].Select();

		saveFilesItems = loadGamePanels.GetComponentsInChildren<Button> (true);




		// load saved games
		SaveLoad.CreatedSavedGamesArray();

		// now, if we already have games, we also want to display them differently. 
		// so let's do that
		updateGamePanel();
	}



	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{


		// if we don't have any input... do nothing
		if (!Input.anyKeyDown)
			return;

		string action = "";

		// if down, go down
		// if up, go up
		// if x go forward
		// if z go back

		// go up in the menu
		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			action = "up";
		} 

		// go down
		else if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			action = "down";
		}

		// go forward
		else if (Input.GetKeyDown (KeyCode.X))
		{
			action = "forward";
		}

		// go back
		else if (Input.GetKeyDown (KeyCode.Z))
		{
			action = "back";
		} 

		// otherwise, a key that doesn't matter
		else
		{
			return;
		}

		// depending on what state we are in determines what we do.
		// if we are locked, can't go forward
		//if (instanceItem.isLocked)
		//	return;

		// lock our toolbox
		//instanceItem.isLocked = true;

		switch (currentState)
		{


		// START STATE : MAIN MENU
		case MENU_STATES.START:
			// wait for keypress to see what our next state is
			// see what we get for start menu button press.
			// for now, we'll accept a click
			Debug.Log ("we are in start");
			MainMenuKeypress (action);

			break;

		// LOAD GAME STATE : LOAD MENU
		case MENU_STATES.LOAD:
			Debug.Log ("we are in load");

			GamePanelKeypress (action);

			break;

		// SAVE GAME STATE
		case MENU_STATES.NEW_GAME:
			Debug.Log("we are in new game");
			GamePanelKeypress (action);
			break;

		// SETTINGS FOR THE GAME STATE
		case MENU_STATES.SETTINGS:
			Debug.Log("we are in settings");
			break;


		default:
			break;
		}


	}




	/// <summary>
	/// Hides the panels.
	/// </summary>
	public void ShowGamePanel()
	{
		// choose which panels are visible
		loadGamePanels.SetActive (true);
		mainMenuPanels.SetActive (false);

		// change the state
		currentState = MENU_STATES.NEW_GAME;
		selectedItem = 0;
		instanceItem.isLocked = false;

		saveFilesItems [0].Select ();


		// we also want to update the display in here so that our buttons and stuff is showing some data
		// about our game
		// for each of the games, get some information about the player that is being stored in the game
		foreach (Game gameObject in SaveLoad.savedGames)
		{
			
		}



	}

	// display panel and allow selection of options for saving a game or 
	// loading a game
	// we should have up to three games saved.
	// and they should have different attributes. Let's see if we can 
	// save transform as well.
	public void ShowMainMenu()
	{
		// choose which panels show
		loadGamePanels.SetActive (false);
		mainMenuPanels.SetActive (true);


		// change the state
		selectedItem = 0;
		currentState = MENU_STATES.START;
		instanceItem.isLocked = false;

		mainMenuItems [0].Select ();

	}






	/// <summary>
	/// Key was pressed in new game menu
	/// </summary>
	/// <param name="keyPressed">Key pressed.</param>
	public void GamePanelKeypress(string keyPressed)
	{
		if (keyPressed.Equals ("up"))
		{
			selectedItem--;

			// for now, the item is just 0
			if (selectedItem < 0)
			{
				// set the selected item at the end
				// right now, we have 2 save files
				selectedItem = saveFilesItems.Length - 1;
			}
			saveFilesItems [selectedItem].Select ();
		} 
		else if (keyPressed.Equals ("down"))
		{
			selectedItem++;

			if (selectedItem >= saveFilesItems.Length)
			{
				selectedItem = 0;
			}

			saveFilesItems [selectedItem].Select ();
		} 
		else if (keyPressed.Equals ("forward"))
		{

			if (currentState == MENU_STATES.NEW_GAME)
			{
				// just overwrite the game here - we'll later on want to do a confirm if we have
				// a legitimate game at this location
				// for now, just overwrite.
				NewGame newGame = new NewGame(selectedItem);
				newGame.CreateNewGame ();
			}
			else if (currentState == MENU_STATES.LOAD)
			{
				// load game here
				SaveLoad.Load(selectedItem);

				// toolbox scene
				SceneManager.LoadScene ("LoadingScene");
			}




		} 
		else if (keyPressed.Equals ("back"))
		{
			ShowMainMenu ();
		}
	}


	/// <summary>
	/// Main Menu key was pressed
	/// </summary>
	/// <param name="keyPressed">Key pressed.</param>
	public void MainMenuKeypress(string keyPressed)
	{
		if (keyPressed.Equals ("up"))
		{
			selectedItem--;

			// for now, the item is just 0
			if (selectedItem < 0)
			{
				// set the selected item at the end
				// right now, we have 2 save files
				selectedItem = mainMenuItems.Length - 1;
			}


			Debug.Log ("selected item up is..." + selectedItem);
			mainMenuItems [selectedItem].Select ();
		} 

		// if key pressed is down, increment our index to travel down
		// the menu
		else if (keyPressed.Equals ("down"))
		{
			selectedItem++;

			if (selectedItem >= mainMenuItems.Length)
			{
				selectedItem = 0;
			}

			Debug.Log ("selected item down is..." + selectedItem);
			mainMenuItems [selectedItem].Select ();
		} 

		// if moving forward: check our index and determine which item to load
		else if (keyPressed.Equals ("forward"))
		{
			// check which number we are on
			if (selectedItem == 0)
			{
				ShowGamePanel ();
				currentState = MENU_STATES.NEW_GAME;
			} 

			else if (selectedItem == 1)
			{
				ShowGamePanel ();
				currentState = MENU_STATES.LOAD;

			}
		}
	}




	private void updateGamePanel()
	{

		// get our text
		Text[] textDisplayArray = loadGamePanels.GetComponentsInChildren<Text>();

		// for each of the save files, display a little something in our game panel
		for (var i = 0; i < 3; i++ )
		{
			Text textDisplay = textDisplayArray [i];
			Game gameItem = SaveLoad.savedGames [i];

			// if our game item is not null, then we want to display something about the player

			if (gameItem != null)
			{
				textDisplay.text = "Player Health : " + gameItem.playerStats.currentHealth + "\nPlayer Experience : " + gameItem.playerStats.experience;
			}

			// if it is null, display that the game is empty
			else 
			{
				textDisplay.text = "Empty Game Slot";
			}
		}
	}


		

}
