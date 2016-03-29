using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class BattleMenu : MonoBehaviour
{


	public TextAsset battleXML;
	public GameObject battlePanel;
	public GameObject prefabButton;
	public Menu optionsMenu;
	public Conversation battleOptionsManager;


	public bool updatingItems = false;
	public bool doneWaitingForClear = false;

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

	void Update()
	{
		// what we'll do in here is keep track of some variables to prevent us from attacking too often
		// so once we make a selection, we'll clean things out
		// then once we've cleaned things out, we can start 
		if (updatingItems && battlePanel != null) 
		{
			// then we do nothing.
			if (!doneWaitingForClear ) {
				
				cleanOutOptions ();

			} 
			else if (doneWaitingForClear && battlePanel.activeInHierarchy) {
				doneWaitingForClear = false;
				updatingItems = false;

				getBoxes ();
			}
		} 
		else if (battlePanel == null)
		{
			Destroy (this);
		}


	}
		

	public void getBoxes()
	{
		
		if (battleOptionsManager.hasNextItem ()) {
			battleOptionsManager.incrementIndex ();
			Speech nextText = battleOptionsManager.getItem ();

			// loop over 		options and display
			showBattleMenu (nextText.options);




		} 
		else
		{
			Speech nextText = battleOptionsManager.getItem ();


			// loop over options and display
			showBattleMenu (nextText.options);
		}


	}

	public void showBattleMenu(List<Options> options)
	{


		// what if we could send in the options into a menu creator, and just
		// tell it that we want to get our particular prefab
		Debug.Log("in show battle menu");

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



		//optionsMenu.transform.localScale = new Vector3(1, 1, 1);
		optionsMenu.renameOptions(options);




	}
		
		

	/// <summary>
	/// Cleans the out options box - destroys all children buttons that could be selected
	/// </summary>
	public void cleanOutOptions()
	{

		optionsMenu = null;

			
		foreach (var waitingObject in battlePanel.GetComponents<WaitingForTime>()) {
			Destroy (waitingObject);
		}
		Destroy (battlePanel.GetComponent<Menu> ());

		doneWaitingForClear = true;

	}
}


