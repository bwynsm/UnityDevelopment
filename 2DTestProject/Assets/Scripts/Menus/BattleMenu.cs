using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class BattleMenu : MonoBehaviour
{


	public TextAsset battleXML;
	public GameObject battlePanel;
	public GameObject prefabButton;
	private Menu optionsMenu;
	public Conversation battleOptionsManager;
	public bool waitingForKey = true;

	void Start()
	{
		// let's only do this once

		if (battleXML != null)
		{
			// get the xml
			battleOptionsManager = new Conversation (battleXML, "PlayerBasic");

			// get first set of options
			battleOptionsManager.incrementIndex ();
			Speech nextItem = battleOptionsManager.getItem ();

			showBattleMenu (nextItem.options);
		}
	}

	void Update()
	{


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
			
			// the first thing that we have to do is to get which player is talking
			// player name = ....

			// wait for a key
			// if key == z, let's go back to basic
			if (Input.GetKey (KeyCode.Z) && optionsMenu.selectionMade == true && optionsMenu.menuIsActive() == true)
			{
				waitingForKey = true;

				// go back to basic
				getBoxes ();
			} 
			else if (Input.GetKey (KeyCode.X) && optionsMenu.selectionMade == true && optionsMenu.menuIsActive() == true)
			{
				waitingForKey = true;
				getBoxes ();
			}

		}
	}

	public void getBoxes()
	{
		
		if (battleOptionsManager.hasNextItem ())
		{
			battleOptionsManager.incrementIndex ();
			Speech nextText = battleOptionsManager.getItem ();


			// loop over options and display
			showBattleMenu (nextText.options);

			Debug.Log ("GET BOXES A ");


		} 
		else
		{
			Speech nextText = battleOptionsManager.getItem ();


			// loop over options and display
			showBattleMenu (nextText.options);

			Debug.Log ("GET BOXES B ");
		}


	}

	public void showBattleMenu(List<Options> options)
	{

		// get the xml and make a document from it
		// disable text box
		//DisableTextBox();
		cleanOutOptions ();

		// what if we could send in the options into a menu creator, and just
		// tell it that we want to get our particular prefab
		battlePanel.AddComponent<Menu>();

		optionsMenu = battlePanel.GetComponent<Menu>();
		optionsMenu.prefabButton = prefabButton;
		optionsMenu.optionsBox = battlePanel;
		optionsMenu.menuOptions = options;
		optionsMenu.menuType = "BattleMenu";

		//optionsMenu.transform.localScale = new Vector3(1, 1, 1);
		optionsMenu.loadOptions(options);
	}
		
		

	/// <summary>
	/// Cleans the out options box - destroys all children buttons that could be selected
	/// </summary>
	public void cleanOutOptions()
	{
		foreach (Transform child in battlePanel.transform)
		{
			GameObject.Destroy (child.gameObject);
		}

		Destroy (battlePanel.GetComponent<Menu> ());
		Destroy (battlePanel.GetComponent<WaitingForTime> ());


	}
}


