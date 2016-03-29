using UnityEngine;
using System.Collections;

public class SpellAnimator : MonoBehaviour {

	Animator anim;
	bool isCastingSpell = false;



	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();

	}


	/// <summary>
	/// Fades to clear.
	/// </summary>
	/// <returns>Returns nothing until we are done fading in. Then returns that we are done</returns>
	public IEnumerator CastSpell()
	{
		Debug.Log ("casting spell");
		isCastingSpell = true;
		anim.SetBool ("isCastingSpell", true);

		while (isCastingSpell)
			yield return null;
	}


	/// <summary>
	/// Fades to black.
	/// </summary>
	/// <returns>Returns nothing until we are done fading to black. Then is done</returns>
	public void StopSpell()
	{
		anim.SetBool ("isCastingSpell", false);
	}


	/// <summary>
	/// Animations complete.
	/// </summary>
	void AnimationComplete()
	{
		Debug.Log ("animation complete");
		isCastingSpell = false;
	}

}
