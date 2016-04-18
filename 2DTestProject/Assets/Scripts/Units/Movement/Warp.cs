using UnityEngine;
using System.Collections;


/// <summary>
/// Warp.
/// 
/// This is for moving from map to map.
/// Determines collisions that make an exit and then moves the user
/// and also helps move the camera over to the new position
/// </summary>
public class Warp : MonoBehaviour {

	public Transform warpTarget;


	/// <summary>
	/// Raises the trigger enter2d event.
	/// 
	/// If this is raised, someone has entered our zone. We need to make sure
	/// it is the right sort of object. Then we freeze for the transfer (do we want
	/// to do this on both sides or just one?)
	/// 
	/// Then we start the fading out and in and then move the camera
	/// </summary>
	/// <param name="other">Other.</param>
	IEnumerator OnTriggerEnter2D(Collider2D other)
	{
		// if we are in here, an object collided
		ScreenFader sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();

		if (!other.gameObject.GetComponent<PlayerMovement> ())
		{
			yield break;
		}

		PlayerUnit playerObject = other.gameObject.GetComponent<PlayerUnit> ();
		playerObject.freeze = true;

		// fade screen to black on warp
		yield return StartCoroutine (sf.FadeToBlack());


		// change the position of the character and the camera position to follow
		other.gameObject.transform.position = warpTarget.position;


		// move the camera once we have moved our character
		Camera.main.transform.position = warpTarget.position;

		// this is currently to try and make the player face downwards
		//playerObject.faceForward(); 
		playerObject.freeze = true;


		// fade in
		yield return StartCoroutine (sf.FadeToClear());



		// once we have faded back in, we can start moving again.
		// we also want to be facing down from wherever we came in if possible? not sure how
		// to get this one working.
		playerObject.freeze = false;



	}
}
