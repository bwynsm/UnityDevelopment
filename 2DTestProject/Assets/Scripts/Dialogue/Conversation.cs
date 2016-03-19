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


	int index = 0;



	// just keep track of our reader.
	XmlReader reader;

	bool isReading;


	/**
	 * Conversation Constructor
	 * 
	 * Takes in a file to read
	 * Sets up a Dialogue so that we can read an XML file
	 */
	public Conversation(TextAsset textFileToRead, string conversationID)
	{
		isReading = true;
		//dialogue = Dialogue.GetData (textFile);
		index = 0;
		XmlReaderSettings settings = new XmlReaderSettings();
		//settings.ConformanceLevel = ConformanceLevel.Auto;
		settings.IgnoreWhitespace = true;
		dialogue = new List<Speech>();



		// read in our file into our XML reader thing
		using (reader = XmlReader.Create (new StringReader (textFileToRead.text), settings))
		{
			


			// if our attribute is current, we are there!
			// if not, move on

			// the first thing we do is move to our first attribute
			// we're really just moving to the first speech here
			isReading = true;
			while (isReading)
			{

				// if we read something and get null back, we are false
				// first we move to attribute
				if (reader.Read ())
				{


					// if we have an element, debug it
					// if that element has attributes, debug that
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "Speeches" && reader.GetAttribute (0) == conversationID)
					{
						

						
						XmlReader inner = reader.ReadSubtree ();

						// jump to the first speech item
						inner.ReadToDescendant("Speeches");

						//


						// next we read over and make our SPEECH item
						while (inner.Read())
						{


							// if we have an element of type speech
							if (reader.NodeType == XmlNodeType.Element && inner.Name == "Speech")
							{
								
								// create a new speech element and get all of our parts
								dialogue.Add(new Speech());
								dialogue [dialogue.Count -1].name = inner.GetAttribute ("name");
								dialogue[dialogue.Count -1].type = inner.GetAttribute("type");


							}
							else if (inner.NodeType == XmlNodeType.Element && inner.Name == "SpeechText")
							{

								// get our speech text for our current element
								dialogue[dialogue.Count -1].SpeechText = inner.ReadElementContentAsString();
							}
							else if (inner.NodeType == XmlNodeType.Element && inner.Name == "Options")
							{
								dialogue [dialogue.Count - 1].type = "options";


								while (inner.ReadToFollowing ("option"))
								{
									string textItem = inner.ReadString ();
									dialogue [dialogue.Count - 1].options.Add (textItem);

								}
							}

						}

						inner.Close ();



					} 
					else
					{
						//reader.Skip ();
					}
				} 
				else
				{
					isReading = false;
				}
			}
							
		}

		reader.Close ();


	}



	/**
	 * getDialogue simply returns our Dialogue object - not sure if we'll need this or not
	 */
	public List<Speech> getDialogue()
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






