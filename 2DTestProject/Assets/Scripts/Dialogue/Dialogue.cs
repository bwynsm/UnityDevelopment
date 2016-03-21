using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

 public class Dialogue 
 {    
    // dialogue contains a list of possible speeches
	// in addition to a list of speeches, contains 
	// the number of the particular speech
	// conversation contains a list of dialogues
	public Dictionary<string, List<Speech>> Speeches;
	public string conversationID;
	private TextAsset xmlText;

	public Dialogue(TextAsset textToRead, string conversationNumber)
	{
		conversationID = conversationNumber;
		xmlText = textToRead;
		Speeches = new Dictionary<string, List<Speech>> ();

		setupConversation ();

	}

	public List<Speech> getDialogue(string conversationNumber)
	{
		return Speeches [conversationNumber];
	}

	public void setConversationID(string conversationNumber)
	{
		conversationID = conversationNumber;
	}


	// other than that, we simply read everything in and start to store it
	public void setupConversation()
	{
		// using the text to read that is our current asset, we'll set up
		// our dialogue list of speeches by number
		XDocument docu = XDocument.Parse(xmlText.text);
		XElement root = docu.Root;

		// loop over speeches
		IEnumerable <XElement> speeches = root.Elements(XName.Get("Speeches"));




		// loop over the group now
		// can we still get the information from an element
		foreach (XElement xElement in speeches)
		{

			// get the attributes
			// and assign that to the dictionary key
			if (xElement.Name == "Speeches")
			{
				// get the conversationID
				if (xElement.HasAttributes && xElement.Attribute ("id") != null  && xElement.Attribute ("id").Value != "")
				{
					Speeches.Add (xElement.Attribute ("id").Value, new List<Speech>());
					string elementID = xElement.Attribute ("id").Value;



					// get this element with this particular speech id
					// loop over speeches
					IEnumerable <XElement> chatItems = xElement.Elements(XName.Get("Speech"));

					// now that we have the SPEECH items, let's loop over those and fill that out
					foreach (XElement speechItem in chatItems)
					{
						// now we start creating our speech item so that we can add that to 
						// our dictionary once we are ready
						Speech speechObject = new Speech();
						speechObject.options = new List<Options>();

						// get the attributes if we have any
						if (speechItem.HasAttributes)
						{
							// in here we can create our new speech item to loop. First we want
							// to get our speaker and our type if there is one
							if (speechItem.Attribute ("name") != null && speechItem.Attribute ("name").Value != "")
							{
								speechObject.name = speechItem.Attribute ("name").Value;
							}
							if (speechItem.Attribute ("type") != null)
							{
								speechObject.type = speechItem.Attribute ("type").Value;
							}


							// loop over nodes
							IEnumerable<XElement> finalOptions = speechItem.Descendants();


							foreach (XElement words in finalOptions)
							{
								Options optionsSet = new Options ();
								// if we have a speechtext, we'll just add that
								if (words.Name == "SpeechText")
								{
									speechObject.SpeechText = words.Value;
								}
								if (words.Name == "option")
								{
									
									// if we have a command
									if (words.HasAttributes)
									{
										// just go through our list of attributes
										if (words.Attribute ("command") != null)
										{
											optionsSet.command = words.Attribute ("command").Value;
										}
										if (words.Value != null && words.Value != "")
										{
											optionsSet.option = words.Value;
										}
										if (words.Attribute ("playerToAlter") != null)
										{
											optionsSet.playerToAlter = words.Attribute ("playerToAlter").Value;
										}
										if (words.Attribute ("currentPlayer") != null)
										{
											optionsSet.currentPlayer = words.Attribute ("currentPlayer").Value;
										}
									}

									// if we have an option, add it to our dictionary
									speechObject.options.Add(optionsSet);
								}
							}

						}

						// add our speechobject now to our dictionary
						Speeches[elementID].Add(speechObject);

					}




				}

			}

		}


	}

	// read in the dialogue



 }
