using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadBattleScene : MonoBehaviour 
{

	public GameObject prefabSlider;
	public Canvas gameCanvas;
	public GameObject prefabButton;
	public TextAsset tempPlayerXML;

	// Use this for initialization
	void Start () 
	{

		Debug.Log ("LOADING....");

		// get our player character
		GameObject currentPlayer = GameObject.FindGameObjectWithTag("PlayerCharacter");
		GameObject enemy = GameObject.FindGameObjectWithTag ("CombatZone");
		currentPlayer.transform.position = new Vector2(259.355f, 455.776f); 
		enemy.transform.position = new Vector2(260.551f, 455.787f); 

		// let's also make them face one another
		Animator anim = currentPlayer.GetComponent<Animator>();
		currentPlayer.GetComponent<PlayerMovement>().freeze = false;
		anim.SetBool ("isWalking", true);
		anim.SetFloat ("input_x", 1f);
		anim.SetFloat ("input_y", 0);

		enemy.GetComponent<EnemyCharacter>().freeze = false;
		anim = enemy.GetComponent<Animator> ();
		anim.SetBool ("isWalking", true);
		anim.SetFloat ("input_x", -1f);
		anim.SetFloat ("input_y", 0);

		currentPlayer.GetComponent<PlayerMovement>().freeze = true;
		currentPlayer.GetComponent<PlayerHealth> ().healthField = GameObject.Find ("HealthStats").GetComponent<Text>();
		enemy.GetComponent<EnemyCharacter> ().freeze = true;

		Camera.main.GetComponent<CameraFollow> ().target = currentPlayer.transform;
		Camera.main.transform.position = currentPlayer.transform.position;


		// add in attack sequence to all enemies and give them their damage
		enemy.AddComponent<EnemyAttack>();

		// give all players their health bar
		currentPlayer.GetComponent<PlayerHealth>().healthSlider = GameObject.Find("PlayerHealth").GetComponent<Slider>();
		enemy.GetComponent<EnemyHealth> ().healthSlider = GameObject.Find ("EnemyHealth").GetComponent<Slider> ();
		enemy.GetComponent<EnemyHealth> ().healthField = GameObject.Find ("EnemyHealthStats").GetComponent<Text>();
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

	}

	
	// Update is called once per frame
	void Update () {
	
	}


}
