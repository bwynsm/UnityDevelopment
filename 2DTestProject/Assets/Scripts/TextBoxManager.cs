using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


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

	public Text theText;

	public TextAsset textFile;

	public GameObject prefabButton;


	// the other items.
	public Text speakerText;
	public GameObject speakerPanel;


	public CharacterConversable player; // if a character can talk.. it's one of these
	public bool isActive = false;

	// I don't have to do anything if we are waiting for a keypress
	public bool waitingForKey = false;

	Conversation dialogueTree;

	// what if the character is already talking?
	// we don't want to start another box collider...


	// Use this for initialization
	void Start () 
	{

		// at the moment we don't have a textbox visible on the start of the game
		DisableTextBox ();

		// load our dialogue tree object
		dialogueTree = new Conversation(textFile, "1");

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
			string playerName;




			// if we have no text, then set in our first item
			if (theText.text == null || theText.text == "")
			{
				Speech nextText = dialogueTree.getItem ();

				// get object by name now that we have it
				playerName = nextText.name;
				player = GameObject.Find(playerName).GetComponent<CharacterConversable>();
			
				theText.text = nextText.SpeechText.Trim();
				speakerText.text = player.playerName.Trim();


				EnableTextBox ();
			}
				

			// if we want to go the next line of text, update the current line
			else if (Input.GetKeyDown (KeyCode.X) )
			{
				waitingForKey = false;

				// if we have another item, show that
				// otherwise, we can close the textbox if we are done talking
				if (dialogueTree.hasNextItem ())
				{
					dialogueTree.incrementIndex ();
					Speech nextText = dialogueTree.getItem ();

					// get object by name now that we have the updated item
					playerName = dialogueTree.getItem().name;
					player = GameObject.Find(playerName).GetComponent<CharacterConversable>();

					if (nextText.SpeechText != null && nextText.SpeechText != "")
					{
						theText.text = nextText.SpeechText.Trim ();
					}



					// not over yet
					//string optionsText = "";

					// get the type
					if (nextText.type == "options")
					{
						// loop over options and display
						setupOptions(nextText.options);

						//theText.text = optionsText;
					}

					speakerText.text = player.playerName.Trim();
					EnableTextBox ();
				}

				// we are done talking
				else
				{
					DisableTextBox ();
				}
			}


		}


	}



	/// <summary>
	/// Moves the text box.
	/// </summary>
	//void moveTextBox()
	//{
		// get where the player is
		//Transform playerPosition = player.whereAmI ();

		// do some math to determine where the text box will need to sit on the screen
		// no matter what the screen size
		//Vector3 goScreenPos = Camera.main.WorldToScreenPoint (playerPosition.position);


		// get our text box in place
		//textBox.transform.position = new Vector3(goScreenPos.x + 75, goScreenPos.y + 110, 0.1f);
	//}


	/// <summary>
	/// Enables the text box.
	/// Also freezes relevant players ideally. This should not be done here, but is
	/// at the moment. I'll figure that out later
	/// </summary>
	public void EnableTextBox()
	{
		// true - but also set our currentline?
		//moveTextBox();


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
		speakerText.text = "";
		isActive = false;
		player.freeze = false;

		// hell, if this does not equal the player, make them stop too
		if (player.name != "Player") 
		{
			GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerMovement> ().freeze = false;

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



	public void setupOptions(List<string> options)
	{

		int indexNum = 1;
		// for each of our options, create some sort of button
		// in our panel and put it at the right spot
		foreach (string option in options)
		{
			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.transform.SetParent (theText.transform, false);
			goButton.transform.localScale = new Vector3(10, 10, 1);
			goButton.GetComponentInChildren<Text>().text = "Option : " + option;
			//goButton.GetComponent<Text>().text = "OPTION : " + option;

			// also add in the resulting function call
			Button tempButton = goButton.GetComponent<Button>();
			tempButton.onClick.AddListener(() => ButtonClicked(indexNum));
			indexNum++;
		}


	}

	public void ButtonClicked(int number)
	{
		Debug.Log ("BUTTON CLICKED :: " + number);
	}

}
