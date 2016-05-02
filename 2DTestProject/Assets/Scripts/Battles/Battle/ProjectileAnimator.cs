using UnityEngine;
using System.Collections;


/// <summary>
/// Projectile animator : works with distance spells and arrows and such
/// spawning from a player
/// </summary>
public class ProjectileAnimator : MonoBehaviour {

	Animator anim;
	bool isCastingSpell = false;
	bool isShootingArrow = false;
	bool haveHitTarget = false;
	GameObject projectile;


	GameObject player;
	GameObject archer;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		GameObject.FindGameObjectWithTag ("Spell").GetComponent<SpriteRenderer>().enabled = false;
		player = GameObject.FindGameObjectWithTag ("PlayerCharacter");
		archer = GameObject.Find ("Archer");
		projectile = GameObject.Find ("Arrow");
	}


	/// <summary>
	/// Update the location on this instance if we are firing a projectile
	/// </summary>
	void Update()
	{
		// but in here, we are doing the udpates in the arrow if the arrow is active
		if (isShootingArrow && !haveHitTarget)
		{
			// update the arrows position
			float xPos;
			float yPos;


			// if we are firing a projectile, let's set up teh x and y coordinates of
			// our vector 2 so that we can fire an updating object in their direction
			if ((player.transform.position.x - projectile.transform.position.x) > 0)
				xPos = 1;
			else if ((player.transform.position.x - projectile.transform.position.x) < 0)
				xPos = -1;
			else
				xPos = 0;


			// update the object in their direction
			if ((player.transform.position.y - projectile.transform.position.y) > 0)
				yPos = 1;
			else if ((player.transform.position.y - projectile.transform.position.y) < 0)
				yPos = -1;
			else
				yPos = 0;


			// this is the angle of where the character is in relation to the shooter
			Vector2 angle = new Vector2 (xPos, yPos);
			projectile.transform.position = ((Vector2)projectile.transform.position + (angle * 2.0f * Time.deltaTime));


			// this is if we are close enouugh, let's move it into position and call it a hit
			if (projectile.transform.position.x <= player.transform.position.x + 0.2 && projectile.transform.position.x >= player.transform.position.x - 0.2 && projectile.transform.position.y <= player.transform.position.y + 0.2 && projectile.transform.position.y >= player.transform.position.y - 0.2)
			{
				projectile.transform.position = player.transform.position;
				haveHitTarget = true;
				// this is where we set off fireworks?
			}

		}

		// otherwise, if we are done shooting our arrow - it has landed, stop the animation and call it quits
		else if (isShootingArrow)
		{
			isShootingArrow = false;
			StartCoroutine(StopProjectile ());
		}
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
	/// Fires a projectile across the screen - an arrow perhaps
	/// </summary>
	/// <returns>Returns nothing until we are done animating</returns>
	public IEnumerator FireProjectile()
	{
		GameObject arrow = GameObject.Find ("Arrow");
		arrow.GetComponent<SpriteRenderer>().enabled = true;

		// set the arrow at the transform position of the archer
		arrow.transform.position = archer.transform.position;

		Debug.Log ("firing arrow");
		isShootingArrow = true;
		haveHitTarget = false;
		anim.SetBool ("IsCastingSpell", true);

		while (isShootingArrow)
			yield return null;
	}


	/// <summary>
	/// Stops the animation and calls it quits. We are done shooting.
	/// Turn off our sprite and go back to no animation
	/// </summary>
	public IEnumerator StopProjectile()
	{
		isShootingArrow = false;

		yield return new WaitForSeconds (1.0f);
		anim.SetBool ("IsCastingSpell", false);
		GameObject.Find ("Arrow").GetComponent<SpriteRenderer>().enabled = false;
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
