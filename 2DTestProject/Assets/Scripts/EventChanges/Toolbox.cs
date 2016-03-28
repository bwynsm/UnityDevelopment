using System;
using UnityEngine;


public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
 


	public GameObject playerCharacter;
	public Vector2 positionInLastScene;
	public Vector2 battlePosition;

	public GameObject enemyDefeated;
 
	public Language language = new Language();
 
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

	void OnLevelWasLoaded(int level)
	{
		// what if we need to destroy a character? let's do that here

		// make sure our camera follow is set up to our main camera
		Camera.main.GetComponent<CameraFollow>().target = playerCharacter.transform;

		if (level != 1)
		{
			playerCharacter.transform.position = positionInLastScene;
			playerCharacter.GetComponent<PlayerMovement>().freeze = false;
		}

		if (enemyDefeated != null)
		{
			Destroy (enemyDefeated);
			enemyDefeated = null;
		}
	}
}
 
[System.Serializable]
public class Language {
	public string current;
	public string lastLang;
}
