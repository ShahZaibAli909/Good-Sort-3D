using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public Sounds[] Music, SfxSounds;
    [SerializeField] private AudioSource MusicSource, SfxSource;

    //instance
    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("MainTheme");
    }
    public void PlayMusic(string name)
    {
        Sounds sound = Array.Find(Music, c=> c.soundName == name);

        if(sound == null)
        {
            Debug.Log("Sound Not Found ");
        }else
        {
            MusicSource.clip = sound.soundClip;
            MusicSource.Play();
        }
    }
    public void Stop() 
    {
        MusicSource.Stop();
    }

    public void PlaySoundEffect(string name)
    {
        Sounds Sfx = Array.Find(SfxSounds, sfx=> sfx.soundName == name );

        if(Sfx == null)
        {
            Debug.Log("Sfx Not Found");
        }
        else
        {
            SfxSource.PlayOneShot(Sfx.soundClip);
        }

    }

    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }
    public void ToggleSfx()
    {
        SfxSource.mute = !SfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }
    public void SfxVolume(float volume)
    {
        SfxSource.volume = volume;
    }
}
