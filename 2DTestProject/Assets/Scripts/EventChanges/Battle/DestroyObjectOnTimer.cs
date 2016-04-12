using UnityEngine;
using System.Collections;


/// <summary>
/// Destroys object on timer.
/// </summary>
public class DestroyObjectOnTimer : MonoBehaviour 
{

	public GameObject thisGameObject;
	public float timer;

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		// if we do not have an object initialized, use the one this instance is attached to
		if (thisGameObject == null)
			thisGameObject = gameObject;


		// if we do not have a timer, just set it to a base 2 seconds
		if (timer <= 0)
			timer = 2;

		// destroy this object after timer seconds
		Destroy (thisGameObject, timer);
	
	}
	

}
