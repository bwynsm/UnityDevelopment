using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu : MonoBehaviour 
{

	public Texture2D background;

	public GameObject optionsBox;
	public GameObject prefabButton;
	//public Texture2D wip3Logo;


	public List<Options> menuOptions;
	public string menuType;
	private bool isActive = true;


	private string commands;
	public bool selectionMade = false;
	private int indexSelected = 0;

	public WaitingForTime waitingObject;

	IEnumerator Start () 
	{
		selectionMade = false;
		isActive = true;
		indexSelected = 0;


		optionsBox.AddComponent<WaitingForTime> ();
		waitingObject = optionsBox.GetComponent<WaitingForTime> ();

		yield return StartCoroutine (waitingObject.PauseBeforeInput());

		// check and see which item is highlighted here before we enter and make that
		// our indexselected
		Debug.Log("Child Count : " + optionsBox.transform.childCount);


		for (var i = 0; i < menuOptions.Count; i++) 
		{
			// if the item is highlighted, set our value to that
			// set i to max
			Button currentButton = optionsBox.transform.GetChild(i).gameObject.GetComponent<Button>();
			currentButton.interactable = true;
		
		}

		while (selectionMade == false) 
		{

			yield return StartCoroutine (waitingObject.WaitForKeyDown ());

			if (!selectionMade && isActive == true && Input.anyKey) 
			{
				Debug.Log ("indexselected : " + indexSelected);

				if (Input.GetKeyDown (KeyCode.DownArrow) == true) 
				{
					Debug.Log ("we are here");
					if (indexSelected < menuOptions.Count - 1)
					{
						indexSelected += 1;
					} 
					else 
					{
						indexSelected = 0;
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
					}
				}

				if (Input.GetKey (KeyCode.Return)) {
					selectionMade = true;
					ButtonClicked (menuOptions [indexSelected]);
				}

				if (Input.GetKey (KeyCode.X)) {
					selectionMade = true;
					Debug.Log ("index selected : " + indexSelected);

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
		if (menuOptions.Count == 0)
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
			Debug.Log ("Option : " + menuOptions [i].option);

			RectTransform rect = goButton.GetComponentInChildren<Text> ().GetComponent<RectTransform>();
			rect.offsetMax = new Vector2 (-10f, -10f);
			rect.offsetMin = new Vector2 (10f, 10f);


			Options optionItem = new Options ();
			optionItem = menuOptions [i];
			Debug.Log ("OPTION ITEM COMMAND : " + optionItem.command);

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
			Debug.Log ("Selection Made" + buttonCommand.command);


			/// what type of options list do we have? conversation or more main menu?
			if (menuType == "conversation")
			{
				// if we have a number, just go to that number
				if (buttonCommand.command.Contains ("id#"))
				{
					string command = (buttonCommand.command.Split ('#')) [1];

					// change conversation id to that?
					// get player by tag name
					CharacterConversable playerObject = GameObject.Find (buttonCommand.playerToAlter).GetComponent<CharacterConversable> ();
					playerObject.GetComponent<ActivateTextAtLine> ().dialogueID = command;
				}


			}




		}

	}




}
