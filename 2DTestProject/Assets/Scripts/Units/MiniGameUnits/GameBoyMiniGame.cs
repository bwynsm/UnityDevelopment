using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoyMiniGame : Menu 
{

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
		
	}



	// if colliding, allow to press button
	// if rendered.

}
