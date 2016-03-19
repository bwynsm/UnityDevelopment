using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 public class Dialogue 
 {    
     /* ======================================== *\
      *               ATTRIBUTES                 *
     \* ======================================== */
	public List<Speech> speechList;


	// dialogue takes in an xmlreader and textfile

	// get the next speech
	public int index = 0;

	// get dialogues


	public bool hasNext()
	{
		// check length
		if (speechList.Count >= index)
		{
			return true;
		}

		return false;
	}

	public Speech getNext()
	{
		index++;

		return speechList [index];
	}

 }
