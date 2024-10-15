using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource BackSource;
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip BGMMenu;
    public AudioClip BGMScene1;
    public AudioClip BGMScene2;
    public AudioClip BGMScene3;
    public AudioClip ButtonPress;
    public AudioClip Xtra;
    public AudioClip Xtra2;
    public AudioClip Xtra3;

    public void Start()
    {
        MusicSource.clip = BGMMenu;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();        
    }

    public void PlayMenuSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ChangeMenuMusic(AudioClip music)
    {
        MusicSource.Stop();
        MusicSource.clip = music;
        MusicSource.Play();
    }

    public void PlayBackNoise(AudioClip clip)
    {
        BackSource.clip = clip;
        BackSource.Play();
    }

    public void StopBackNoise()
    {
        BackSource.Stop();
    }
}
