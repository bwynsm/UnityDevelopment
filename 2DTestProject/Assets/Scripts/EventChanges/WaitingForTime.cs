using UnityEngine;
using System.Collections;

public class WaitingForTime : MonoBehaviour 
{

	public bool isPaused = true;

	public WaitingForTime()
	{
	}

	// Use this for initialization
	void Start () 
	{
		isPaused = true;
	}



	/// <summary>
	/// Pauses the script before we try and receive input to prevent
	/// too quickly proceeding past something without seeing what it was
	/// in the first place
	/// </summary>
	/// <returns>The before input.</returns>
	public IEnumerator PauseBeforeInput()
	{
		Debug.Log ("paused for input");
		yield return new WaitForSeconds (0.15f);
		Debug.Log ("unpaused for input");
	}

	/// <summary>
	/// Waits for key down - any key
	/// </summary>
	/// <returns>The for key down.</returns>
	public IEnumerator WaitForKeyDown()
	{

		// wait half a second
		while (!Input.anyKey) 
		{
			yield return null;
		}

		isPaused = false;

	}
}
