using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class CaptureButtonClick : MonoBehaviour {


	public GameObject prefabButton;
	public Transform parentTransform;

	// Use this for initialization
	void Start () {
    
    
    List<string> options = new List<string>();
    options.Add("hello");
    options.Add("second");
    options.Add("third");
    
    for (int i = 0; i < options.Count; i++)
		{


			GameObject goButton = (GameObject)Instantiate (prefabButton);
			goButton.GetComponentInChildren<Text>().text = "Option : " + options[i];

			string x = i.ToString ();
			//goButton.AddComponent(
			goButton.GetComponent<Button>().onClick.AddListener(
				() => {  buttonClicked(x); }
			);
			goButton.transform.SetParent (parentTransform, false);
			//goButton.transform.localScale = new Vector3(1, 1, 1);

			// also add in the resulting function call
			//Button tempButton = goButton.GetComponent<Button>();
			//tempButton.onClick.AddListener(() => { Debug.Log("we are here " + indexNum);});

		}
    
   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void buttonClicked(string i)
	{
		Debug.Log("we are here" + i);
	}
}
