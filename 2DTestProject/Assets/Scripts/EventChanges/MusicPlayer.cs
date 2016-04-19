using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


/// <summary>
/// Music changer
/// </summary>
public class MusicPlayer 
{


	public AudioMixerSnapshot outOfCombat;
	public AudioMixerSnapshot inCombat;
	public AudioClip[] stings;
	public AudioSource stingSource;
	public float bpm = 60;


	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;

	// Use this for initialization
	public MusicPlayer () 
	{
		m_QuarterNote = 60 / bpm;
		m_TransitionIn = m_QuarterNote * 6;
		m_TransitionOut = m_QuarterNote * 8;

	}




	/// <summary>
	/// Transitions the music out
	/// </summary>
	public void transitionOut()
	{
		//AudioMixerSnapshot.TransitionTo(m_TransitionOut);
		GameObject.FindGameObjectWithTag("MainCamera").GetComponentsInChildren<AudioSource>()[0].Play();
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