using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip BGM_menu;
    public AudioClip BGM_chapters;
    public AudioClip BGM_mysterious;
    public AudioClip BGM_relaxed;
    public AudioClip BGM_nighttime;
    public AudioClip SFX_opendoor;
    public AudioClip SFX_notification;
    public AudioClip SFX_click;
    public AudioClip SFX_doorknock;


    // =============================== //


    public void StopSceneMusic()
    {
        MusicSource.Stop();        
    }


    public void PlaySceneSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }


    public void ChangeSceneMusic(AudioClip music)
    {
        MusicSource.Stop();
        MusicSource.clip = music;
        MusicSource.Play();
    }
}
