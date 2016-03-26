using UnityEngine;
using System.Collections;

public class LoadBattleScene : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		// get our manager

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
		enemy.GetComponent<EnemyCharacter> ().freeze = true;

		Camera.main.GetComponent<CameraFollow> ().target = currentPlayer.transform;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
