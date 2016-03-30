using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	public enum BATTLE_STATES
	{
		START = 0,
		DECIDE_TURN = 1,
		DECIDE_ATTACK = 2,
		PERFORM_COMMANDS = 3,
		CHECK_CONDITIONS = 4,
		WIN = 5,
		LOSE = 6
	};


	BATTLE_STATES currentState;

	// classes we use
	LoadBattleScene sceneInit;
	CharacterConversable currentPlayerTurn;
	CharacterConversable[] battleTurnOrder;
	int currentTurn = 0;
	public bool waitingForTurn = false;
	public bool turnFinished = false;
	public string attackDone;
	public Text displayAttackText;
	public GameObject attackTextPanel;




	// Use this for initialization
	void Start () 
	{
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
		sceneInit = GameObject.Find ("LoadScene").GetComponent<LoadBattleScene> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Toolbox.Instance.isLocked)
			return;

		Toolbox.Instance.isLocked = true;

	
		switch (currentState)
		{


		// load our scene and move our battle forward
		case BATTLE_STATES.START:
			
			sceneInit.LoadBattleSceneItems ();
			currentState = BATTLE_STATES.DECIDE_TURN;
			battleTurnOrder = sceneInit.turnOrder;
			Toolbox.Instance.isLocked = false;
				break;


		// decide whose turn it is
		case BATTLE_STATES.DECIDE_TURN:
			

			currentState = BATTLE_STATES.DECIDE_ATTACK;
			currentState = BATTLE_STATES.DECIDE_ATTACK;
			// pick the turn person
			currentPlayerTurn = battleTurnOrder [currentTurn];

			// update our current turn to the next player in the queue
			// and cycle if we are at the end.
			currentTurn++;
			if (currentTurn >= battleTurnOrder.Length)
				currentTurn = 0;

			Toolbox.Instance.isLocked = false;


			break;



		// decide which attack we are using on whoever's turn it is
		case BATTLE_STATES.DECIDE_ATTACK:
			// get our battle manager if we are a good guy.
			// Otherwise, perform an attack if we are a bad guy
			if (!waitingForTurn)
			{
				waitingForTurn = true;
				turnFinished = false;

				if (currentPlayerTurn.isPlayerCharacter)
				{
					// get their battle mode and start gaming
					currentPlayerTurn.gameObject.GetComponent<BattleMenu> ().isMyTurn = true;
				}

				// otherwise, if it is not my turn, enemy attack!
				else
				{
					StartCoroutine (currentPlayerTurn.gameObject.GetComponent<EnemyAttack> ().Attack ());
				}
			}

			if (turnFinished)
			{
				waitingForTurn = false;
				turnFinished = false;
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
			currentState = BATTLE_STATES.DECIDE_TURN;
			Toolbox.Instance.isLocked = false;
			break;

		// if the player side has won, hooray! victory conditions and experience
		case BATTLE_STATES.WIN:
			break;

		// if the good side has lost, sad day. Penalties and teleport
		case BATTLE_STATES.LOSE:
			break;

		// if we hit a non-existent case
		default:
			break;

		}


	}

	private IEnumerator displayAttack()
	{
		// wait for a couple of seconds and then we'll change the state
		attackTextPanel.SetActive(true);
		displayAttackText.text = currentPlayerTurn.playerName + " " + attackDone;
		yield return new WaitForSeconds (2);

		currentState = BATTLE_STATES.CHECK_CONDITIONS;

		attackTextPanel.SetActive(false);
		Toolbox.Instance.isLocked = false;
	}


}
