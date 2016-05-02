using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadWhichBattle : MonoBehaviour 
{
	public Canvas gameboyCanvas;
	public Canvas basicCanvas;


	// load the prefabs I'll need
	// load the xml
	public GameObject prefabSlider;
	public TextAsset tempPlayerXML;
	public GameObject gameboyCharacter;
	public GameObject gameboyEnemy;

	public Sprite healthBarLostTick;
	public Sprite healthBarTick;



	// Use this for initialization
	void Awake () 
	{
	
		Toolbox toolboxInstance = Toolbox.Instance;

		// game boy battle scene
		if (toolboxInstance.battleScene.Equals ("GameBoy"))
		{
			// add the component for the gameboy battle scene
			GameBoyLoadGame gameBoy = gameObject.AddComponent<GameBoyLoadGame>();
			gameboyCanvas.enabled = true;
			basicCanvas.enabled = false;

			// set up some variables
			gameBoy.gameboyCharacter = gameboyCharacter;
			gameBoy.gameboyEnemy = gameboyEnemy;
			gameBoy.tempPlayerXML = tempPlayerXML;

			gameBoy.healthBarLostTick = healthBarLostTick;
			gameBoy.healthBarTick = healthBarTick;

			Camera.main.orthographicSize = 1;

			Debug.Log ("game boy battle scene");
		} 

		// basic battle scene
		else
		{
			// add the component for the basic scene
			gameObject.AddComponent<LoadBattleScene>();
			gameboyCanvas.enabled = false;
			basicCanvas.enabled = true;

			Camera.main.orthographicSize = 2;

			Debug.Log ("basic battle scene");
		}


	}

}
