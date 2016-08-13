using UnityEngine;
using System.Collections;

/// <summary>
/// Combat text : translates combat text in a direction
/// </summary>
public class CombatText : MonoBehaviour {

	public float speed;
	public Vector3 direction;
	public float timer;

	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		direction = new Vector3 (0, 2, 0);
	}
	
	// Update is called once per frame
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		float translation = speed * Time.deltaTime;
		transform.Translate (direction * translation);
	}
}
