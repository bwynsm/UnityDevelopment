using System;
using UnityEngine;


public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
 


	public GameObject playerCharacter;
	public Vector2 positionInLastScene;
	public Vector2 battlePosition;

	public string enemyDefeated;
 
	public Language language = new Language();
	public bool sceneAlreadyLoaded = false;


	public bool isLocked = false;
 
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake () 
	{
		// Your initialization code here
		// should rarely have to awaken here..
		playerCharacter = GameObject.FindGameObjectWithTag("PlayerCharacter");
	}
 

	// (optional) allow runtime registration of global objects
	static public T RegisterComponent<T> () where T: Component {
		return Instance.GetOrAddComponent<T>();
	}



	/// <summary>
	/// Raises the level was loaded event. If a level was loaded, see if we need to
	/// destroy any units or if we need to freeze or move units or camera objects
	/// </summary>
	/// <param name="level">Level.</param>
	void OnLevelWasLoaded(int level)
	{
		if (positionInLastScene == null)
		{
			// find the starting point
			positionInLastScene = GameObject.FindGameObjectWithTag("Respawn").transform.position;
		}

		if (level != 1)
		{
			// make sure our camera follow is set up to our main camera
			Camera.main.GetComponent<CameraFollow> ().target = playerCharacter.transform;

			playerCharacter.transform.position = positionInLastScene;
			playerCharacter.GetComponent<PlayerUnit> ().freeze = false;
			GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<Animator> ().SetBool ("IsFighting", false);
			Destroy(playerCharacter.GetComponent<DamageNumbers>());

			foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
			{
				player.SetActive (false);

				// set the other characters as not showing
				// remove components
				Destroy(player.GetComponent<DamageNumbers>());

			}
		} 
		else
		{
			// for each enemy and player type tag?
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				enemy.GetComponent<Animator> ().SetBool ("IsFighting", true);
			}

			foreach (GameObject player in GameObject.FindGameObjectsWithTag("PlayerCharacter"))
			{
				player.GetComponent<Animator> ().SetBool ("IsFighting", true);

				// set the other characters as not showing

			}
				
		}




		// if an enemy was defeated, destroy it from the scene.
		// we'll have to make this persist later.
		if (enemyDefeated != null)
		{
			Destroy (GameObject.Find(enemyDefeated));
			enemyDefeated = null;
		}


		// this has to happen at the end here so that we don't get into multiple fights ideally
		sceneAlreadyLoaded = true;
	}
}
 
[System.Serializable]
public class Language {
	public string current;
	public string lastLang;
}
