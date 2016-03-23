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



	public IEnumerator PauseBeforeInput()
	{
		Debug.Log ("paused for input");
		yield return new WaitForSeconds (0.15f);
		Debug.Log ("unpaused for input");
	}

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
