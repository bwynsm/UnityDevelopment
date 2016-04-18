using UnityEngine;
using System.Collections;


/// <summary>
/// Sort objects in the camera to pretend we have 3D
/// </summary>
public class SortObjects : MonoBehaviour 
{

	/// <summary>
	/// This runs after update and sorts the objects in the visible spectrum by y position
	/// so that our character can hide behind a barrel if he/she is scared
	/// </summary>
	void LateUpdate()
	{

		// if the object is visible, we may change the order of its display
		if (GetComponent<Renderer> ().isVisible) 
		{
			//Debug.Log ("we are changing object location : " + gameObject.name + " " + (int)Camera.main.WorldToScreenPoint (GetComponent<MeshRenderer> ().bounds.min).y * -1);

			GetComponent<Renderer> ().sortingOrder = (int)Camera.main.WorldToScreenPoint (GetComponent<MeshRenderer> ().bounds.min).y * -1;
		}

	}

}
