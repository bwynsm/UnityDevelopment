using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


/// <summary>
/// Sting changer
/// </summary>
public class StingPlayer : MonoBehaviour
{

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		DontDestroyOnLoad (gameObject);
	}


	// for this, we are just keeping track of a whole list of stings
	// and we'll play the particular sting we want - we'll be able to have a list of 
	// functions
	public AudioClip[] stings;
	public AudioSource stingSource;



	/// <summary>
	/// Plays the menu movement down sound.
	/// </summary>
	public void playMenuDownSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 0)
		{
			stingSource.PlayOneShot (stings[0]);
		}
	}


	/// <summary>
	/// Plays the menu up sound.
	/// </summary>
	public void playMenuUpSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 0)
		{
			stingSource.PlayOneShot (stings[0]);
		}
	}



	/// <summary>
	/// Plays the new game sound.
	/// </summary>
	public void playNewGameSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 1)
		{
			stingSource.PlayOneShot (stings[1]);
		}
	}


	/// <summary>
	/// Plays the load game sound.
	/// </summary>
	public void playLoadGameSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 1)
		{
			stingSource.PlayOneShot (stings[1]);
		}
	}


	/// <summary>
	/// Plays the select item sound.
	/// </summary>
	public void playSelectItemSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 2)
		{
			stingSource.PlayOneShot (stings[2]);
		}
	}



	/// <summary>
	/// Plays the back button sound.
	/// </summary>
	public void playBackButtonSound()
	{
		if (stings.Length > 3)
		{
			stingSource.PlayOneShot (stings [3]);
		}
	}


	/// <summary>
	/// Plays the message sound.
	/// </summary>
	public void playMessageSound()
	{
		// play our first sting which is where we'll store this item
		if (stings.Length > 4)
		{
			stingSource.PlayOneShot (stings[4]);
		}
	}

}