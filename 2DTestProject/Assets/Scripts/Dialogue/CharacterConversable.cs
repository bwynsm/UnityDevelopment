using UnityEngine;
using System.Collections;
using System;

/**
 * CharacterConversable
 * This actually isn't an interface.. I thought about it. I probably could still
 * make this an interface and just have everyone do it. 
 * But this determines whether a character can converse or be conversed with
 */
public class CharacterConversable : MonoBehaviour, IComparable
{

	// if frozen
	public bool freeze = false;
	public bool isTalking = false;
	public string playerName;
	public int speed;

	public bool isPlayerCharacter;

	// character has a transform - where is the character
	public Transform whereAmI()
	{
		return this.GetComponent<Transform> ();
	}


	/// <summary>
	/// Returns if character is frozen
	/// </summary>
	/// <returns><c>true</c>, if frozen, <c>false</c> otherwise.</returns>
	// character returns if frozen when talking
	public bool isFrozen()
	{
		return freeze;
	}



	/// <summary>
	/// Compares the current instance with another object of the same type and returns an integer that indicates whether
	/// the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="obj">Object.</param>
	public int CompareTo(object obj)
	{
		if (obj == null)
			return 1;

		CharacterConversable temp = (CharacterConversable)obj;

		if (this.speed < temp.speed)
			return 1;
		else if (this.speed > temp.speed)
			return -1;
		else
			return 0;
		
	}

}
