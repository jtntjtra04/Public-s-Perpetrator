using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource footstepSource;    // separate AudioSource for footsteps, assigned in Inspector
    public AudioClip BGM_night;
    public AudioClip BGM_crime_scene;
    public AudioClip BGM_basement;
    public AudioClip door;
    public AudioClip shelf_drag;
    public AudioClip inventory_open;
    public AudioClip item_obtained;
    public AudioClip walking_wood;


    private void Start()
    {
    musicSource.clip = BGM_night;
    musicSource.Play();
    }


    public void PlaySFX(AudioClip clip, float pitch = 1f)
    {
        SFXSource.pitch = pitch;
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
