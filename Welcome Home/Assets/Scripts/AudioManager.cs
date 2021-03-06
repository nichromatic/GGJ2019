﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> musicTracks = new List<Sound>();
    public List<Sound> soundEffects = new List<Sound>();
    AudioSource musicChannel;
    float musicFadeMultiplier = 3f;
    float musicFadeInitialVolume = 0f;
    bool musicFading = false;
    public static AudioManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        musicChannel = this.gameObject.AddComponent<AudioSource>();
        for (int i = 0; i < soundEffects.Count; i++)
        {
            AudioSource soundSrc = this.gameObject.AddComponent<AudioSource>();
            soundEffects[i].source = soundSrc;
        }
    }

    public void playSound(string name)
    {
        AudioSource audiosrc = soundEffects.Find(s => s.name == name).source;
        if (audiosrc.isPlaying)
        {
            audiosrc.Stop();
        }
        audiosrc.clip = soundEffects.Find(m => m.name == name).clip;
        audiosrc.volume = soundEffects.Find(m => m.name == name).volume;
        audiosrc.pitch = soundEffects.Find(m => m.name == name).pitch;
        audiosrc.loop = soundEffects.Find(m => m.name == name).loop;
        audiosrc.Play();
    }

    public void stopSound(string name)
    {
        AudioSource audiosrc = soundEffects.Find(s => s.name == name).source;
        if (audiosrc.isPlaying)
        {
            audiosrc.Stop();
        }
    }

    public void playMusic(string name)
    {
        if (musicChannel.isPlaying)
        {
            musicChannel.Stop();
        }
        musicChannel.clip = musicTracks.Find(m => m.name == name).clip;
        musicChannel.volume = musicTracks.Find(m => m.name == name).volume;
        musicChannel.pitch = musicTracks.Find(m => m.name == name).pitch;
        musicChannel.loop = musicTracks.Find(m => m.name == name).loop;
        musicChannel.Play();
    }

    public void stopMusic()
    {
        if (musicChannel.isPlaying)
        {
            musicChannel.Stop();
        }
    }

    [ContextMenu("Change music")]
    public void randomMusic()
    {
        int i = Random.Range(0, 5);
        string name = "nivel" + (i + 1);
        playMusic(name);
        //Debug.Log("Playing music " + (i+1));
    }

    [ContextMenu("Fade out music")]
    public void callFadeOut()
    {
        StartCoroutine(fadeOut());
    }

    [ContextMenu("Fade in music")]
    public void callFadeIn(string name)
    {
        //randomMusic();
        playMusic(name);

        StartCoroutine(fadeIn());
    }

    public IEnumerator fadeOut()
    {
        if (!musicFading)
        {
            musicFadeInitialVolume = musicChannel.volume;
            musicFading = true;
            while (musicChannel.volume > 0)
            {
                musicChannel.volume -= Time.deltaTime / musicFadeMultiplier;
                yield return null;
            }
            musicFading = false;
            stopMusic();
            musicChannel.volume = musicFadeInitialVolume;
        }
    }

    public IEnumerator fadeIn()
    {
        if (!musicFading)
        {
            musicFading = true;
            musicFadeInitialVolume = musicChannel.volume;
            musicChannel.volume = 0f;
            while (musicChannel.volume < musicFadeInitialVolume)
            {
                musicChannel.volume += Time.deltaTime / musicFadeMultiplier;
                yield return null;
            }
            musicFading = false;
        }
    }
}
