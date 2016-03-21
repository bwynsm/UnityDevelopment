using System.Collections;
using System.Collections.Generic;
//using System.Xml;
//using System.Xml.Serialization;



/**
 * Speech defines a simple set of elements for our XML dialogue
 * 
 * - name is the name of the player object
 * - type is the type of item we are reading (normal text, options, etc). What to expect next for the xml
 * -- default here is just text
 * - speechtext is the text field if type = text
 * - options is the options field if we have type = options
 */
public class Speech
{
	//[XmlAttribute("name")]
	public string name;

	//[XmlAttribute("type")]
	public string type;

	public string SpeechText;


	public List<Options> options = new List<Options>();
	public string command;


	public string toString()
	{
		return "Name : " + name + " Type : " + type + " SpeechText : " + SpeechText;
	}

}
