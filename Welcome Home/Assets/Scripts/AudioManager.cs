using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    List<Sound> musicTracks = new List<Sound>();
    List<Sound> soundEffects = new List<Sound>();
    AudioSource musicChannel = new AudioSource();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        for (int i = 0; i < soundEffects.Count; i++)
        {
            AudioSource soundSrc = new AudioSource();
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
        audiosrc.Play();
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
        musicChannel.Play();
    }
}
