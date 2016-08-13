using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AcceptDecline : Menu 
{

	// mostly just starts out a new menu
	// has two options
	// is vertical or horizontal


	public string acceptString;
	public string cancelString;
	public bool canCollide;


	public int minWidth;
	public int minHeight;


	bool isColliding = false;
	bool isActive;


	/// <summary>
	/// Initializes a new instance of the <see cref="AcceptDecline"/> class.
	/// </summary>
	/// <param name="accept">Accept.</param>
	/// <param name="cancel">Cancel.</param>
	public AcceptDecline(string accept, string cancel)
	{
		acceptString = accept;
		cancelString = cancel;
	}


	/// <summary>
	/// Initializes a new instance of the <see cref="AcceptDecline"/> class.
	/// </summary>
	/// <param name="accept">Accept.</param>
	/// <param name="cancel">Cancel.</param>
	/// <param name="minWidth">Minimum width.</param>
	/// <param name="minHeight">Minimum height.</param>
	public AcceptDecline (string accept, string cancel, int mWidth, int mHeight)
	{
		acceptString = accept;
		cancelString = cancel;
		minWidth = mWidth;
		minHeight = mHeight;

		menuOptions = new List<Options> () 
		{
			new Options("", acceptString, "", "Player", ""),
			new Options("exit", cancelString, "", "Player", "")
		};
	}


	// wait until we have input
	/// <summary>
	/// Start this instance.
	/// </summary>
	new void Start()
	{
		menuOptions = new List<Options> () 
		{
			//new Options("playGameBoyMiniGame", acceptString, "", "Player", ""),
			new Options("accept", acceptString, "", "Player", ""),
			new Options("exit", cancelString, "", "Player", "")

		};

		menuType = "conversation";

		// if we require layout options... which most of the time we should not
		if (requiresLayout)
		{
			setLayoutOptions (minWidth, minHeight);
		}

		selectionMade = false;
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		// if we are colliding and we have a key hit, then we load menu
		// otherwise, nothing
		if (((canCollide && isColliding) || !canCollide) && Input.anyKeyDown)
		{
			
			// check for key. Otherwise... nothing
			if (Input.GetKeyDown (KeyCode.X) && Time.timeScale > 0 && GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking == false)
			{
				// display our menu
				// set to "talking"
				// set commands of conversation (like "battle" for gameboy)
				loadAcceptDeclineDisplay ();
			}
			// check for key. Otherwise... nothing
			else if (Input.GetKeyDown (KeyCode.Z) && Time.timeScale > 0 && GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking == true)
			{
				// display our menu
				// set to "talking"
				// set commands of conversation (like "battle" for gameboy)
				closeAcceptDeclineDisplay ();
			}
				
		}
		else if (((canCollide && isColliding) || !canCollide) && isActive == true && menuIsActive () == false)
		{
			closeAcceptDeclineDisplay ();
		}
	}


	/// <summary>
	/// Creates the options for the list if we do not use a constructor
	/// </summary>
	public void createOptions()
	{
		menuOptions = new List<Options> () 
		{
			new Options("accept", acceptString, "", "Player", ""),
			new Options("exit", cancelString, "", "Player", "")

		};

		menuType = "conversation";

		// if we require layout options... which most of the time we should not
		if (requiresLayout)
		{
			Debug.Log (" we require layout options and are setting them in here");
			setLayoutOptions (minWidth, minHeight);
		}

		selectionMade = false;
	}

	/// <summary>
	/// Loads the accept decline display.
	/// </summary>
	public void loadAcceptDeclineDisplay()
	{
		isActive = true;

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
		isActive = false;
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
		Debug.Log ("we are here in button clicked");

		yield return null;
	}

}
