using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageNumbers : MonoBehaviour
{
	public GameObject prefabDamage;


	/// <summary>
	/// Creates the damage popup after damage is taken
	/// </summary>
	/// <param name="damage">Damage.</param>
	public void CreateDamagePopup(int damage, Vector3 position)
	{
		// let's just put it at player position


		GameObject damageGameObject = (GameObject)Instantiate (prefabDamage, position, Quaternion.identity);

		damageGameObject.transform.SetParent (GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
		damageGameObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		damageGameObject.transform.position = Camera.main.WorldToScreenPoint(position) + new Vector3(0, 25, 0);
		damageGameObject.GetComponent<Text> ().text = damage.ToString ();
		damageGameObject.AddComponent<DestroyObjectOnTimer> ();

		Debug.Log (Camera.main.WorldToScreenPoint (position) + " " + damageGameObject.transform.position + " " + position);
		Destroy (this, 5f);
	}
}
