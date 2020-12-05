using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;
	public Sound[] sounds;
	private Sound s;

	private string sound;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}

	void Start()
	{
		Play("Loop");
	}

	public void Play(string sound)
	{
		s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	//To reduce sound when transition between menu.
	public IEnumerator ReduceVolume()
	{
		sound = "Loop";

		s = Array.Find(sounds, item => item.name == sound);

		while (true)
		{
			if (s.source.volume <= 0.35f)
			{
				s.source.volume = 0.35f;
				break;
			}

			s.source.volume -= 0.03f;

			yield return new WaitForSeconds(0.005f);
		}
	}

	//Increase sound when loading a new menu.
	public IEnumerator IncreaseVolume()
	{
		print("Increasing");
		sound = "Loop";

		s = Array.Find(sounds, item => item.name == sound);

		while (true)
		{
			if (s.source.volume >= 0.6f)
			{
				s.source.volume = 0.6f;
				break;
			}

			s.source.volume += 0.03f;

			yield return new WaitForSeconds(0.005f);
		}
	}
}
