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
		

	public void getBoxes()
	{
		
		if (battleOptionsManager.hasNextItem ())
		{
			battleOptionsManager.incrementIndex ();
			Speech nextText = battleOptionsManager.getItem ();
			Debug.Log ("GET BOXES A ");

			// loop over options and display
			showBattleMenu (nextText.options);




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
		Debug.Log ("we are battlemenu showbattlemenu with 3 and a count of : " + options.Count);

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


