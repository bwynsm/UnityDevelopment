using UnityEngine;
using System.Collections;

public class EventCatch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("we are here");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ButtonClicked(string textItem)
	{
		Debug.Log("we are here");
	}
}
