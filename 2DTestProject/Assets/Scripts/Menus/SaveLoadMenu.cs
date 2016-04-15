using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

			LoadKeypress (action);

			break;

		// SAVE GAME STATE
		case MENU_STATES.NEW_GAME:
			Debug.Log("we are in new game");
			NewGameKeypress (action);
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
	/// Loads the panels.
	/// </summary>
	public void ShowLoadGamePanel()
	{
		// choose which panels are visible
		loadGamePanels.SetActive (true);
		mainMenuPanels.SetActive (false);

		// change the state
		currentState = MENU_STATES.LOAD;
		selectedItem = 0;
		instanceItem.isLocked = false;
		saveFilesItems [0].Select ();
	}


	/// <summary>
	/// Hides the panels.
	/// </summary>
	public void ShowNewGamePanel()
	{
		// choose which panels are visible
		loadGamePanels.SetActive (true);
		mainMenuPanels.SetActive (false);

		// change the state
		currentState = MENU_STATES.NEW_GAME;
		selectedItem = 0;
		instanceItem.isLocked = false;

		saveFilesItems [0].Select ();
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
	/// Key was pressed in load menu
	/// </summary>
	/// <param name="keyPressed">Key pressed.</param>
	public void LoadKeypress(string keyPressed)
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

		} 
		else if (keyPressed.Equals ("back"))
		{
			ShowMainMenu ();
		}



	}


	/// <summary>
	/// Key was pressed in new game menu
	/// </summary>
	/// <param name="keyPressed">Key pressed.</param>
	public void NewGameKeypress(string keyPressed)
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
				ShowNewGamePanel ();
			} else if (selectedItem == 1)
			{
				ShowLoadGamePanel ();
			}
		}
	}


		

}
