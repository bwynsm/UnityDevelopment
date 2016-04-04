using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{

	private Vector3 originPosition;
	//private Quaternion originRotation;
	public float shake_decay;
	public float shake_intensity;
 
 

	/// <summary>
	/// Update this instance. We're going to shake the camera briefly and then
	/// let it slowly decay
	/// </summary>
	void Update ()
	{
		// while we are still shaking, calculate new shake intensite
		if (shake_intensity > 0)
		{
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			/*transform.rotation = new Quaternion(
			originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
			originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
			originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
			originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);*/
			shake_intensity -= shake_decay;
		}
	}
 


	public void Shake(float shakeIntensityVal = 0.3f, float shakeDecayVal = 0.025f)
	{
		originPosition = transform.position;
		//originRotation = transform.rotation;
		shake_intensity = shakeIntensityVal;
		shake_decay = shakeDecayVal;
	}
}
