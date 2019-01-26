using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public float volume;
    public float desiredVolume;
    public float pitch;
    public AudioClip clip;
    public AudioSource source;
    public string name;
    public bool loop;
    public float fade;
    public float currentFade;

    public Sound(string name = "clip", float volume = 1.0f, float pitch = 1.0f, AudioClip clip = null, bool loop = false, float fade = 0.1f)
    {
        this.name = name;
        this.volume = volume;
        this.desiredVolume = volume;
        this.pitch = pitch;
        this.clip = clip;
        this.loop = loop;
        this.fade = fade;
        this.currentFade = fade;
    }

    public bool Fade (bool fadingIn, float deltaTime)
    {
        currentFade -= deltaTime;

        if (fadingIn)
        {
            volume += (desiredVolume / fade)*deltaTime;
        } else
        {
            volume -= (desiredVolume / fade)*deltaTime;
        }
        if (currentFade <= 0)
        {
            volume = desiredVolume;
            currentFade = fade;
            return true;
        }
        return false;
    }
}