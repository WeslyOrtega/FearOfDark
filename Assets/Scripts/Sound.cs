using UnityEngine.Audio;
using UnityEngine;

// Credit: YT @Brackeys - "Introduction to AUDIO in Unity"

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;
}
