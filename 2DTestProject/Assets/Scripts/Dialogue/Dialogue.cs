using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


 [XmlRoot("Dialogue")]
 public class Dialogue 
 {    
     /* ======================================== *\
      *               ATTRIBUTES                 *
     \* ======================================== */


	[XmlArray("Speeches"), XmlArrayItem("Speech")]
	public Speech[] Speeches;
     
 
     /// <summary>
     /// Gets or sets the options.
     /// </summary>
     /// <value>The options.</value>
     [XmlArray("Options")]
     [XmlArrayItem("Option")]
     public List<string> Options
     {
         get ;
         set ;
     }
		

	/**
	 * This getData simply creates our serializer and returns it over the
	* text asset text file that we sent in (xml file)
	*/
	public static Dialogue GetData(TextAsset textFile)
	{
		var serializer = new XmlSerializer(typeof(Dialogue));
		using (var reader = new System.IO.StringReader (textFile.text))
		{
			return serializer.Deserialize (reader) as Dialogue;
		}
	}
     
     // You can define other constructors if you want too

 }
