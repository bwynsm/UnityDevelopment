using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : Menu
{

	public bool gamePaused = false;





	new void Start()
	{
		menuOptions = new List<Options> () 
		{
			new Options("", "Other", "", "Player"),
			new Options("", "Stats", "", "Player"),
			new Options("", "Party", "", "Player"),
			new Options("", "Settings", "", "Player"),
			new Options("exit", "Exit", "", "Player")

		};
	}



	void Awake()
	{
		if (!gamePaused)
		{
			optionsBox.SetActive (false);
		}
	}



	/// <summary>
	/// Update this instance.
	/// </summary>
	// it's not a type of menu, but it uses menu
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && gamePaused == false && GameObject.Find ("Player").GetComponent<PlayerUnit> ().isTalking == false)
		{
			
			pauseGame ();
		} 
		else if (Input.GetKeyDown (KeyCode.Escape) && gamePaused == true)
		{
			unpauseGame ();
		} 
			
	}


	public void pauseGame()
	{
		// pause the game
		Time.timeScale = 0;

		gamePaused = true;
		GameObject.Find ("Player").GetComponent<PlayerMovement> ().setFrozen ();
		optionsBox.SetActive (true);



		// what if we could send in the options into a menu creator, and just
		// tell it that we want to get our particular prefab
		//optionsBox.AddComponent<Menu>();
		menuType = "PauseMenu";

		loadOptions (menuOptions);
		StartCoroutine (Initialize ());

		//optionsMenu.transform.localScale = new Vector3(1, 1, 1);
		//optionsMenu.loadOptions(optionsList);
	}

	public void unpauseGame()
	{
		Time.timeScale = 1;
		gamePaused = false;
		GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<PlayerUnit> ().freeze = false;
		optionsBox.SetActive (false);

		Destroy (optionsBox.GetComponent<WaitingForTime> ());
		//Destroy (optionsMenu);

		cleanOutOptions ();


		// try and restart the music
		//GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<MusicChanger>().transitionOut();
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

}
