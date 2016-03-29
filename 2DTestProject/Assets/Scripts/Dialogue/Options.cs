using UnityEngine;
using System.Collections;

public class Options {

	public string command;
	public string option;
	public string playerToAlter;
	public string currentPlayer;



	/// <summary>
	/// Initializes a new instance of the <see cref="Options"/> class.
	/// </summary>
	public Options ()
	{
	}


	public Options(string commandString, string optionString, string playerToAlterString, string currentPlayerString)
	{
		command = commandString;
		option = optionString;
		playerToAlter = playerToAlterString;
		currentPlayer = currentPlayerString;
	}

	/// <summary>
	/// Returns a string list of this item that is just easier to read for debugging
	/// purposes if necessary
	/// </summary>
	/// <returns>The string.</returns>
	public string toString()
	{
		return "Option : " + option + " - Command : " + command + " - Player To Alter : " + playerToAlter + " - Current Player : " + currentPlayer;
	}
}
