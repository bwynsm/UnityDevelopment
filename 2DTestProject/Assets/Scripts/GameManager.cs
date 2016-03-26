using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{



	public int currentHealth = 100;
	public int maxHealth = 100;
	public int level = 3;
	public int experience = 500;
	public GameObject playerCharacter;
	public Vector2 positionInLastScene;


	void Awake()
	{
		// if we have a last location, set it. Otherwise, set new
		if (positionInLastScene == null || positionInLastScene == Vector2.zero) {
			//playerCharacter.transform.position = new Vector2 (-3.8f, -12.0f);
		} 
		else 
		{
			playerCharacter.transform.position = positionInLastScene;
		}
			
	}

}
