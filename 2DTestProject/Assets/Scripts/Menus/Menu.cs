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

	public Texture2D background;		// unused at the moment - texture background for menu

	public GameObject optionsBox;		// this is the options box panel that we are displaying in
	public GameObject prefabButton;		// this is the current button we are using for display
	//public Texture2D wip3Logo;


	public List<Options> menuOptions;	// list of all the options in menu
	public string menuType;				// conversation, main menu, etc
	private bool isActive = true; 		// if our menu is active


	private string commands;			// string of commands made by choosing something
	public bool selectionMade = false;	// if we have made a selection of a button
	private int indexSelected = 0;     	// button selected

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
		yield return StartCoroutine (Initialize ());
	}

	public IEnumerator Initialize()
	{


		selectionMade = false;
		isActive = true;
		indexSelected = 0;

		optionsBox.AddComponent<WaitingForTime> ();
		waitingObject = optionsBox.GetComponent<WaitingForTime> ();

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
		
		}

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


			if (!selectionMade && isActive == true && Input.anyKey) 
			{

				if (Input.GetKeyDown (KeyCode.DownArrow) == true) 
				{
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

				if (Input.GetKeyDown (KeyCode.UpArrow) == true) 
				{
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

				if (Input.GetKey (KeyCode.Return)) {
					selectionMade = true;
					ButtonClicked (menuOptions [indexSelected]);
				}

				if (Input.GetKey (KeyCode.X)) {
					selectionMade = true;

					ButtonClicked (menuOptions [indexSelected]);
				}

			}


		}



	}
		

	public bool menuIsActive()
	{
		return isActive;
	}




	// yields to a coroutine

	public void loadOptions(List<Options> options)
	{
		if (menuOptions == null || menuOptions.Count == 0)
		{
			menuOptions.AddRange (options);
		}
		isActive = true;


		//goButton.transform.localScale = new Vector3(1, 1, 1);



		// for each of our options, create some sort of button
		// in our panel and put it at the right spot
		for (int i = 0; i < options.Count; i++)
		{
			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.GetComponentInChildren<Text>().text = "Option : " + menuOptions[i].option;

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

		}
			
			
	}




	void ButtonClicked(Options buttonCommand)
	{
		isActive = false;
		Destroy (waitingObject);

		if (selectionMade)
		{


			/// what type of options list do we have? conversation or more main menu?
			if (menuType == "conversation")
			{
				Commands command = new Commands ();
				command.resolveConversationCommands (buttonCommand);


			}

			// if we have a main menu, we can send those commands over to commands as well
			// and just run our functions for that.
			else if (menuType == "PauseMenu")
			{
				Debug.Log ("Pause Menu Button Clicked! : " + buttonCommand);

				Commands command = new Commands ();
				command.resolvePauseMenuCommands (buttonCommand);
				// what is our command? 
			}



		}

	}




}
