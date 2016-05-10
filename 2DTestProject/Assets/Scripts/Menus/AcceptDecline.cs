using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcceptDecline : Menu 
{

	// mostly just starts out a new menu
	// has two options
	// is vertical or horizontal

	public bool isVertical; // if it is vertical, true. Otherwise, false.

	public string acceptString;
	public string cancelString;

	bool isColliding = false;


	// wait until we have input
	/// <summary>
	/// Start this instance.
	/// </summary>
	new void Start()
	{
		menuOptions = new List<Options> () 
		{
			new Options("playGameBoyMiniGame", acceptString, "", "Player", ""),
			new Options("exit", cancelString, "", "Player", "")

		};

		menuType = "conversation";
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// if we are colliding and we have a key hit, then we load menu
		// otherwise, nothing
		if (isColliding && Input.anyKeyDown)
		{
			// check for key. Otherwise... nothing
			if (Input.GetKeyDown (KeyCode.X) && Time.timeScale > 0 && GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking == false)
			{

				Debug.Log ("we are pushing down a key" + menuOptions.Count);
				// display our menu
				// set to "talking"
				// set commands of conversation (like "battle" for gameboy)
				loadAcceptDeclineDisplay();
			}
			// check for key. Otherwise... nothing
			else if (Input.GetKeyDown (KeyCode.Z) && Time.timeScale > 0 && GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking == true)
			{

				Debug.Log ("we are pushing down a key" + menuOptions.Count);
				// display our menu
				// set to "talking"
				// set commands of conversation (like "battle" for gameboy)
				closeAcceptDeclineDisplay();
			}
		}
	}


	/// <summary>
	/// Loads the accept decline display.
	/// </summary>
	public void loadAcceptDeclineDisplay()
	{
		Debug.Log ("we are here");

		GameObject.Find ("Player").GetComponent<PlayerMovement> ().setFrozen ();
		GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking = true;
		optionsBox.SetActive (true);



		// what if we could send in the options into a menu creator, and just
		// tell it that we want to get our particular prefab
		//optionsBox.AddComponent<Menu>();
		menuType = "conversation";


		loadOptions (menuOptions);
		StartCoroutine (Initialize ());

	}


	/// <summary>
	/// Closes the accept decline display.
	/// </summary>
	public void closeAcceptDeclineDisplay()
	{
		GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<PlayerUnit> ().freeze = false;
		GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking = false;
		optionsBox.SetActive (false);

		Destroy (optionsBox.GetComponent<WaitingForTime> ());
		//Destroy (optionsMenu);

		cleanOutOptions ();
	}


	/// <summary>
	/// Raises the trigger exit2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		isColliding = false;
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		isColliding = true;
	}


	/// <summary>
	/// When one of our menu buttons is clicked, we go here to deal with the command issued
	/// </summary>
	/// <param name="buttonCommand">Button command.</param>
	/// <returns>The clicked.</returns>
	public new IEnumerator ButtonClicked(Options buttonCommand)
	{
		Debug.Log ("we are in this button clicked");

		yield return null;
	}

}
