using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;



/// <summary>
/// Load battle scene from battle managers first phase of battle
/// Gets the player turns and sets them in their positions and starting animations
/// </summary>
public class LoadBattleScene : MonoBehaviour 
{

	public GameObject prefabSlider;
	public Canvas gameCanvasu;
	public GameObject prefabButton;
	public TextAsset tempPlayerXML;
	public Texture2D border;



	public List<CharacterConversable> turnOrder;
	private List<GameObject> allCombatants;

	private GameObject currentPlayer;
	private GameObject currentEnemy;

	BattleManager batMan;

	// Use this for initialization
	void Start () 
	{
		batMan = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<BattleManager> ();
	}



	/// <summary>
	/// Loads the turns for the players in combat
	/// </summary>
	public void LoadTurns()
	{
		
		// get our player character and our enemy character
		currentPlayer = GameObject.FindGameObjectWithTag("PlayerCharacter");
		currentEnemy = GameObject.FindGameObjectWithTag ("Enemy");
		currentPlayer.transform.position = new Vector2(1.8f, -3.40f);
		currentEnemy.transform.position = new Vector2 (5.2f, -3.40f);

		// characterconversable[]
		CharacterConversable[] enemyChildrenTemp = currentEnemy.GetComponentsInChildren<CharacterConversable> (true);
		CharacterConversable[] playerChildrenTemp = currentPlayer.GetComponentsInChildren<CharacterConversable> (true);
		allCombatants = new List<GameObject> ();

		Debug.Log (" ARRAY LENGTHS : " + enemyChildrenTemp.Length + " " + playerChildrenTemp.Length);

		// add each of the children here
		for (var i = 0; i < 3 ; i++)
		{
			// if we have another enemy child - this we can re-do later when we randomize enemies
			// probably from prefabs
			if (i < enemyChildrenTemp.Length)
			{
				allCombatants.Add (enemyChildrenTemp [i].gameObject);
				enemyChildrenTemp [i].gameObject.SetActive (true);
				batMan.enemies.Add (enemyChildrenTemp [i].gameObject.GetComponent<EnemyUnit> ());
			}

			// if we have another player child in our player party
			if (i < playerChildrenTemp.Length)
			{
				allCombatants.Add (playerChildrenTemp [i].gameObject);
				playerChildrenTemp [i].gameObject.SetActive (true);
				batMan.teammates.Add (playerChildrenTemp [i].gameObject.GetComponent<PlayerUnit> ());
			}
		}


	}

	/// <summary>
	/// Loads the battle scene items.
	/// </summary>
	public void LoadBattleSceneItems()
	{
		Toolbox.Instance.sceneAlreadyLoaded = false;
		Debug.Log ("LOADING....");


		// get our first two characters and their children.


		// let's sort by speed - reverse order so that we can have the highest speeds go first
		allCombatants.Sort((GameObject x, GameObject y) =>  y.GetComponent<CharacterConversable>().speed.CompareTo(x.GetComponent<CharacterConversable>().speed));


		// now that we have all combatants...
		// loop over the objects?
		// we can tell what they are by their tags
		float playerIndex = 0.8f;
		float enemyIndex = 0.0f;

		foreach (var combatant in allCombatants)
		{
			CharacterConversable combatantCharacter = combatant.GetComponent<CharacterConversable> ();

			Debug.Log ("COMBATANT NAME : " + combatant.name + " AND THEIR SPEED : " + combatantCharacter.speed);
			turnOrder.Add (combatant.GetComponent<CharacterConversable> ());

			// IF THE UNIT IS A PLAYER CHARACTER, PUT IT ON THE PLAYER'S SIDE
			if (combatant.GetComponent<CharacterConversable>().isPlayerCharacter)
			{

				Debug.Log ("COMBATANT " + combatant.name + " BEFORE TRANSFORM : " + combatant.transform.position);

				if (!combatant.Equals (currentPlayer))
				{
					// find positions for everyone.
					combatant.transform.position = new Vector2 (1.8f, -3.40f + (playerIndex)); 
					playerIndex += 0.8f;
				}

				Debug.Log ("COMBATANT " + combatant.name + " AFTER TRANSFORM : " + combatant.transform.position);

				// let's also make them face one another
				Animator anim = combatant.GetComponent<Animator>();


				// set the players to face each other. 
				PlayerUnit combatantUnit = combatant.GetComponent<PlayerUnit>();
				combatantUnit.GetComponent<PlayerUnit>().freeze = false;
				anim.SetBool ("isWalking", true);
				anim.SetFloat ("input_x", 1f);
				anim.SetFloat ("input_y", 0);
				combatantUnit.GetComponent<PlayerUnit> ().freeze = true;

				// give all players their health bar
				PlayerHealth health = combatantUnit.GetComponent<PlayerHealth> ();


				//if (combatant.playerName == "Princess")
				//{
					health.healthField = GameObject.Find ("HealthStats").GetComponent<Text> ();
					health.healthSlider = GameObject.Find ("PlayerHealth").GetComponent<Slider> ();
				//}

				combatant.AddComponent<BattleMenu> ().battleXML = tempPlayerXML;
				BattleMenu battleMenu = combatant.GetComponent<BattleMenu> ();
				battleMenu.allCombatants = turnOrder;
				battleMenu.battlePanel = GameObject.Find ("BattlePanel");

			}

			// otherwise, we're an enemy character - but let's check and see if we have an enemy unit
			// component anyway
			else
			{
				

				if (!combatant.Equals(currentEnemy))
				{
					enemyIndex += 0.8f;
					// find positions for everyone.
					combatant.transform.position = new Vector2 (5.2f, -3.40f + (enemyIndex)); 
					Debug.Log ("ENEMY NAME : " + combatant.name + " POSITION : " + combatant.transform.position);
				}


				// let's also make them face one another
				Animator anim = combatant.GetComponent<Animator>();
				EnemyUnit combatantUnit = combatant.GetComponent<EnemyUnit>();

				// make enemy face hero side
				combatantUnit.freeze = false;
				anim = combatant.GetComponent<Animator> ();
				anim.SetBool ("isWalking", true);
				anim.SetFloat ("input_x", -1f);
				anim.SetFloat ("input_y", 0);
				combatantUnit.freeze = true;


				// add in attack sequence to all enemies and give them their damage
				// activate attack
				combatantUnit.enemyAttack.isActive = true;

				// just the grue gets it's own health bar at the moment until we figure that out.
				EnemyHealth enemyHealth = combatantUnit.enemyHealth;
				enemyHealth.healthSlider = GameObject.Find ("EnemyHealth").GetComponent<Slider> ();
				enemyHealth.healthField = GameObject.Find ("EnemyHealthStats").GetComponent<Text> ();
				enemyHealth.healthSlider.maxValue = enemyHealth.maxHealth;
				enemyHealth.healthSlider.minValue = 0;
				enemyHealth.healthSlider.value = enemyHealth.currentHealth;
				enemyHealth.healthField.text = "<color=yellow>" + enemyHealth.currentHealth + "</color> / <color=white>" + enemyHealth.maxHealth + "</color>";
			}



		}


	}







}
