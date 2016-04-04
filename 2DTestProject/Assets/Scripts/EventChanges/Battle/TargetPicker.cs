using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;


public class TargetPicker : MonoBehaviour 
{

	// In this class, we are going to get a list of possible
	// units to click on in this battle. We are going to do this by first
	// determining whether this is a player unit or an enemy unit.
	// if the player selects to go right or left, they can switch sides
	// if they hit x, they select that unit
	// if they hit z, they deselect that unit
	// if they hit up or down, they move up or down the list (cyclical)

	// on start, we are going to need:
	// list of all players
	// list of current attacking player (can you attack yourself?)
	// current command
	// I think from MENU we call TARGET PICKER and from TARGET PICKER we call COMMANDS
	// and from COMMANDS we call PLAYER ATTACK functions

	public List<CharacterConversable> battleList;
	public CharacterConversable currentPlayer;
	private List<CharacterConversable> teammateList;
	private List<CharacterConversable> opponentList;

	private GUIStyle style;
	private Texture2D boxTexture;

	// once this function is done, we'll have a target. Either it will
	// be a non-target, or an actual target
	public CharacterConversable chosenTarget;
	public bool hasChosenTarget = false;
	private int index = 0;
	public int targetSide = 1; // 1 if enemy, 0 if ally - can be reset as public from player based on commands
	private Vector2[] targetRect = new Vector2[4]; 

	void Start()
	{
		style = new GUIStyle ();
		style.border.bottom = 3;
		style.border.top = 3;
		style.border.left = 3;
		style.border.right = 3;
		boxTexture = new Texture2D (1, 1);
		boxTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Border.png", typeof(Texture2D));


	}

	/// <summary>
	/// Loads the battle.
	/// </summary>
	public void loadBattle()
	{
		// call a function that gets two separate battle lists of
		// allies and enemies
		drawBattleLines();

	}



	// in update, we want to go through and wait for input.
	void Update()
	{
		// if we have chosen a target, return null
		if (hasChosenTarget)
			return;

		// just wait for input : if we get up down left or right, we simply don't end
		// our input. Otherwise, we could end our input
		if (!Input.anyKeyDown)
			return;



		// we have a keypress
		else
		{
			// if our keypress is one of the cardinal directions....!
			if (Input.GetKeyDown (KeyCode.UpArrow))
			{

				// if we have more room in the opponents side
				if (targetSide == 1 && index < opponentList.Count - 1)
				{
					index++;
					Debug.Log("OPPONENT NAME : " + opponentList[index].playerName + " " + index);
				} 
				else if (targetSide == 0 && index < teammateList.Count - 1)
				{
					index++;
					Debug.Log("TEAMMATE NAME : " + teammateList[index]);
				}
			} 
			else if (Input.GetKeyDown (KeyCode.DownArrow))
			{
				// if we have more room in the opponents side
				if (targetSide == 1 && index > 0)
				{
					index--;
					Debug.Log("OPPONENT NAME : " + opponentList[index].playerName + " " + index);
				}
				else if (targetSide == 0 && index > 0)
				{
					index--;
					Debug.Log("TEAMMATE NAME : " + teammateList[index]);
				}

			} 
			else if (Input.GetKeyDown (KeyCode.LeftArrow))
			{
				targetSide = 0;

				// if our index is higher or lower than max, set the index to
				// the max it can be
				switch (targetSide)
				{
				case 0:
					if (index >= teammateList.Count)
						index = teammateList.Count - 1;
					break;
				case 1:
					if (index >= opponentList.Count)
						index = opponentList.Count - 1;
					break;
				default:
					break;
				}
			} 
			else if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				targetSide = 1;

				// if our index is higher or lower than max, set the index to
				// the max it can be
				switch (targetSide)
				{
				case 0:
					if (index >= teammateList.Count)
						index = teammateList.Count - 1;
					break;
				case 1:
					if (index >= opponentList.Count)
						index = opponentList.Count - 1;
					break;
				default:
					break;
				}
			}


			// otherwise, let's see if we got any other interesting input
			else if (Input.GetKeyDown (KeyCode.X))
			{
				// select our current index - how are we storing index?
				// parallel indeces? that will keep track of right and left as well
				// at this point we have selected something. Set our selected to our current
				// and say that we are done selecting so that we can move on
				// get based off of our index
				// based on our current side and our current index, set our value to
				if (targetSide == 0)
				{
					chosenTarget = teammateList [index];
				} 
				else
				{
					chosenTarget = opponentList [index];
				}

				hasChosenTarget = true;
			} 
			else if (Input.GetKeyDown (KeyCode.Z))
			{

				// return null and say that we have selected something
				chosenTarget = null;
				hasChosenTarget = true;
			}	

		}
	}




	/// <summary>
	/// When we are in drawing phase
	/// </summary>
	void OnGUI()
	{
		if (hasChosenTarget)
			return;




		// get the current player selected by the index
		CharacterConversable currentlySelectedTarget;

		// get our currently selected target to make a rectangle around
		if (targetSide == 0)
		{
			currentlySelectedTarget = teammateList [index];
		} 
		else
		{
			currentlySelectedTarget = opponentList [index];
		}


		Bounds bounds = currentlySelectedTarget.gameObject.GetComponent<SpriteRenderer>().bounds;
		//Debug.Log ("CURRENTLY SELECTED UNIT'S TRANFORM AND NAME : " + " BOUNDS : " + bounds.min.x + " " + bounds.min.y  + " , " + bounds.max.x + " , " + bounds.max.y);
		//Debug.Log("EXTENTS : " + bounds.center + " " + currentlySelectedTarget.playerName + " " + currentlySelectedTarget.transform.position);

		targetRect[0] = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, 0));
		targetRect[1] = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, 0));
		targetRect[2] = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, 0));
		targetRect[3] = Camera.main.WorldToScreenPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, 0));

		//Vector3 cameraPoint = Camera.main.WorldToScreenPoint (new Vector3(currentlySelectedTarget.transform.position.x, currentlySelectedTarget.transform.position.y, 0));

		// get into screen height space
		for (int i = 0; i < targetRect.Length; i++)
		{
			targetRect [i].y = Screen.height - targetRect [i].y;
		}

		// calculate the mins and maxes
		Vector3 min = targetRect [0];
		Vector3 max = targetRect [0];
		for (int i = 1; i < targetRect.Length; i++)
		{
			min = Vector3.Min (min, targetRect [i]);
			max = Vector3.Max (max, targetRect [i]);
		}




		//Debug.Log ("CAMERA POINT : " + cameraPoint + " " + currentlySelectedTarget.playerName + " " + index);
		Rect rectanglePlayer = Rect.MinMaxRect (min.x, min.y, max.x, max.y);
		//rectanglePlayer.x = cameraPoint.x;
		//rectanglePlayer.y = cameraPoint.y;
		//rectanglePlayer.width = 100;
		//rectanglePlayer.height = 100;
		//Debug.Log (" MIN : " + min + "  MAX : " + max + " WIDTH AND HEIGHT : " + rectanglePlayer.width + " , " + rectanglePlayer.height);   

	
		//GUI.backgroundColor = Color.clear;
		//GUI.color = Color.green;
     	//Render the box
		//GUI.Box (rectanglePlayer, boxTexture);
		GUI.DrawTexture (rectanglePlayer, boxTexture);


		//ShadowAndOutline.DrawOutline (rectanglePlayer, "hello this is our text", style, Color.black, Color.blue, rectanglePlayer.height);
	}






	/// <summary>
	/// Selects the target for attacking or buffing or whatever
	/// </summary>
	/// <returns>The target.</returns>
	public IEnumerator selectTarget()
	{
		while (!hasChosenTarget)
			yield return null;
	}




	/// <summary>
	/// Draws the battle lines by creating two lists of sides
	/// </summary>
	void drawBattleLines()
	{
		// if our lists are null, update them
		if (teammateList == null)
		{
			teammateList = new List<CharacterConversable> ();
		}

		if (opponentList == null)
		{
			opponentList = new List<CharacterConversable> ();
		}


		// loop over battle list and create a teammate list and an opponent list
		foreach (CharacterConversable player in battleList)
		{

			Debug.Log (" CURRENT PLAYER : " + currentPlayer.playerName + " ITERATION PLAYER : " + player.playerName + " LOCATION : " + player.transform.position);

			// if the player is a player character, they are on our side
			// if we are a player
			if (player.isPlayerCharacter)
			{
				// if our current player is a player character, and this is too, then this
				// is a teammate
				if (currentPlayer.isPlayerCharacter)
				{
					teammateList.Add (player);
				} 
				else
				{
					opponentList.Add (player);
				}

			} 

			// if this player is not, add to our opponent list if we are
			// a player character
			else
			{
				// if our current player is a player character, and this is too, then this
				// is a teammate
				if (currentPlayer.isPlayerCharacter)
				{
					opponentList.Add (player);

				}

				// if our current player is an enemy.... add to teammate list
				else
				{
					teammateList.Add (player);	
				}
			}
		}

	}






}
