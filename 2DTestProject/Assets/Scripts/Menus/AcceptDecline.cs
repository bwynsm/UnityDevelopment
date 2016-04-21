using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AcceptDecline : Menu 
{

	public string MiniGame; // this is the mini game we'll play

	//private bool isDeciding = false;
	public bool hasDecided = false;

	// Use this for initialization
	new void Start () 
	{
		menuOptions = new List<Options> () 
		{
			new Options("gameboy", "Play", "", "Player", ""),
			new Options("decline", "Not now", "", "Player", "")
		};
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if we are not on screen, ignore this object
		if (!GetComponent<Renderer> ().isVisible)
		{
			return;
		}

		/// if we've made our decision, destroy this
		if (hasDecided)
		{
			Destroy (this);
		}

	}






}
