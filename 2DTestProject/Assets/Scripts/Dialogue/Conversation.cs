using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;



public class Conversation 
{
	public int numberOfConversations = 0;
	//TextAsset textFile;


	// how are we going to provide options? We're going to have a current "Character Say" tag, and take that in stages.
	// everything is in xml - brilliant ben - 
	// if just basic text, just display it.
	// if choice, walk through the choice options
	// if some sort of command, do something
	List<Speech> dialogue;


	int index = -1;



	// just keep track of our reader.
	XmlReader reader;


	public string optionsID = "";

	Dialogue conversationItem;


	/**
	 * Conversation Constructor
	 * 
	 * Takes in a file to read
	 * Sets up a Dialogue so that we can read an XML file
	 */
	public Conversation(TextAsset textFileToRead, string conversationID)
	{

		//dialogue = Dialogue.GetData (textFile);
		index = -1;

		dialogue = new List<Speech>();

		//createDialogue (textFileToRead, conversationID);

		conversationItem = new Dialogue (textFileToRead, conversationID);

		dialogue = conversationItem.getDialogue (conversationID);

		// return dialogues

	}



	/// <summary>
	/// Changes the dialogue id and loads the new conversation based off of that
	/// which also resets the index
	/// </summary>
	/// <param name="newConversationID">New conversation I.</param>
	public void changeDialogue(string newConversationID)
	{
		conversationItem.setConversationID (newConversationID);

		Debug.Log ("changing dialogue in conversation" + newConversationID);
		dialogue = conversationItem.getDialogue (newConversationID);
		index = -1;
	}

	/**
	 * getItem returns our Speeches item from where we are at in our list in Dialogue
	 * At some point this will become more complex perhaps, as we develop more options
	 * but I don't know if that is in this scope
	 */
	public Speech getItem()
	{
		// the get item that is next - we won't even have to do anything here
		// but get an item based on a number

		return dialogue[index];
	}


	/**
	 * incrementIndex - self explanatory - updates our index of where we are at
	 * in our dialogue xml
	 */
	public void incrementIndex ()
	{
		index++;
	}


	/**
	 * hasNextItem returns a boolean on whether we have another item left in our
	 * dialogue to read
	 */
	public bool hasNextItem()
	{
		if ((index + 1) < dialogue.Count)
		{
			return true;
		} 
		else
		{
			return false;
		}

	}
		

	/**
	 * get index simply returns our index
	 */
	public int getIndex()
	{
		return index;
	}




}






