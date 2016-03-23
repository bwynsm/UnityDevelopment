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




	// other character we are looking at - their location
	private PlayerMovement mainPlayer;

	public string dialogueID = "1";

	private bool isColliding = false;
	private bool startedTalking = false;


	// Use this for initialization
	void Start () 
	{
		// get our text box
		theTextBox = FindObjectOfType<TextBoxManager> ();
		mainPlayer = GameObject.Find ("Player").GetComponent<PlayerMovement> ();


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
			startedTalking = false;
		}
	}


	/// <summary>
	/// Updates after the main update - here is where we move our characters to face
	/// each other if they are not already
	/// </summary>
	void LateUpdate()
	{
		if (theTextBox.inConversation == true && !startedTalking && isColliding)
		{
			startedTalking = true;
			Animator anim = player.GetComponent<Animator> ();
			Vector2 direction = new Vector2 (mainPlayer.transform.position.x - 
											player.transform.position.x, 
											mainPlayer.transform.position.y - 
											player.transform.position.y);
			anim.SetBool ("isWalking", true);
			anim.SetFloat ("input_x", direction.x);
			anim.SetFloat ("input_y", direction.y);
			anim.SetBool ("isWalking", false);
			//rbody.MovePosition(rbody.position + direction * 0.01f);

			// do the same change for the main player in reverse
			anim = mainPlayer.GetComponent<Animator> ();
			direction = new Vector2 (player.transform.position.x - mainPlayer.transform.position.x, player.transform.position.y - mainPlayer.transform.position.y);
			anim.SetBool ("isWalking", true);
			anim.SetFloat ("input_x", direction.x);
			anim.SetFloat ("input_y", direction.y);
			anim.SetBool ("isWalking", false);
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
			{
				isColliding = true;
			}

			waitForPress = true;
			return;
		}




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



	/// <summary>
	/// Activates new text at a new location. This is for if we hit a branch and want
	/// to start down a different conversational path
	/// </summary>
	/// <param name="newConversationID">New conversation I.</param>
	public void activateNewText(string newConversationID)
	{
		theTextBox.changeScriptLocation (newConversationID);

		// if we want an NPC to shout only once
		if (destroyWhenActivated) 
		{
			Destroy (gameObject.GetComponent<Collider2D> ());
		}

	}
}
