using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/// <summary>
/// Load game. Very first initialization step. Right now, just loading player and starting scene
/// after setting player as an indestructable object
/// </summary>
public class LoadGame : MonoBehaviour {

	// this loads all of our prefabs that need to not be destroyed on load
	// for now, we'll just have our character in here
	public GameObject playerCharacter;

	// Use this for initialization
	void Start () 
	{
		GameObject player = Instantiate (playerCharacter);
		DontDestroyOnLoad (player);
		player.name = "Player";


		// now we'll just load our opening scene
		SceneManager.LoadScene("OpeningScene");
		Toolbox.Instance.playerCharacter = player;
	}



}
