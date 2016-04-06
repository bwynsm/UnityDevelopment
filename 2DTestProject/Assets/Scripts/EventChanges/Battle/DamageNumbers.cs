using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
	public Transform damageTransform;
	public GameObject prefabDamage;


	/// <summary>
	/// Creates the damage popup after damage is taken
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void CreateDamagePopup(int damage, GameObject parentObject)
	{
		GameObject damageGameObject = (GameObject)Instantiate (prefabDamage, damageTransform.position, damageTransform.rotation);
		damageGameObject.transform.parent = parentObject.transform;

		damageGameObject.GetComponent<Text> ().text = damage.ToString ();

	}
}
