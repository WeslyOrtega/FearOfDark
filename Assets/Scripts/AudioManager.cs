using UnityEngine.Audio;
using UnityEngine;

using System;

// Credit: @Brackeys - "Introduction to AUDIO in Unity"

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    public AudioMixerGroup audioMixer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer;
        }
    }

    void Start()
    {
        Play("background_music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find<Sound>(sounds, s => s.name == name);
        if (s is not null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogWarning("Cannot play audio.\nSound with name '"
                + name + "' was not found.");
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find<Sound>(sounds, s => s.name == name);
        if (s is not null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.LogWarning("Cannot stop audio.\nSound with name '"
                + name + "' was not found.");
        }
    }
}
