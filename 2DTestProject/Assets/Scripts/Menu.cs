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

	void Start () 
	{
		selectionMade = false;
		isActive = true;
		indexSelected = 0;
	}

	public bool menuIsActive()
	{
		return isActive;
	}

	void Update () 
	{

		if (!selectionMade && isActive == true && Input.anyKey)
		{
			Debug.Log (menuOptions.Count);
			if (Input.GetKeyDown (KeyCode.DownArrow) == true)
			{
				if (indexSelected < menuOptions.Count - 1)
				{
					indexSelected += 1;
				} else
				{
					indexSelected = 0;
				}
			}

			if (Input.GetKeyDown (KeyCode.UpArrow) == true)
			{
				if (indexSelected > 0)
				{
					indexSelected -= 1;
				} else
				{
					indexSelected = menuOptions.Count - 1;
				}
			}

			if (Input.GetKey (KeyCode.Return))
			{
				selectionMade = true;
				ButtonClicked (menuOptions [indexSelected]);
			}

			if (Input.GetKey (KeyCode.X))
			{
				selectionMade = true;
				ButtonClicked (menuOptions [indexSelected]);
			}
				
		}





	}




	public void loadOptions(List<Options> options)
	{
		Debug.Log (menuOptions.Count);
		if (menuOptions.Count == 0)
		{
			menuOptions.AddRange (options);
		}
		Debug.Log ("length of list : " + menuOptions.Count);
		isActive = true;


		//goButton.transform.localScale = new Vector3(1, 1, 1);



		// for each of our options, create some sort of button
		// in our panel and put it at the right spot
		for (int i = 0; i < options.Count; i++)
		{
			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.GetComponentInChildren<Text>().text = "Option : " + menuOptions[i].option;
			Debug.Log(menuOptions[i].toString());

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
			//goButton.transform.localScale = new Vector3(1, 1, 1);


		}

		Debug.Log ("Menu OPTIONS LENGTH : " + menuOptions.Count);
			
	}

	void ButtonClicked(Options buttonCommand)
	{
		isActive = false;

		if (selectionMade)
		{
			Debug.Log ("Selection Made");


			/// what type of options list do we have? conversation or more main menu?
			if (menuType == "conversation")
			{
				// if we have a number, just go to that number
				if (buttonCommand.command.Contains ("id#"))
				{
					string command = (buttonCommand.command.Split ('#')) [1];

					// change conversation id to that?
					// get player by tag name
					//Debug.Log("Player to Alter : " + tagItem.playerToAlter);
					CharacterConversable playerObject = GameObject.Find (buttonCommand.playerToAlter).GetComponent<CharacterConversable> ();
					//Debug.Log ("Player Name? : " + playerObject.playerName);
					playerObject.GetComponent<ActivateTextAtLine> ().dialogueID = command;
				}


			}




		}

	}
}
