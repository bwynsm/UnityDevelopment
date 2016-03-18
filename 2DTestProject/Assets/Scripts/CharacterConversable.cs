using UnityEngine;
using System.Collections;


/**
 * CharacterConversable
 * This actually isn't an interface.. I thought about it. I probably could still
 * make this an interface and just have everyone do it. 
 * But this determines whether a character can converse or be conversed with
 */
public class CharacterConversable : MonoBehaviour 
{

	// if frozen
	public bool freeze = false;
	public bool isTalking = false;
	public string playerName;

	// character has a transform - where is the character
	public Transform whereAmI()
	{
		return this.GetComponent<Transform> ();
	}


	// character returns if frozen when talking
	public bool isFrozen()
	{
		return freeze;
	}
		

}
