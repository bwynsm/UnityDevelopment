using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
 
     public AudioClip blip;
 
     public Texture2D background;
     //public Texture2D wip3Logo;
 
     public GUIStyle mainButton;
 
     private string[] mainMenuLabels = { "SINGLE RACE", "CAMPAIGN", "SPLIT SCREN", "STATS", "QUIT" };
     private bool[] mainMenuButtons;
     private int mainMenuSelected;
 
     void Awake () {
 
     }
 
     void Start () {
 
         mainMenuButtons = new bool[mainMenuLabels.Length];
         mainMenuSelected = 0;
     
     }
     
     void Update () {
         
         if (Input.GetKeyDown (KeyCode.DownArrow) == true) {
             if (mainMenuSelected < mainMenuLabels.Length - 1)
                 mainMenuSelected += 1;
             else
                 mainMenuSelected = 0;
         }
         
         if (Input.GetKeyDown (KeyCode.UpArrow) == true) {
             if (mainMenuSelected > 0)
                 mainMenuSelected -= 1;
             else
                 mainMenuSelected = mainMenuLabels.Length - 1;
         }
     }
 
     void OnGUI () {
 
         float width = Screen.width / 2;
         float height = Screen.height / 2;
 
         // MAIN BACKGROUND
         GUI.Box (new Rect (width - 960, height - 600, 1920, 1200), background);
 
         // MAIN MENU
         GUI.BeginGroup (new Rect (width - 512, height - 384, 1024, 768));
         
         //GUI.Label (new Rect (64, 96, 416, 48), wip3Logo);
		GUIStyle newStyle = new GUIStyle();
		newStyle.normal.textColor = Color.blue;
         
         for (int i = 0; i < mainMenuLabels.Length; i++) {
             

             GUI.SetNextControlName (mainMenuLabels[i]);
			if (mainMenuSelected == i) {
				mainMenuButtons [i] = GUI.Button (new Rect (64, 160 + i * 64, 416, 48), mainMenuLabels [i], newStyle);
				//Debug.Log ("Main Menu Color ?");
			} else {
				mainMenuButtons [i] = GUI.Button (new Rect (64, 160 + i * 64, 416, 48), mainMenuLabels [i], mainButton);
				//Debug.Log ("Main Menu Color Normal");
			}

         }
         
         GUI.EndGroup ();
         
         GUI.FocusControl (mainMenuLabels[mainMenuSelected]);
         
         if (mainMenuButtons[0]) {
             
             Debug.Log ("SINGLE RACE");
             //GameManager.Instance.SetRaceType (GameManager.RaceType.SingleRace);
         }
         
         if (mainMenuButtons[1]) {}
         
         if (mainMenuButtons[2]) {}
         
         if (mainMenuButtons[3]) {}
         
         if (mainMenuButtons[4]) {
             
             Application.Quit ();
         }
         
         if (Input.GetKey (KeyCode.Return)) {
             mainMenuButtons[mainMenuSelected] = true;
         }
     }
 }
