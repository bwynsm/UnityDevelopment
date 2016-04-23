using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;



/// <summary>
/// Load battle scene from battle managers first phase of battle
/// Gets the player turns and sets them in their positions and starting animations
/// </summary>
public class GameBoyLoadGame : MonoBehaviour 
{

	public GameObject prefabSlider;
	public TextAsset tempPlayerXML;

	public List<GameBoyUnit> turnOrder;
	private List<GameObject> allCombatants;

	public GameObject gameboyCharacter;
	public GameObject gameboyEnemy;

	private GameObject currentPlayer;
	private GameObject currentEnemy;

	public GameObject healthBarLostTick;
	public GameObject healthBarTick;


	// Use this for initialization
	void Start () 
	{
		//batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
		turnOrder = new List<GameBoyUnit> ();
	}


	public void LoadCharacters()
	{
		// FIRST ADD OUR CONTESTANTS
		// we need to have a list of possible fighter prefabs loaded
		// then we go through each of those and simply load one
		Instantiate(gameboyCharacter, new Vector3(-4.4f, -1.5f, 0), Quaternion.identity);
		Instantiate(gameboyEnemy, new Vector3(-3.0f, -1.5f, 0), Quaternion.identity);


	}

	public void LoadGameBoyTurns()
	{
		// get our player character and our enemy character
		currentPlayer = GameObject.FindGameObjectWithTag("GameBoyUnit");
		currentEnemy = GameObject.FindGameObjectWithTag ("Enemy");
		GameBoyUnit ourPlayerUnit = currentPlayer.GetComponent<GameBoyUnit> ();
		GameBoyUnit ourEnemyUnit = currentEnemy.GetComponent<GameBoyUnit> ();

		ourPlayerUnit.Flip ();

		ourPlayerUnit.targetUnit = ourEnemyUnit;
		ourEnemyUnit.targetUnit = ourPlayerUnit;
		ourPlayerUnit.healthLeft = GameObject.Find("LeftHealthText").GetComponent<Text>();
		ourEnemyUnit.healthLeft = GameObject.Find ("RightHealthText").GetComponent<Text>();
		ourPlayerUnit.healthBarTick = healthBarTick;
		ourEnemyUnit.healthBarTick = healthBarTick;


		ourPlayerUnit.CreateHealth ();
		ourEnemyUnit.CreateHealth ();



		ourEnemyUnit.healthBarLostTick = healthBarLostTick;
		ourPlayerUnit.healthBarLostTick = healthBarLostTick;

		// characterconversable[]
		allCombatants = new List<GameObject> ();


		allCombatants.Add (currentPlayer);
		allCombatants.Add (currentEnemy);
		turnOrder.Add (ourPlayerUnit);
		turnOrder.Add (ourEnemyUnit);


		// we need to be able to loop over and instantiate prefabs.


	}


	/// <summary>
	/// Loads the battle scene items.
	/// </summary>
	public void LoadBattleSceneItems()
	{
		Toolbox.Instance.sceneAlreadyLoaded = false;

	}







}

