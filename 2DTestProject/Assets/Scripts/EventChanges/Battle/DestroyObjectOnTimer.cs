using UnityEngine;
using System.Collections;

public class DestroyObjectOnTimer : MonoBehaviour {

	public GameObject thisGameObject;
	public float timer;

	// Use this for initialization
	void Start () 
	{
		if (thisGameObject == null)
			thisGameObject = gameObject;

		Destroy (thisGameObject, timer);
	
	}
	

}
