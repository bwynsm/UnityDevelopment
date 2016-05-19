using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


/// <summary>
/// Battle manager : stores the state of the battle
/// </summary>
public class GameBoyBattleManager : MonoBehaviour 
{

	public enum BATTLE_STATES
	{
		START = 0,
		DECIDE_TURN = 1,
		DECIDE_ATTACK = 2,
		PERFORM_COMMANDS = 3,
		CHECK_CONDITIONS = 4,
		WIN = 5,
		LOSE = 6, 
		CHARACTER_SELECTION,
		END
	};
			
	public BATTLE_STATES currentState;

	// classes we use
	GameBoyLoadGame sceneInit;
	GameBoyUnit currentPlayerTurn;
	List<GameBoyUnit> battleTurnOrder;
	int currentTurn = 0;
	public bool waitingForTurn = false;
	public bool turnFinished = false;
	public string attackDone;
	public Text displayAttackText;
	public GameObject attackTextPanel;

	private bool turnsLoaded = false;
	private int gainedExperience;
	private bool gambling = false;
	public Image gameboyImagePanel;
	public GameObject victoryScreenPanel;
	private bool playAgainChoiceMade = false;

	// maybe it should be up to the battle manager to store good guys and bad guys?



	// Use this for initialization
	void Start () 
	{

		// if we have a gameboy game, delete this object
		if (Toolbox.Instance.battleScene != "GameBoy")
		{
			Destroy (this);
		}

		// The battle manager contains a list of scripts as attached to objects
		// INIT will get the list of objects and do some setup
		// DECIDE ON TURN will call the queue and toggle at this point
		// DECIDE ATTACK will simply call the units attack
		// PERFORM COMMANDS will perform the attack's commands
		// Check conditions will just determine if we are done with the round
		// WIN will fire the winning conditions off
		// LOSE will fire the losing conditions off

		// Battle Manager
		// - START / INIT
		// - DECIDE WHOSE TURN IT IS (speed and turn queue? - for now we'll just swap off in a simple turn based queue)
		// - DECIDE ATTACK (random if enemy)
		// - PERFORM COMMANDS
		// - CHECK CONDITIONS
		// - WIN
		// - LOSE

		currentState = BATTLE_STATES.START;
		sceneInit = GameObject.Find ("LoadScene").GetComponent<GameBoyLoadGame> ();



		Camera.main.orthographicSize = 1;
		Camera.main.transform.position = new Vector3 (-3.78f, -1.30f, -10);

		sceneInit.LoadCharacters ();


	}


	/// <summary>
	/// Update this instance. Controls our state machine. Uses ToolBox
	/// </summary>
	// Update is called once per frame
	void Update () 
	{

		// if we are locked, can't go forward
		if (Toolbox.Instance.isLocked)
			return;

		// lock our toolbox
		Toolbox.Instance.isLocked = true;
		Toolbox toolboxInstance = Toolbox.Instance;

		// switch over our battle state
		switch (currentState)
		{


		// load our scene and move our battle forward
		case BATTLE_STATES.START:
			playAgainChoiceMade = false;
			currentTurn = 0;

			// if we don't have turns loaded, load the turns
			if (!turnsLoaded)
			{
				sceneInit.LoadGameBoyTurns ();
				turnsLoaded = true;
				Toolbox.Instance.isLocked = false;
			}

			// if we have turns loaded, load the battle scene and move into deciding
			// whose turn it is
			else
			{
				sceneInit.LoadBattleSceneItems ();
				currentState = BATTLE_STATES.DECIDE_TURN;
				battleTurnOrder = sceneInit.turnOrder;
				Toolbox.Instance.isLocked = false;
			}
			break;


			// decide whose turn it is
		case BATTLE_STATES.DECIDE_TURN:

			Debug.Log ("We are deciding turn " + currentTurn);
			currentState = BATTLE_STATES.DECIDE_ATTACK;

			// pick the turn person
			currentPlayerTurn = battleTurnOrder [currentTurn];

			// update our current turn to the next player in the queue
			// and cycle if we are at the end.
			currentTurn++;
			if (currentTurn >= battleTurnOrder.Count)
				currentTurn = 0;


			turnFinished = false;
			waitingForTurn = false;
			Toolbox.Instance.isLocked = false;


			break;



			// decide which attack we are using on whoever's turn it is
		case BATTLE_STATES.DECIDE_ATTACK:

			// skip this turn if our player has no health
			if (currentPlayerTurn.isPlayerCharacter && currentPlayerTurn.GetComponent<GameBoyUnit> ().currentHealth <= 0)
			{
				waitingForTurn = true;
				currentState = BATTLE_STATES.LOSE;
				Toolbox.Instance.isLocked = false;
			} 
			// if the current player is an enemy and is dead, victory! (for now - later on we'll just skip this turn
			// and add in a "CHECK" phase
			else if (!currentPlayerTurn.isPlayerCharacter && currentPlayerTurn.GetComponent<GameBoyUnit> ().currentHealth <= 0)
			{

				waitingForTurn = true;
				currentState = BATTLE_STATES.WIN;
				Toolbox.Instance.isLocked = false;
			}



			// get our battle manager if we are a good guy.
			// Otherwise, perform an attack if we are a bad guy
			if (!waitingForTurn)
			{
				waitingForTurn = true;
				turnFinished = false;

				if (currentPlayerTurn.isPlayerCharacter)
				{
					// get their battle mode and start gaming
					//currentPlayerTurn.gameObject.GetComponent<BattleMenu> ().isMyTurn = true;
					StartCoroutine (currentPlayerTurn.Attack ());
				}

				// otherwise, if it is not my turn, enemy attack!
				else
				{
					StartCoroutine (currentPlayerTurn.Attack ());
				}
			}


			// if the turn is finished, perform our commands
			if (turnFinished)
			{
				currentState = BATTLE_STATES.PERFORM_COMMANDS;
				Toolbox.Instance.isLocked = false;

			}

			break;

			// perform commands of the attack
		case BATTLE_STATES.PERFORM_COMMANDS:
			// let's do a little enum in here
			StartCoroutine (displayAttack ());


			break;

			// check the conditions of the battle - has someone won? is any unit
			// to be destroyed?
		case BATTLE_STATES.CHECK_CONDITIONS:

			bool playerStillAlive = true;
			bool enemyStillAlive = true;


			// if our player is dead, we have lost
			if (!playerStillAlive)
			{
				currentState = BATTLE_STATES.LOSE;
				Toolbox.Instance.isLocked = false;
				break;
			}

		
			// if our enemy is dead, we have won
			if (!enemyStillAlive)
			{
				currentState = BATTLE_STATES.WIN;
				Toolbox.Instance.isLocked = false;
				break;
			}


			// first check if the player characters are all dead
			// then check if the enemy characters are all dead
			Debug.Log("we are here in check condition");

			// for each of the enemies in play - are we all dead?
			// for each of the players in play, are we all dead?


			currentState = BATTLE_STATES.DECIDE_TURN;
			Toolbox.Instance.isLocked = false;
			break;

			// if the player side has won, hooray! victory conditions and experience
		case BATTLE_STATES.WIN:




			// let's add that experience to our tally
			// enemy defeated
			GameObject enemyDefeated = GameObject.FindGameObjectWithTag ("Enemy");
			gainedExperience += enemyDefeated.GetComponent<GameBoyUnit> ().experience;

			// if we have received a choice - we can choose to play again
			if (playAgainChoiceMade)
			{

				// check if we are gambling or continuing on.
				if (gambling)
				{
					currentState = BATTLE_STATES.START;
					Toolbox.Instance.isLocked = false;
				} 

				// we have decided we are done gambling on experience
				else if (!gambling)
				{
					currentState = BATTLE_STATES.END;
					Toolbox.Instance.isLocked = false;
				}
			} 

			// display character selection panel
			else
			{
				displayCharacterSelectionPanel ();
			}

			break;

			// if the good side has lost, sad day. Penalties and teleport
		case BATTLE_STATES.LOSE:

			currentState = BATTLE_STATES.END;
			Toolbox.Instance.isLocked = false;

			break;

		case BATTLE_STATES.CHARACTER_SELECTION:
			break;

		case BATTLE_STATES.END:

			// load previous scene
			toolboxInstance.positionInLastScene = toolboxInstance.battlePosition;

			toolboxInstance.sceneAlreadyLoaded = false;
			GameObject.FindGameObjectWithTag ("PlayerCharacter").GetComponent<PlayerUnit> ().playerExperience += gainedExperience;





			// spawn at least location?
			// what if the grue is still there?
			// we'll get this in a moment.
			SceneManager.LoadScene("OpeningScene");

			break;
		
			// if we hit a non-existent case
		default:
			break;

		}


	}



	/// <summary>
	/// Changes the character states.
	/// </summary>
	private void changeCharacterStates()
	{

		// print out sort order
		foreach (var character in battleTurnOrder)
		{
			// set every character to 
			character.GetComponent<Animator>().SetBool("IsFighting", false);
		}
	}


	/// <summary>
	/// Displays the attack in the text panel
	/// </summary>
	/// <returns>The attack.</returns>
	private IEnumerator displayAttack()
	{
		// wait for a couple of seconds and then we'll change the state
		attackTextPanel.SetActive(true);
		displayAttackText.text = currentPlayerTurn.playerName + " " + attackDone;
		yield return new WaitForSeconds (2.5f);

		currentState = BATTLE_STATES.CHECK_CONDITIONS;

		attackTextPanel.SetActive(false);
		Toolbox.Instance.isLocked = false;
	}



	/// <summary>
	/// Displays the character selection panel.
	/// </summary>
	/// <returns>The character selection panel.</returns>
	private void displayCharacterSelectionPanel()
	{
		// until we get a response from our unit picker..
		victoryScreenPanel.SetActive(true);
		AcceptDecline acceptDeclineObject = victoryScreenPanel.GetComponent<AcceptDecline> ();
		acceptDeclineObject.createOptions ();
		acceptDeclineObject.loadAcceptDeclineDisplay ();
	}


	/// <summary>
	/// Play the game again.
	/// </summary>
	public void PlayAgain()
	{
		// destroy both our current units so we can make some new ones
		Destroy (GameObject.FindGameObjectWithTag ("GameBoyUnit"));
		Destroy (GameObject.FindGameObjectWithTag ("Enemy"));

		// remove our panel
		victoryScreenPanel.SetActive (false);

		gambling = true;
		playAgainChoiceMade = true;
		turnsLoaded = false;

		// clear our lists
		sceneInit.turnOrder.Clear ();
		battleTurnOrder.Clear ();
		sceneInit.LoadCharacters ();
		Toolbox.Instance.isLocked = false;
	}


	/// <summary>
	/// Quits the game boy.
	/// </summary>
	public void QuitGameBoy()
	{
		// destroy our units
		Destroy (GameObject.FindGameObjectWithTag ("GameBoyUnit"));
		Destroy (GameObject.FindGameObjectWithTag ("Enemy"));

		victoryScreenPanel.SetActive (false);

		gambling = false;
		playAgainChoiceMade = true;
		Toolbox.Instance.isLocked = false;
	}
}
