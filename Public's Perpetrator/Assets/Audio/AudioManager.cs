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
    public AudioClip metal_screech;
    public AudioClip lamp_flicker;
    public AudioClip flashlight_click;


    private void Start()
    {
    musicSource.clip = BGM_night;
    musicSource.Play();
    }


    public void PlaySFX(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        SFXSource.volume = volume;
        SFXSource.pitch = pitch;
        SFXSource.PlayOneShot(clip);
    }


    public void PlaySpatialSFX(AudioClip clip, Vector3 position, float maxDistance, float volume)
    {
        Vector3 flatPosition = new Vector3(position.x, position.y, 0f);

        GameObject tempGO = new GameObject("SpatialSFX" + clip.name);
        tempGO.transform.position = flatPosition;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;

        aSource.spatialBlend = 1.0f;
        aSource.rolloffMode = AudioRolloffMode.Linear;

        aSource.minDistance = 1f;
        aSource.maxDistance = maxDistance;  // sound area is bound to this distance

        aSource.Play();
        Destroy(tempGO, clip.length);
    }


    public void ChangeMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.clip.LoadAudioData();
        musicSource.Play();
    }
}
