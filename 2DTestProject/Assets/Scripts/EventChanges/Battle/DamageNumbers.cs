using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Damage numbers rising above player : makes them appear and attaches them to object point
/// </summary>
public class DamageNumbers : MonoBehaviour
{
	public GameObject prefabDamage;
	public RectTransform battleCanvas;


	/// <summary>
	/// Creates the damage popup after damage is taken
	/// </summary>
	/// <param name="damage">Damage Number.</param>
	/// /// <param name="position">Position of unit to put numbers over</param>
	public void CreateDamagePopup(int damage, Vector3 position)
	{
		// let's just put it at player position


		GameObject damageGameObject = (GameObject)Instantiate (prefabDamage, position, Quaternion.identity);

		if (battleCanvas.Equals (null))
		{
			battleCanvas = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		}

		// set the parent to the canvas so it can display
		damageGameObject.transform.SetParent (battleCanvas, false);

		// set the scale to 1
		damageGameObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		// set the position to the unit we've passed the position of, and get the screen point
		damageGameObject.transform.position = Camera.main.WorldToScreenPoint(position) + new Vector3(0, 25, 0);

		// update the text
		damageGameObject.GetComponent<Text> ().text = damage.ToString ();

		// destroy the object after a moment so that we don't accumulate
		damageGameObject.AddComponent<DestroyObjectOnTimer> ();

	}
}
