using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LoadBattleScene : MonoBehaviour 
{

	public GameObject prefabSlider;
	public Canvas gameCanvas;
	public GameObject prefabButton;
	public TextAsset tempPlayerXML;


	public CharacterConversable[] turnOrder;
	// Use this for initialization
	void Start () 
	{

	}

	/// <summary>
	/// Loads the battle scene items.
	/// </summary>
	public void LoadBattleSceneItems()
	{
		Toolbox.Instance.sceneAlreadyLoaded = false;
		Debug.Log ("LOADING....");


		// get our player character and our enemy character
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("PlayerCharacter");
		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");

		// temporarily place them in predefined spots
		currentPlayer.transform.position = new Vector2(1.8f, -3.40f); 
		enemy.transform.position = new Vector2(5.2f, -3.40f); 

		// let's also make them face one another
		Animator anim = currentPlayer.GetComponent<Animator>();
		currentPlayer.GetComponent<PlayerMovement>().freeze = false;
		anim.SetBool ("isWalking", true);
		anim.SetFloat ("input_x", 1f);
		anim.SetFloat ("input_y", 0);



		// make enemy face hero side
		enemy.GetComponent<EnemyCharacter> ().freeze = false;
		anim = enemy.GetComponent<Animator> ();
		anim.SetBool ("isWalking", true);
		anim.SetFloat ("input_x", -1f);
		anim.SetFloat ("input_y", 0);

		enemy.GetComponent<EnemyCharacter> ().freeze = true;

			

		// freeze the hero
		currentPlayer.GetComponent<PlayerMovement>().freeze = true;
		currentPlayer.GetComponent<PlayerHealth> ().healthField = GameObject.Find ("HealthStats").GetComponent<Text>();



		// add in attack sequence to all enemies and give them their damage
		// activate attack
		enemy.GetComponent<EnemyAttack>().isActive = true;


		EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

		// give all players their health bar
		currentPlayer.GetComponent<PlayerHealth>().healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
		enemyHealth.healthSlider = GameObject.Find ("EnemyHealth").GetComponent<Slider> ();
		enemyHealth.healthField = GameObject.Find ("EnemyHealthStats").GetComponent<Text>();
		enemyHealth.healthSlider.maxValue = enemyHealth.maxHealth;
		enemyHealth.healthSlider.minValue = 0;
		enemyHealth.healthSlider.value = enemyHealth.currentHealth;
		enemyHealth.healthField.text = "<color=yellow>" + enemyHealth.currentHealth + "</color> / <color=white>" + enemyHealth.maxHealth + "</color>";



		currentPlayer.AddComponent<BattleMenu> ().battleXML = tempPlayerXML;
		BattleMenu battleMenu = currentPlayer.GetComponent<BattleMenu> ();
		battleMenu.battlePanel = GameObject.Find ("BattlePanel");
		battleMenu.prefabButton = prefabButton;



		// decide turn order
		turnOrder = FindObjectsOfType(typeof(CharacterConversable)) as CharacterConversable[];

		// get their respective speeds eventually. For now, we'll just hard code
		Array.Sort(turnOrder);

		// print out sort order
		foreach (var character in turnOrder)
		{
			// set every character to 
			character.GetComponent<Animator>().SetBool("IsFighting", true);


			Debug.Log ("CHARACTER NAME : " + character.name + " " + character.playerName + " AND THEIR SPEED : " + character.speed);
		}
	}


}
