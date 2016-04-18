using UnityEngine;
using System.Collections;

public class DestroyGame : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		Destroy (GameObject.FindGameObjectWithTag ("PlayerCharacter"));
		Toolbox.Instance.playerCharacter = null;
	}

}
