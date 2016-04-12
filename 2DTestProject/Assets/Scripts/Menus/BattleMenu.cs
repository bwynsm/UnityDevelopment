using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Battle menu.
/// </summary>
public class BattleMenu : MonoBehaviour
{


	public TextAsset battleXML;
	public GameObject battlePanel;
	public GameObject prefabButton;
	public Menu optionsMenu;
	public Conversation battleOptionsManager;


	// get all of the players in this battle
	public List<CharacterConversable> allCombatants;

	public bool updatingItems = false;
	public bool doneWaitingForClear = false;

	public bool isMyTurn = false;
	private bool toggled = false;


	/// <summary>
	/// Start this instance.
	/// Gets our set of attacks for character
	/// </summary>
	void Start()
	{
		// let's only do this once

		if (battleXML != null)
		{
			// get the xml
			battleOptionsManager = new Conversation (battleXML, "PlayerBasic");

			updatingItems = true;
		}
	}


	/// <summary>
	/// If it is our turn, updates the battle panel
	/// </summary>
	void Update()
	{
		if (isMyTurn)
		{

			// what we'll do in here is keep track of some variables to prevent us from attacking too often
			// so once we make a selection, we'll clean things out
			// then once we've cleaned things out, we can start 
			if (updatingItems && battlePanel != null)
			{
				// then we do nothing.
				if (!doneWaitingForClear)
				{
					cleanOutOptions ();
				}

				// if we are done waiting for our clear, and we have a battle panel
				// then we're going to update our items and not do it more than once
				else if (doneWaitingForClear && battlePanel.activeInHierarchy)
				{
					doneWaitingForClear = false;
					updatingItems = false;

					getBoxes ();
				}
			}

			// if our battle panel is null, destroy our object
			else if (battlePanel == null)
			{
				Destroy (this);
			}
		}

		// destroy this if it isn't our turn and we ahve no battle panel
		else if (!isMyTurn && battlePanel == null)
		{
			Destroy (this);
		}
		else if (!isMyTurn && optionsMenu != null && !toggled)
		{
			toggled = true;

			// hide our options buttons
			optionsMenu.toggleOptionsDisplay(false);
		}


	}
		
	/// <summary>
	/// Gets the boxes for selection of attacks
	/// </summary>
	public void getBoxes()
	{



		// if our conversation has another item, get it
		// this is if we branched (like went into items or spells etc)
		if (battleOptionsManager.hasNextItem ()) {
			battleOptionsManager.incrementIndex ();
			Speech nextText = battleOptionsManager.getItem ();

			// loop over 		options and display
			showBattleMenu (nextText.options);

		} 

		// otherwise, show our current item
		else
		{
			Speech nextText = battleOptionsManager.getItem ();


			// loop over options and display
			showBattleMenu (nextText.options);
		}


	}


	/// <summary>
	/// Shows the battle menu.
	/// </summary>
	/// <param name="options">Options.</param>
	public void showBattleMenu(List<Options> options)
	{


		// what if we could send in the options into a menu creator, and just
		// tell it that we want to get our particular prefab
		// if we don't have a menu right now.... add one
		if (optionsMenu == null && battlePanel.GetComponent<Menu> () == null) 
		{
			battlePanel.AddComponent<Menu> ();
		} 

			

		// let's get the player character their menu back
		optionsMenu = battlePanel.GetComponent<Menu>();

		optionsMenu.prefabButton = prefabButton;
		optionsMenu.optionsBox = battlePanel;
		optionsMenu.menuOptions = options;
		optionsMenu.menuType = "BattleMenu";
		optionsMenu.attackingPlayer = gameObject.GetComponent<PlayerUnit> ();
		optionsMenu.targetPlayer = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<EnemyUnit>();


		// hide our options buttons
		optionsMenu.toggleOptionsDisplay(true);
		toggled = false;


		//optionsMenu.transform.localScale = new Vector3(1, 1, 1);
		optionsMenu.renameOptions(options);




	}
		
		

	/// <summary>
	/// Cleans the out options box - destroys all children buttons that could be selected
	/// </summary>
	public void cleanOutOptions()
	{

		optionsMenu = null;

			
		// for each of our child options, clean them out
		foreach (var waitingObject in battlePanel.GetComponents<WaitingForTime>()) {
			Destroy (waitingObject);
		}
		Destroy (battlePanel.GetComponent<Menu> ());

		doneWaitingForClear = true;

	}
}


