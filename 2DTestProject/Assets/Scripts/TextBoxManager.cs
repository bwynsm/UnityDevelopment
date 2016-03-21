using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Text box manager.
/// 
/// This class is a bit bloated at the moment. It controls everything text box
/// for the dialogues. It has a text, textbox, and a file that is sent to it
/// from the activation. 
/// I haven't done any good polymorphism to take some of the load off of this class, but I should.
/// </summary>
public class TextBoxManager : MonoBehaviour 
{
	public GameObject textBox;
	public GameObject optionsBox;

	public Canvas canvasItem;

	public Text theText;

	public TextAsset textFile;

	public GameObject prefabButton;
	public bool inConversation = false;

	// the other items.
	public Text speakerText;
	public GameObject speakerPanel;


	public CharacterConversable player; // if a character can talk.. it's one of these
	public bool isActive = false;

	// I don't have to do anything if we are waiting for a keypress
	public bool waitingForKey = false;

	Conversation dialogueTree;

	private bool firstLoad = true;

	// what if the character is already talking?
	// we don't want to start another box collider...


	// Use this for initialization
	void Start () 
	{

		firstLoad = true;

		// at the moment we don't have a textbox visible on the start of the game
		DisableTextBox ();
		DisableOptionsBox ();

		// load our dialogue tree object
		//dialogueTree = new Conversation(textFile, "1");

	}

	
	// Update is called once per frame
	void Update () 
	{
		
		// we don't want code to continue
		if (!isActive || player == null) 
		{
			return;
		}


		// if we are waiting on a keypress and we receive one, move on
		// this is a gimmick that we may change later
		if (Input.anyKey)
		{
			waitingForKey = false;
		}

		// if we are not null and we do not have an item
		// then we want to get an item
		// if we have an item, we are waiting until the next item
		if (!waitingForKey )
		{
			waitingForKey = true;
			// the first thing that we have to do is to get which player is talking
			// player name = ....


			// if we have no text, then set in our first item
			if (firstLoad)
			{
				firstLoad = false;
				getBoxes ();

			}
				

			// if we want to go the next line of text, update the current line
			else if (Input.GetKeyDown (KeyCode.X) )
			{
				waitingForKey = false;

				// if we have another item, show that
				// otherwise, we can close the textbox if we are done talking
				getBoxes();
			}


		}


	}




	public void getBoxes()
	{
		string playerName;

		if (dialogueTree.hasNextItem ())
		{
			dialogueTree.incrementIndex ();
			Speech nextText = dialogueTree.getItem ();

			// get object by name now that we have the updated item
			playerName = dialogueTree.getItem().name;
			//Debug.Log ("Player name : " + playerName);
			player = GameObject.Find(playerName).GetComponent<CharacterConversable>();
			speakerText.text = player.playerName.Trim();

			if (nextText.SpeechText != null && nextText.SpeechText != "")
			{
				theText.text = nextText.SpeechText.Trim ();
				EnableTextBox ();
			}



			// not over yet
			//string optionsText = "";

			// get the type
			if (nextText.type == "options")
			{
				// loop over options and display
				setupOptions(nextText.options);
				EnableOptionsBox ();
			}



		}

		// we are done talking
		else
		{
			DisableTextBox ();
			DisableOptionsBox ();
		}
	}
		


	/// <summary>
	/// Enables the text box.
	/// Also freezes relevant players ideally. This should not be done here, but is
	/// at the moment. I'll figure that out later
	/// </summary>
	public void EnableTextBox()
	{
		// true - but also set our currentline?
		//moveTextBox();

		DisableOptionsBox ();
		textBox.SetActive (true);
		speakerPanel.SetActive (true);
		isActive = true;


		// if is active, freeze our player. They cannot talk and move at the same time
		if (isActive) 
		{
			// set our player to frozen
			player.freeze = true;

			// hell, if this does not equal the player, make them stop too
			if (player.name != "Player") 
			{
				GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerMovement> ().freeze = true;

			}
		}

	}


	/// <summary>
	/// Disables the text box.
	/// 
	/// Also allows the players to all move again. You may again go 
	/// about your business
	/// </summary>
	public void DisableTextBox()
	{
		textBox.SetActive (false);
		speakerPanel.SetActive (false);
		theText.text = "";
		isActive = false;
		player.freeze = false;

		// hell, if this does not equal the player, make them stop too
		if (player.name != "Player") 
		{
			GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerMovement> ().freeze = false;

		}
	}




	public void DisableOptionsBox()
	{
		cleanOutOptions ();
		optionsBox.SetActive (false);
		isActive = false;
		player.freeze = false;
		speakerPanel.SetActive (false);
	}

	public void EnableOptionsBox()
	{
		// true - but also set our currentline?
		//moveTextBox();

		DisableTextBox ();
		optionsBox.SetActive (true);
		speakerPanel.SetActive (true);
		isActive = true;


		// if is active, freeze our player. They cannot talk and move at the same time
		if (isActive) 
		{
			// set our player to frozen
			player.freeze = true;

			// hell, if this does not equal the player, make them stop too
			if (player.name != "Player") 
			{
				GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerMovement> ().freeze = true;

			}
		}

	}



	/// <summary>
	/// Reloads the script.
	/// 
	/// The script we are reloading is the script that contains the dialogue
	/// This is most often done in activatetextatline.
	/// </summary>
	/// <param name="theNewText">The new text.</param>
	public void reloadScript(TextAsset theNewText, string conversationID)
	{
		firstLoad = true;

		// if we have a new text file that exists
		// then we can load our new conversation window
		if (theNewText != null) 
		{
			dialogueTree = null;
			theText.text = "";

			// load our dialogue tree object
			textFile = theNewText;
			Conversation temp = new Conversation(textFile, conversationID);
			dialogueTree = temp;

			isActive = true;
		}

	}


	/// <summary>
	/// Sets the player we are conversing with.
	/// 
	/// Not sure I'll need this with the dialogue system
	/// </summary>
	/// <param name="talkToPlayer">Talk to player.</param>
	public void setPlayer(CharacterConversable talkToPlayer)
	{
		player = talkToPlayer;
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

	}



	public void setupOptions(List<Options> options)
	{
		// disable text box
		//DisableTextBox();
		cleanOutOptions ();
		EnableOptionsBox ();




		//goButton.transform.localScale = new Vector3(1, 1, 1);

		// for each of our options, create some sort of button
		// in our panel and put it at the right spot
		for (int i = 0; i < options.Count; i++)
		{

			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.GetComponentInChildren<Text>().text = "Option : " + options[i].option;

			Options optionItem = new Options ();
			optionItem = options [i];
			//goButton.AddComponent(
			goButton.GetComponent<Button>().onClick.AddListener(
				() => {  ButtonClicked(optionItem); }
			);
			goButton.transform.SetParent (optionsBox.transform, false);
			//goButton.transform.localScale = new Vector3(1, 1, 1);


		}


	}

	public void ButtonClicked(Options tagItem)
	{
		// based off of which tag item we selected, we can do a number of things.
		// if we selected option 1, we can change that players number to something for
		// activate text at line
		// we need to get the other person we are talking to though - how do we do that? 
		// do we store that integer in the command?
		//Debug.Log("OPTIONS ITEM : Command: " + tagItem.command + " Option: " + tagItem.option + " Player : " + tagItem.playerToAlter + " CurrentPlayer:" + tagItem.currentPlayer);

		if (tagItem.playerToAlter != null)
		{
			// get player by tag name
			//Debug.Log("Player to Alter : " + tagItem.playerToAlter);
			CharacterConversable playerObject = GameObject.Find (tagItem.playerToAlter).GetComponent<CharacterConversable> ();
			//Debug.Log ("Player Name? : " + playerObject.playerName);
			playerObject.GetComponent<ActivateTextAtLine> ().dialogueID = tagItem.command;

		}

		// once the button is clicked, we want to immediately shut down and go to the next item

		// conversation jump possibility?
		// send in an ID and see our jump


		getBoxes();

	}

}
