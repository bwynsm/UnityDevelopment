using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Commands Class that works with resolving string commands
/// into actions
/// </summary>
public class Commands 
{

	// in commands, we are basically receiving a list of options
	// and we are just looping over them and activating our items.
	// we are adding and removing an object from a player
	// as necessary if we need to do so.
	// Mostly, we are taking in that options list and we are 
	// updating the playerToAlter with whatever the commands are
	// on that player to alter.
	// this won't be damage. That will be triggered elsewhere.


	/// <summary>
	/// For the moment, just a handle on an object
	/// </summary>
	public Commands()
	{
		
	}



	/// <summary>
	/// Resolves the commands sent from options in a conversation
	/// </summary>
	/// <param name="optionItem">Option item.</param>
	public void resolveConversationCommands(Options optionItem)
	{
		// we are looking at the command
		string playerToAlter 	= optionItem.playerToAlter;
		string currentPlayer 	= optionItem.currentPlayer;
		string commandsString	= optionItem.command;



		// next we need to take that command that we have and we need
		// to parse it out. We'll split it by semicolon
		string[] commandsList = commandsString.Split(';');

		Debug.Log ("SIZE OF SPLIT : " + commandsList.Length);


		// now that we have everything split up, we want to loop over that list
		foreach (var commandItem in commandsList)
		{
			// now we add that section from options
			// if we have a number, just go to that number
			if (commandItem.Contains ("id#"))
			{
				// split our command our and send it to our function
				string command = (commandItem.Split ('#')) [1];
				changeDialogueID (command, playerToAlter);
			} 

			// otherwise, if we are doing an immediate branching, branch out immediately
			// to the other branch
			else if (commandItem.Contains ("goto#"))
			{
				// split our command our and send it to our function
				string command = (commandItem.Split ('#')) [1];
				branchConversation (command, playerToAlter);
			} 

			// unrecognized command
			else
			{
				Debug.Log ("We have an unrecognized command : " + commandItem);
			}
		}




	}


	/// <summary>
	/// Changes the dialogue ID for the next time we talk to this character
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="playerToAlter">Player to alter.</param>
	private void changeDialogueID(string command, string playerToAlter)
	{
		// change conversation id to that?
		// get player by tag name
		CharacterConversable playerObject = GameObject.Find (playerToAlter).GetComponent<CharacterConversable> ();
		playerObject.GetComponent<ActivateTextAtLine> ().dialogueID = command;
	}



	/// <summary>
	/// Branchs the conversation to a new XML branch from the activatetextatline object
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="playerToAlter">Player to alter.</param>
	private void branchConversation(string command, string playerToAlter)
	{
		// first we'll get the same thing as last line does, except that we'll reload
		// change conversation id to that?
		// get player by tag name
		CharacterConversable playerObject = GameObject.Find (playerToAlter).GetComponent<CharacterConversable> ();
		playerObject.GetComponent<ActivateTextAtLine> ().activateNewText(command);
	}





}
