using UnityEngine;
using System.Collections;


/**
 * ActivateTextAtLine
 * This class allows us to start reading a dialogue for a character.
 * It contains a text asset which we'll use to run our TextBoxManager
 * and deals with waiting for button presses and destruction of zones
 * if necessary for colliders (if an NPC has a shout zone)
 */ 
public class ActivateTextAtLine : MonoBehaviour 
{



	public TextAsset theText; // the text file

	public TextBoxManager theTextBox; // the textbox to display in
	public bool destroyWhenActivated; // if we have a collider zone for shouting, destroy potentially when done

	public bool requireButtonPress; // button required to talk
	private bool waitForPress; // waiting for a button press to initiate talking
	public CharacterConversable player; // a character that can hold a conversation


	public string dialogueID = "1";

	private bool isColliding = false;


	// Use this for initialization
	void Start () 
	{
		// get our text box
		theTextBox = FindObjectOfType<TextBoxManager> ();
	}


	// Update is called once per frame
	// In update we are waiting for a keypress possibly
	// and destroying if that is the kind of object we are.
	void Update () 
	{

		// also, we don't want to enable if we are already enabled.
		// we also have to have text..
		if (waitForPress && Input.GetKeyDown (KeyCode.X) && theTextBox.isActive != true && isColliding && !theTextBox.inConversation) 
		{
			theTextBox.inConversation = true;
			theTextBox.reloadScript (theText, dialogueID);

			if (destroyWhenActivated) 
			{
				Destroy (gameObject.GetComponent<Collider2D> ());
			}
		}
		else if ( waitForPress && Input.GetKeyDown (KeyCode.X) && theTextBox.isActive != true && isColliding)
		{
			theTextBox.inConversation = false;
		}
	}


	// if something walks into our zone, then we want to see if we are waiting
	// for something more. If the type of other is our player, (and not just another NPC)
	// then we may want to do something to display our text
	void OnTriggerEnter2D(Collider2D other)
	{
		if (player.isTalking)
		{
			return;
		}

		// if we aren't shouting, but waiting for the player to talk to us
		if (requireButtonPress) 
		{
			if (other.name == "Player")
				isColliding = true;

			waitForPress = true;
			return;
		}

		Debug.Log ("we are here");


		// if our other person is the player...
		if (other.name == "Player") 
		{

			theTextBox.setPlayer (player);
			theTextBox.reloadScript (theText, dialogueID);

			// if we want an NPC to shout only once
			if (destroyWhenActivated) 
			{
				Destroy (gameObject.GetComponent<Collider2D> ());
			}




		}
	}


	// whenever the player leaves, he can't still chat
	// can't talk to everyone from far away
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player") 
		{
			isColliding = false;
			waitForPress = false;
		}
	}
}
