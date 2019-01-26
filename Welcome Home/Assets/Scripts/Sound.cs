using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class Sound
{
    public float volume;
    public float pitch;
    public AudioClip clip;
    public AudioSource source;
    public string name;

    public Sound(string name = "clip", float volume = 1.0f, float pitch = 1.0f, AudioClip clip = null)
    {
        this.name = name;
        this.volume = volume;
        this.pitch = pitch;
        this.clip = clip;
    }
}