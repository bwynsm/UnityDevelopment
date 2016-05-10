using UnityEngine;
using System.Collections;

public class GUIDisplayItems : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad (gameObject);
	}

	public Texture2D background;		// unused at the moment - texture background for menu
	public Texture2D texturePickerBorder;

	public Font defaultFont;
	public GameObject prefabButton;		// this is the current button we are using for display



}
