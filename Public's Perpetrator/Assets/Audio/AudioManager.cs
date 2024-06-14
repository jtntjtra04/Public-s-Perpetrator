using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip BGM1;
    public AudioClip BGM2;
    public AudioClip BGM3;
    public AudioClip door;
    public AudioClip shelfdrag;
    public AudioClip steeldoor;

    private void Start()
    {
    musicSource.clip = BGM1;
    musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.clip.LoadAudioData();
        musicSource.Play();
    }
}
