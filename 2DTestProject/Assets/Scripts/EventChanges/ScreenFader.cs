using UnityEngine;
using System.Collections;

/// <summary>
/// Screen fader.
/// 
/// Runs a canvas that sets the screen to fade and back with
/// yielding animations
/// </summary>
public class ScreenFader : MonoBehaviour {


	Animator anim;
	bool isFading = false;



	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();

	}


	/// <summary>
	/// Fades to clear.
	/// </summary>
	/// <returns>Returns nothing until we are done fading in. Then returns that we are done</returns>
	public IEnumerator FadeToClear()
	{
		isFading = true;
		anim.SetTrigger ("FadeIn");

		while (isFading)
			yield return null;
	}


	/// <summary>
	/// Fades to black.
	/// </summary>
	/// <returns>Returns nothing until we are done fading to black. Then is done</returns>
	public IEnumerator FadeToBlack()
	{
		isFading = true;
		anim.SetTrigger ("FadeOut");

		while (isFading)
			yield return null;
	}

	public void FlashWhite()
	{
		anim.SetTrigger ("FlashWhite");
	}

	public void NormalState()
	{
		anim.SetTrigger ("DoneFlashingWhite");
	}


	/// <summary>
	/// Animations complete.
	/// </summary>
	void AnimationComplete()
	{
		isFading = false;
	}

}
