using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {

	public float speed;
	public Vector3 direction;
	public float timer;

	// Use this for initialization
	void Start () 
	{
		direction = new Vector3 (0, 2, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float translation = speed * Time.deltaTime;
		transform.Translate (direction * translation);
	}
}
