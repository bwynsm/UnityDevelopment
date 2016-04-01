using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


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
	PlayerUnit playerAttacking;
	//EnemyUnit enemyUnderAttack;
	PlayerUnit unitBeingBuffed;

	/// <summary>
	/// For the moment, just a handle on an object
	/// </summary>
	public Commands()
	{
		
	}



	// for now, let's just resolve everything with a closure
	public void resolvePauseMenuCommands(Options optionItem)
	{
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PauseMenu> ().unpauseGame();
	}

	/// <summary>
	/// Resolves the commands sent from options in a conversation
	/// </summary>
	/// <param name="optionItem">Option item.</param>
	public void resolveBattleCommands(Options optionItem)
	{
		// we are looking at the command
		string playerToAlter 	= optionItem.playerToAlter;
		string currentPlayer 	= optionItem.currentPlayer;
		string commandsString	= optionItem.command;
		BattleManager batMan = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BattleManager>();

		bool finalSelection = false;
		// if player to alter is blank, debug that

		// next we need to take that command that we have and we need
		// to parse it out. We'll split it by semicolon
		string[] commandsList = commandsString.Split(';');



		// now that we have everything split up, we want to loop over that list
		foreach (var commandItem in commandsList)
		{
			// now we add that section from options
			// if we have a number, just go to that number
			if (commandItem.Contains ("id#"))
			{
				Debug.Log ("CONTAINS ID : " + commandItem);

				// split our command our and send it to our function
				string command = (commandItem.Split ('#')) [1];
				changeBattleDialogueID (command, playerToAlter);


			} 

			// otherwise, if we are doing an immediate branching, branch out immediately
			// to the other branch
			else if (commandItem.Contains ("goto#"))
			{
				// split our command our and send it to our function
				string command = (commandItem.Split ('#')) [1];
				branchBattle (command, playerToAlter);
			} 

			// unrecognized command
			else if (commandItem.Contains ("attack"))
			{
				Debug.Log ("we are attacking");

				if (playerAttacking == null)
					Debug.Log ("attacking player is null");
				else if (playerAttacking.GetComponent<PlayerAttack> () == null)
					Debug.Log ("PLAYERATTACK is null");

				playerAttacking.GetComponent<PlayerAttack> ().startAttacking = true;
				finalSelection = true;
			} 

			// for SPELL attacks
			else if (commandItem.Contains ("ice") || commandItem.Contains ("fire") || commandItem.Contains ("water"))
			{
				playerAttacking.GetComponent<PlayerAttack>().castOffensiveSpell (commandItem);
				finalSelection = true;
			} 

			// for damage like in items 
			else if (commandItem.Contains ("damage#"))
			{
				// get the number of damage
				playerAttacking.GetComponent<PlayerAttack>().dealDamage (commandItem);
				finalSelection = true;
			}

			// for healing (or potentially other buffs)
			else if (commandItem.Contains ("health#"))
			{
				// get the number of damage

				playerAttacking.GetComponent<PlayerAttack>().buffPlayer (commandItem,unitBeingBuffed);
				finalSelection = true;
			}

			// otherwise, we have something we don't recognize
			else
			{
				Debug.Log ("We have an unrecognized command : " + commandItem + " " + currentPlayer);
			}
		}
			
		playerAttacking.GetComponent<BattleMenu> ().updatingItems = true;


		// no longer our turn
		if (finalSelection)
		{
			playerAttacking.GetComponent<BattleMenu> ().isMyTurn = false;
			batMan.turnFinished = true;
		}


	}


	/// <summary>
	/// Sets the attacking player
	/// </summary>
	/// <param name="attacker">Attacker.</param>
	public void setAttackingPlayer(PlayerUnit attacker)
	{
		playerAttacking = attacker;
	}


	/// <summary>
	/// Sets the enemy under attack.
	/// </summary>
	/// <param name="enemyUnderAttack">Enemy under attack.</param>
	public void setEnemyUnderAttack(EnemyUnit attacked)
	{
		//enemyUnderAttack = attacked;
		playerAttacking.GetComponent<PlayerAttack> ().enemyUnit = attacked;
		playerAttacking.GetComponent<PlayerAttack> ().target = attacked.gameObject;
	}



	/// <summary>
	/// Sets the player being buffed.
	/// </summary>
	/// <param name="playerUnit">Player unit.</param>
	public void setPlayerBeingBuffed(PlayerUnit playerUnit)
	{
		unitBeingBuffed = playerUnit;
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


		// if player to alter is blank, debug that
		Debug.Log("Player to alter : " + currentPlayer);

		// next we need to take that command that we have and we need
		// to parse it out. We'll split it by semicolon
		string[] commandsList = commandsString.Split(';');



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
				Debug.Log ("We have an unrecognized command : " + commandItem + " " + currentPlayer);
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
	/// Changes the dialogue ID for the next time we talk to this character
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="playerToAlter">Player to alter.</param>
	private void changeBattleDialogueID(string command, string playerToAlter)
	{
		Debug.Log ("COMMAND : " + command + " Player To Alter : " + playerToAlter);
		// change conversation id to that?
		// get player by tag name
		CharacterConversable playerObject = GameObject.Find (playerToAlter).GetComponent<CharacterConversable> ();
		Debug.Log ("we are getting our new dialogue");
		playerObject.GetComponent<BattleMenu> ().battleOptionsManager.changeDialogue(command);


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
		Debug.Log ("command" + command + " Player to alter : " + playerToAlter);
		playerObject.GetComponent<ActivateTextAtLine> ().activateNewText(command);
	}

	/// <summary>
	/// Branches the battle to a new XML branch from the activatetextatline object
	/// </summary>
	/// <param name="command">Command.</param>
	/// <param name="playerToAlter">Player to alter.</param>
	private void branchBattle(string command, string playerToAlter)
	{
		// first we'll get the same thing as last line does, except that we'll reload
		// change conversation id to that?
		// get player by tag name
		CharacterConversable playerObject = GameObject.Find (playerToAlter).GetComponent<CharacterConversable> ();
		playerObject.GetComponent<BattleMenu> ().battleOptionsManager.changeDialogue (command);
	}





}
