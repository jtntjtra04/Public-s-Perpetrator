using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip BGMMenu;
    public AudioClip ButtonPress;
    public AudioClip Xtra;

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
}
