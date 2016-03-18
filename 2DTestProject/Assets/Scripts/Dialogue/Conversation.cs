using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;



public class Conversation 
{
	public int numberOfConversations = 0;
	TextAsset textFile;


	// how are we going to provide options? We're going to have a current "Character Say" tag, and take that in stages.
	// everything is in xml - brilliant ben - 
	// if just basic text, just display it.
	// if choice, walk through the choice options
	// if some sort of command, do something
	Dialogue dialogue;

	int index = 0;



	/**
	 * Conversation Constructor
	 * 
	 * Takes in a file to read
	 * Sets up a Dialogue so that we can read an XML file
	 */
	public Conversation(TextAsset textFileToRead)
	{
		textFile = textFileToRead;

		dialogue = Dialogue.GetData (textFile);
		index = 0;

	}



	/**
	 * getDialogue simply returns our Dialogue object - not sure if we'll need this or not
	 */
	public Dialogue getDialogue()
	{
		return dialogue;
	}



	/**
	 * getItem returns our Speeches item from where we are at in our list in Dialogue
	 * At some point this will become more complex perhaps, as we develop more options
	 * but I don't know if that is in this scope
	 */
	public Speech getItem()
	{
		return dialogue.Speeches [index];
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
		
		if ((index + 1) < dialogue.Speeches.Length)
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

	// let's just go line to line at the moment. We don't even need anything else
	// player name
	// file
	// current line
	// end dialog (do we want to offer a button for running away from conversation?)

	// Mostly, everything here is done in secret. If we hit a collider, then we want to
	// activate a whole process to start and finish a conversation almost as simply as
	// "Start Conversation" 
	// and then updating the speech bubble if we meet parameters on the other end.
	// We are going to let the character decide if those items are met by determining
	// buttons and states etc. (this doesn't know anything about anything but the
	// conversation and trying to display the conversation bubble - which is player
	// locations and who is talking and the xml and how to parse

	// functions: (public)
	//// update display bubble (this is basically a read line - but we might have other options)
	//// 
	//// - show display bubble
	//// - hide display bubble
	//// - parse xml
	//// - display options (this is down the road - I want to display a different type of bubble
	//// if there are going to be text options in the text - character needs to decide something
	//// - get person talking
	//// - get current line
	//// - is finished with conversation? Because if we've already had this and only want to have
	//// this once, perhaps we read a different section? There can also be routes to a conversation
	//// that we can dive into here.... but how do we keep track of that? I think simply a sign
	//// that we are done with a conversation and won't have it again, and then also perhaps something
	//// to prevent the trigger from happening again? Destroy when activated?



}






