using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] musics;
	public Sound[] sounds;

	// to save musics and sounds volume from settings
	[Range(0f, 1f)]
	public float musicVol = 1f;
	[Range(0f, 1f)]
	public float soundVol = 1f;

	// to save audio source that are currently playing music
	[HideInInspector]
	public static AudioSource musicPlayer;

	// to save current scene name
	private String currentScene;

	void Awake()
	{
		//Debug.Log("Awake function of Audio Manager called");
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

        foreach (Sound s in musics)
        {
			if (s.source == null)
            {
				//Debug.Log("Audio source is null");
				s.source = gameObject.AddComponent<AudioSource>();
			}
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixerGroup;

            // to ignore pause so music can keep playing
            if (s.ignorePause)
            {
                s.source.ignoreListenerPause = true;
            }
        }

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = mixerGroup;

			// to ignore pause so sound can keep playing
			if (s.ignorePause)
			{
				s.source.ignoreListenerPause = true;
			}
		}
	}

    private void Update()
    {
		Scene scene = SceneManager.GetActiveScene();

		// if current scene is still null then add the current scene name. Only play scene music 
		if (currentScene == null)
        {
			//Debug.Log("First scene loaded");
			currentScene = scene.name;
			PlaySceneMusic();
        }

		// check if the scene has changed
		else if (scene.name != currentScene)
        {
			//Debug.Log("Scene has changed");
			currentScene = scene.name;
			StopMusic();
			PlaySceneMusic();
		}
	}

    // function to play music for the scene everytime the scene loaded
    private void PlaySceneMusic()
    {
        // Debug.Log("Play scene music called");

        // check scene name to play music specifically for the scene
        if (currentScene == "MainMenu")
        {
            PlayMusic("Menu_Music");
        }

        else if (currentScene == "Level 1")
		{
            PlayMusic("Level1_Music");
        }

		else
        {
			PlayMusic("Level1_Music");
		}
	}

    public void PlayMusic(string sound)
	{
		Sound s = Array.Find(musics, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("UI Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * musicVol;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
		s.source.enabled = false;
		s.source.enabled = true;
		musicPlayer = s.source;
		
		//Debug.Log("Called");
	}

	// function currently not used, but might be later
	public void StopMusic(string sound)
	{
		Sound s = Array.Find(musics, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("UI Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	// to stop music that are currently playing
	public void StopMusic()
    {
		if (musicPlayer != null)
		{
			musicPlayer.Stop();
		}
    }

	public void PlaySound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * soundVol;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void StopSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void SetMusicVol(float volume)
    {
		// to limit the volume range from 0-1
		if (volume > 1)
        {
			volume = 1;
        }
		else if (volume < 0)
		{
			volume = 0;
		}

		//change saved music volume
		musicVol = volume;

		foreach (Sound s in musics)
        {
			// if there are sound playing, change the volume so the change will be immediately heard
			if (s.source.isPlaying)
            {
				s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * musicVol;
			}
        }
	}

	public void SetSoundVol(float volume)
	{
		// to limit the volume range from 0-1
		if (volume > 1f)
		{
			volume = 1f;
		}
		else if (volume < 0f)
		{
			volume = 0f;
		}

		//change saved music volume
		soundVol = volume;

		foreach (Sound s in sounds)
		{
			// if there are sound playing, change the volume so the change will be immediately heard
			if (s.source.isPlaying)
			{
				s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) * soundVol;
			}
		}
	}

	// just to change music from outside easier
	public void ChangeMusic(string music)
    {
		StopMusic();
		PlayMusic(music);
    }
}

// source: Brackeys