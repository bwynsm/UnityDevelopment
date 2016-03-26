using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicChanger : MonoBehaviour {


	public AudioMixerSnapshot outOfCombat;
	public AudioMixerSnapshot inCombat;
	public AudioClip[] stings;
	public AudioSource stingSource;
	public float bpm = 60;


	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;

	// Use this for initialization
	void Start () 
	{
		m_QuarterNote = 60 / bpm;
		m_TransitionIn = m_QuarterNote * 6;
		m_TransitionOut = m_QuarterNote * 8;

	}


	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other)
	{

		// if we have entered into a combat zone, change the music to our combat transition
		if (other.CompareTag ("CombatZone"))
		{
			inCombat.TransitionTo (m_TransitionIn);
			//PlaySting();
		}
	}

	public void transitionOut()
	{
		//AudioMixerSnapshot.TransitionTo(m_TransitionOut);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponentsInChildren<AudioSource>()[0].Play();
	}


	/// <summary>
	/// Raises the trigger exit2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		// if we have exited a combat zone, transition back out to our ambient music
		if (other.CompareTag("CombatZone"))
		{
			outOfCombat.TransitionTo(m_TransitionOut);
		}
	}


	// at some point we may need to play a sting - unused for now
	/// <summary>
	/// Plays the sting. A sting being a short burst of sound on entering a new area or something
	/// </summary>
	void PlaySting()
	{
		int randClip = Random.Range (0, stings.Length);
		stingSource.clip = stings[randClip];
		stingSource.Play();
	}


}