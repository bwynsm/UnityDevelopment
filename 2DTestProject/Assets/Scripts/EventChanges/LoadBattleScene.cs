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


	public void LoadBattleSceneItems()
	{
		Toolbox.Instance.sceneAlreadyLoaded = false;
		Debug.Log ("LOADING....");

		// get our player character
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("PlayerCharacter");
		GameObject enemy = GameObject.FindGameObjectWithTag ("CombatZone");
		currentPlayer.transform.position = new Vector2(1.8f, -3.40f); 
		enemy.transform.position = new Vector2(5.2f, -3.40f); 

		// let's also make them face one another
		Animator anim = currentPlayer.GetComponent<Animator>();
		currentPlayer.GetComponent<PlayerMovement>().freeze = false;
		anim.SetBool ("isWalking", true);
		anim.SetFloat ("input_x", 1f);
		anim.SetFloat ("input_y", 0);

		Debug.Log ("ENEMY NAME : " + enemy.name);

		if (enemy.GetComponent<EnemyCharacter> ())
		{

			enemy.GetComponent<EnemyCharacter> ().freeze = false;
			anim = enemy.GetComponent<Animator> ();
			anim.SetBool ("isWalking", true);
			anim.SetFloat ("input_x", -1f);
			anim.SetFloat ("input_y", 0);

			enemy.GetComponent<EnemyCharacter> ().freeze = true;
			Debug.Log ("FREEZING ENEMY");
		}
			

		currentPlayer.GetComponent<PlayerMovement>().freeze = true;
		currentPlayer.GetComponent<PlayerHealth> ().healthField = GameObject.Find ("HealthStats").GetComponent<Text>();
		//Camera.main.GetComponent<CameraFollow> ().target = currentPlayer.transform;
		//Camera.main.transform.position = currentPlayer.transform.position;


		// add in attack sequence to all enemies and give them their damage
		enemy.AddComponent<EnemyAttack>();


		EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

		// give all players their health bar
		currentPlayer.GetComponent<PlayerHealth>().healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
		Debug.Log ("ENEMY HEALTH? : " + GameObject.Find ("EnemyHealth").GetComponent<Slider> ().name);
		Debug.Log ("Enemy Health ???? : " + enemyHealth.name);
		enemyHealth.healthSlider = GameObject.Find ("EnemyHealth").GetComponent<Slider> ();
		enemyHealth.healthField = GameObject.Find ("EnemyHealthStats").GetComponent<Text>();
		enemyHealth.healthSlider.maxValue = enemyHealth.startingHealth;
		enemyHealth.healthSlider.minValue = 0;
		enemyHealth.healthSlider.value = enemyHealth.currentHealth;
		enemyHealth.healthField.text = "<color=yellow>" + enemyHealth.currentHealth + "</color> / <color=white>" + enemyHealth.startingHealth + "</color>";
		enemy.tag = "Enemy";


		// add a couple of images?
		// add our prefab button and set it to whatever
		//GameObject slider = (GameObject)Instantiate (prefabSlider);

		// set parent. 

		//slider.transform.SetParent (gameCanvas.transform, false);
		//slider.transform.localScale =  new Vector3(1, 1, 1);

		//Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, enemy.transform.position);
		//Debug.Log ("SCREEN POINT : " + screenPoint + "  SCREEN POINT MATH : " + gameCanvas.GetComponent<RectTransform> ().sizeDelta + " = " + (screenPoint - gameCanvas.GetComponent<RectTransform> ().sizeDelta / 2f));
		//slider.GetComponent<RectTransform> ().anchoredPosition = screenPoint - new Vector2(350, -150) -  gameCanvas.GetComponent<RectTransform> ().sizeDelta / 2f;

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
			Debug.Log ("CHARACTER NAME : " + character.name + " " + character.playerName + " AND THEIR SPEED : " + character.speed);
		}
	}


}
