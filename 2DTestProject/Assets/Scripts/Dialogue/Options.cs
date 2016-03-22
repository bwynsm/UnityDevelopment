using UnityEngine;
using System.Collections;

public class Options {

	public string command;
	public string option;
	public string playerToAlter;
	public string currentPlayer;

	public Options ()
	{
	}


	public string toString()
	{
		return "Option : " + option + " - Command : " + command + " - Player To Alter : " + playerToAlter + " - Current Player : " + currentPlayer;
	}
}
