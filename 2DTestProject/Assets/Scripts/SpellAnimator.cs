using UnityEngine;
using System.Collections;

public class SpellAnimator : MonoBehaviour {

	Animator anim;
	bool isCastingSpell = false;



	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		GameObject.FindGameObjectWithTag ("Spell").GetComponent<SpriteRenderer>().enabled = false;
	}


	/// <summary>
	/// Fades to clear.
	/// </summary>
	/// <returns>Returns nothing until we are done fading in. Then returns that we are done</returns>
	public IEnumerator CastSpell()
	{
		GameObject.FindGameObjectWithTag ("Spell").GetComponent<SpriteRenderer>().enabled = true;
		Debug.Log ("casting spell");
		isCastingSpell = true;
		anim.SetBool ("IsCastingSpell", true);

		while (isCastingSpell)
			yield return null;
	}


	/// <summary>
	/// Fades to black.
	/// </summary>
	/// <returns>Returns nothing until we are done fading to black. Then is done</returns>
	public void StopSpell()
	{
		anim.SetBool ("IsCastingSpell", false);
		GameObject.FindGameObjectWithTag ("Spell").GetComponent<SpriteRenderer>().enabled = false;
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
