using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Menu Class that controls all of the interfaces with GUI elements in our project
/// like the options in conversations or the main menu etc
/// </summary>
public class Menu : MonoBehaviour 
{


	// PRIVATE VARIABLES
	private StingPlayer stingPlayer;
	private GUIDisplayItems GUIdisplayItems;
	private bool isActive = true; 		// if our menu is active
	private string commands;			// string of commands made by choosing something
	private int indexSelected = 0;     	// button selected
	private int minWidth;
	private int minHeight;
	private bool hasAccepted;

	// PUBLIC VARIABLES
	public GameObject optionsBox;		// this is the options box panel that we are displaying in
	public List<Options> menuOptions;	// list of all the options in menu
	public string menuType;				// conversation, main menu, etc
	public bool isHorizontal;
	public bool requiresLayout;

	public PlayerUnit attackingPlayer;
	public EnemyUnit targetPlayer;
	public PlayerUnit buffPlayer;

	public bool selectionMade = false;	// if we have made a selection of a button
	public WaitingForTime waitingObject; // object to pause the game for something



	/// <summary>
	/// Start this instance.
	/// 
	/// This just runs a loop that we only need to run once before we destroy the object.
	/// The Menu comes and goes and waits for input and gives options and highlights and
	/// then does commands based on inputit
	/// </summary>
	public IEnumerator Start ()
	{
		// when we start, we want to have game tools initialized and some music sounds
		stingPlayer = GameObject.FindGameObjectWithTag("GameTools").GetComponent<StingPlayer>();
		GUIdisplayItems = GameObject.FindGameObjectWithTag("GameTools").GetComponent<GUIDisplayItems>();


		if (optionsBox != null) 
		{
			yield return StartCoroutine (Initialize ());
		} 

		hasAccepted = false;
	}


	// if selection made and we are in battle, we want to update and wait for input until we receive
	// and new updates from our new class of battle


	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public IEnumerator Initialize()
	{
		selectionMade = false;
		isActive = true;
		hasAccepted = false;

		if (GUIdisplayItems == null)
		{
			GUIdisplayItems = GameObject.FindGameObjectWithTag("GameTools").GetComponent<GUIDisplayItems>();
		}


		// we need to be careful with this - because some menus won't have this few
		// indices. We need to remove the hardcoding for this. What if a menu has more
		// options here?
		if (indexSelected <= 0 || indexSelected >= 4)
			indexSelected = 0;


		if (optionsBox == null)
		{
			yield return null;
		}
			

		optionsBox.AddComponent<WaitingForTime> ();
		waitingObject = optionsBox.GetComponent<WaitingForTime> ();


		// wait for input if we are dealing with a pause menu - might want to replace
		// this with a flag
		if (menuType != "PauseMenu")
		{
			yield return StartCoroutine (waitingObject.PauseBeforeInput ());
		}


		// check and see which item is highlighted here before we enter and make that
		// our indexselected
		for (var i = 0; i < menuOptions.Count; i++) 
		{
			// if the item is highlighted, set our value to that
			// set i to max
			Button currentButton = optionsBox.transform.GetChild(i).gameObject.GetComponent<Button>();
			currentButton.interactable = true;

			if (i == 0 && indexSelected <= 0)
				currentButton.Select ();
			else if (i == indexSelected)
				currentButton.Select ();
		}


		// until we have made a selection and while we still have an options box,
		// wait for input
		while (selectionMade == false && optionsBox.activeInHierarchy) 
		{
			yield return StartCoroutine (waitingObject.WaitForKeyDown ());

			if (EventSystem.current.currentSelectedGameObject == null)
			{
				// if we have no more object
				if (optionsBox.activeInHierarchy == false)
				{
					yield return null;
				}
				else if (indexSelected >= 0 && indexSelected <= menuOptions.Count - 1)
				{
					optionsBox.GetComponentsInChildren<Button> () [indexSelected].Select ();
				}
				else 
				{
					optionsBox.GetComponentsInChildren<Button> () [0].Select ();
				}
			}
				


			// if we haven't made a selection yet - 
			if (!selectionMade && isActive == true && Input.anyKeyDown) 
			{
				if (!isHorizontal)
				{
					
					if (Input.GetKeyDown (KeyCode.DownArrow) == true)
					{
						incrementIndex ();
					}

					/// INPUT KEY DOWN : MOVE DOWN THE LIST
					if (Input.GetKeyDown (KeyCode.UpArrow) == true)
					{
						decrementIndex ();
					}
				}
				else
				{
					// if we are on a horizontal menu, right should go right
					if (Input.GetKeyDown (KeyCode.RightArrow) == true)
					{
						incrementIndex ();
					}

					// if we are on a horizontal menu, left should go left
					else if (Input.GetKeyDown (KeyCode.LeftArrow) == true)
					{
						decrementIndex ();
					}
				}

				if (Input.GetKeyDown (KeyCode.Return)) {
					selectionMade = true;

					// play selection made sting if possible
					if (menuType == "PauseMenu" && stingPlayer != null) 
					{
						stingPlayer.playSelectItemSound ();
					}


					yield return StartCoroutine( ButtonClicked (menuOptions [indexSelected]));
				}

				if (Input.GetKeyDown (KeyCode.X)) 
				{
					
					selectionMade = true;

					// play selection made sting if possible
					if (menuType == "PauseMenu" && stingPlayer != null) 
					{
						stingPlayer.playSelectItemSound ();
					}

					yield return StartCoroutine( ButtonClicked (menuOptions [indexSelected]));
				}

			}


		}



	}
		


	/// <summary>
	/// Returns if the menu is active
	/// </summary>
	/// <returns><c>true</c>, if the menu is active, <c>false</c> otherwise.</returns>
	public bool menuIsActive()
	{
		return isActive;
	}


	/// <summary>
	/// Renames the options without having to delete them all and start over
	/// </summary>
	/// <param name="options">Options.</param>
	public void renameOptions(List<Options> options)
	{
		if (optionsBox != null && menuOptions != null &&  optionsBox.GetComponentsInChildren<Button>().Length > 0)
		{
			
			// get buttons from children
			Button[] panelButtons = optionsBox.GetComponentsInChildren<Button>(true);

			for (int i = 0; i < panelButtons.Length; i++)
			{

				// check if we can update the button
				// get the i number of our option
				panelButtons [i].GetComponentInChildren<Text> ().text = options [i].option;


				// if we have no selected buttons, set our first to selected
				if (i == 0 && (indexSelected <= 0 || indexSelected >= panelButtons.Length))
				{
					panelButtons [i].Select ();
				}
				else if (i == indexSelected)
				{
					panelButtons [i].Select ();
				}

				panelButtons [i].interactable = false; 

				//goButton.AddComponent(
				panelButtons [i].onClick.AddListener(
					() => {  ButtonClicked(options[i]); }
				);

			}

		}
		else 
		{
			loadOptions (options);
		}


	}



	/// <summary>
	/// Toggles the options display so we can hide this when it is the enemy's turn
	/// </summary>
	/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
	public void toggleOptionsDisplay(bool isEnabled)
	{
		if (optionsBox != null && menuOptions != null &&  optionsBox.GetComponentsInChildren<Button>().Length > 0)
		{

			// get buttons from children
			Button[] panelButtons = optionsBox.GetComponentsInChildren<Button>(true);

			// walk through each of the buttons and either hide or display
			// the whole set based parameter input
			for (int i = 0; i < panelButtons.Length; i++)
			{
				panelButtons [i].enabled = isEnabled;
			

				// if we are activating, set colors and alpha
				if (isEnabled)
				{
					panelButtons [i].GetComponentInChildren<CanvasRenderer> ().SetAlpha (255);
					panelButtons [i].GetComponentInChildren<Text> ().color = Color.red;
					panelButtons [i].GetComponentInChildren<Text> ().font = GUIdisplayItems.defaultFont;
				}

				// otherwise, no alpha and clear button text
				else
				{
					panelButtons [i].GetComponentInChildren<CanvasRenderer> ().SetAlpha (0);
					panelButtons [i].GetComponentInChildren<Text> ().color = Color.clear;
				}

			}

		}
	}
		


	/// <summary>
	/// Loads the options for the menu
	/// </summary>
	/// <param name="options">Options.</param>
	public void loadOptions(List<Options> options)
	{
		if (menuOptions == null || menuOptions.Count == 0)
		{
			menuOptions.AddRange (options);
		}
		isActive = true;


		if (GUIdisplayItems == null)
		{
			GUIdisplayItems = GameObject.FindGameObjectWithTag("GameTools").GetComponent<GUIDisplayItems>();
		}

		// for each of our options, create some sort of button
		// in our panel and put it at the right spot
		for (int i = 0; i < options.Count; i++)
		{
			GameObject goButton = (GameObject)Instantiate (GUIdisplayItems.prefabButton);
			goButton.GetComponentInChildren<Text>().text = menuOptions[i].option;

			RectTransform rect = goButton.GetComponentInChildren<Text> ().GetComponent<RectTransform>();
			rect.offsetMax = new Vector2 (-10f, -10f);
			rect.offsetMin = new Vector2 (10f, 10f);


			Options optionItem = new Options ();
			optionItem = menuOptions [i];

			// if we have no selected buttons, set our first to selected
			if (i == 0 && indexSelected <= 0)
			{
				goButton.GetComponent<Button> ().Select ();
			}
			//goButton.AddComponent(
			goButton.GetComponent<Button>().onClick.AddListener(
				() => {  ButtonClicked(optionItem); }
			);
			goButton.transform.SetParent (optionsBox.transform, false);
			goButton.transform.localScale = new Vector3(1, 1, 1);
			goButton.GetComponent<Button> ().interactable = false; 

			// if there is a minimum height and width set
			if (requiresLayout)
			{
				LayoutElement layEle = goButton.AddComponent<LayoutElement> ();
				layEle.minWidth = minWidth;
				layEle.minHeight = minHeight;
			}
		}
			
			
	}



	/// <summary>
	/// Sets the layout options for width and height of the layout element
	/// </summary>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public void setLayoutOptions(int width, int height)
	{
		requiresLayout = true;

		Debug.Log ("WIDTH : " + width + "  HEIGHT : " + height);
		minWidth = width;
		minHeight = height;
		indexSelected = 0;
		selectionMade = false;
	}



	/// <summary>
	/// When one of our menu buttons is clicked, we go here to deal with the command issued
	/// </summary>
	/// <param name="buttonCommand">Button command.</param>
	public IEnumerator ButtonClicked(Options buttonCommand)
	{
		isActive = false;
		Destroy (waitingObject);

		if (selectionMade)
		{

			/// what type of options list do we have? conversation or more main menu?
			if (menuType == "conversation")
			{
				if (buttonCommand.command == "exit")
				{



					yield break;
				}
				else if (buttonCommand.command == "accept")
				{
					hasAccepted = true;
					yield break;
				}

				Commands command = new Commands ();
				command.resolveConversationCommands (buttonCommand);


			}

			// if we have a main menu, we can send those commands over to commands as well
			// and just run our functions for that.
			else if (menuType == "PauseMenu")
			{

				Commands command = new Commands ();
				command.resolvePauseMenuCommands (buttonCommand);
				// what is our command? 
			} 
			else if (menuType == "BattleMenu")
			{
				

				// yield on starting a new 
				// add targetpicker to this player character, and then use that
				// to get the list of players from his battlemenu? who has the list of
				// units to attack?
				// player -> battle menu -> all combatants shoved into player -> add component -> targetpicker 
				TargetPicker playerTargetPicker = attackingPlayer.GetOrAddComponent<TargetPicker>();
				playerTargetPicker.currentPlayer = attackingPlayer;
				playerTargetPicker.battleList = attackingPlayer.GetComponent<BattleMenu> ().allCombatants;
				playerTargetPicker.loadBattle ();


				// disable button presses
				// check and see which item is highlighted here before we enter and make that
				// our indexselected
				for (var i = 0; i < menuOptions.Count; i++) 
				{
					// if the item is highlighted, set our value to that
					// set i to max
					Button currentButton = optionsBox.transform.GetChild(i).gameObject.GetComponent<Button>();
					currentButton.interactable = false;

				}

				// now that we've loaded the battle, we want to do an enumerator that waits
				// for an input from our user. Either we get a "back up" with a null, or we get
				// a character to attack (for now let's just assume it will be an enemy)
				yield return StartCoroutine(playerTargetPicker.selectTarget());


				while (!playerTargetPicker.hasChosenTarget)
					yield return null;




				if (playerTargetPicker.chosenTarget == null)
				{
					Destroy (playerTargetPicker);

					// if we get a null, make the buttons live again and we'll try again?
					// disable button presses
					// check and see which item is highlighted here before we enter and make that
					// our indexselected
					// check and see which item is highlighted here before we enter and make that
					// our indexselected
					for (var i = 0; i < menuOptions.Count; i++) 
					{
						// if the item is highlighted, set our value to that
						// set i to max
						Button currentButton = optionsBox.transform.GetChild(i).gameObject.GetComponent<Button>();
						currentButton.interactable = true;

						if (i == 0 && indexSelected <= 0)
							currentButton.Select ();
						else if (i == indexSelected)
							currentButton.Select ();
					}

					selectionMade = false;
					isActive = true;
						
					yield break;
				}


				// get our command object
				Commands command = new Commands();

				GameObject targetObject = playerTargetPicker.chosenTarget.gameObject;
				command.setAttackingPlayer (attackingPlayer);
				if (targetObject.GetComponent<PlayerUnit> () != null)
				{
					command.setPlayerBeingBuffed (targetObject.GetComponent<PlayerUnit> ());
				}
				if (targetObject.GetComponent<EnemyUnit> ())
				{
					command.setEnemyUnderAttack (targetObject.GetComponent<EnemyUnit>());
				}

				Destroy (playerTargetPicker);

				// let's resolve our battle commands
				buttonCommand.playerToAlter = "Player";
				command.resolveBattleCommands (buttonCommand);
			}


		}

		yield return new WaitForSeconds (0.01f);

	}



	/// <summary>
	/// Cleans the out options box - destroys all children buttons that could be selected
	/// </summary>
	public void cleanOutOptions()
	{
		foreach (Transform child in optionsBox.transform)
		{
			GameObject.Destroy (child.gameObject);
		}

		// destroy component;

	}


	/// <summary>
	/// Increments the index.
	/// </summary>
	public void incrementIndex()
	{
		/// if we have stings and we are in a pause menu
		if (menuType == "PauseMenu" && stingPlayer != null)
		{
			stingPlayer.playMenuDownSound ();
		} 

		if (indexSelected < menuOptions.Count - 1)
		{
			indexSelected += 1;
		}
		else
		{
			indexSelected = 0;
			optionsBox.GetComponentsInChildren<Button> () [0].Select ();
		}
	}


	/// <summary>
	/// Decrements the index.
	/// </summary>
	public void decrementIndex()
	{
		/// if we have stings and we are in a pause menu
		if (menuType == "PauseMenu" && stingPlayer != null)
		{
			stingPlayer.playMenuDownSound ();
		}

		if (indexSelected > 0)
		{
			indexSelected -= 1;
		}
		else
		{
			indexSelected = menuOptions.Count - 1;
			optionsBox.GetComponentsInChildren<Button> () [indexSelected].Select ();
		}
	}


	/// <summary>
	/// Returns if the user has accepted or declined.
	/// </summary>
	/// <returns><c>true</c>, if accepted, <c>false</c> otherwise.</returns>
	public bool userHasAccepted()
	{
		return hasAccepted;
	}

}
