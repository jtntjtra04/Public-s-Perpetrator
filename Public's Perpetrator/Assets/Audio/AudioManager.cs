using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource footstepSource;    // separate AudioSource for footsteps, assigned in Inspector
    public AudioClip BGM_windy_indoors;
    public AudioClip BGM_crime_scene;
    public AudioClip door;
    public AudioClip shelf_drag;
    public AudioClip inventory_open;
    public AudioClip item_obtained;
    public AudioClip walking_wood;
    public AudioClip metal_screech;
    public AudioClip lamp_flicker;
    public AudioClip flashlight_click;


    // =============================== //


    private void Start()
    {
    musicSource.clip = BGM_windy_indoors;
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

        GameObject tempGO = new GameObject("SpatialSFX" + clip.name);                   // a temporary object to play these audio cues
        tempGO.transform.position = flatPosition;

        GameObject container = GameObject.Find("ActivePropSFX Holder");                 // store these audio clone cues in a separate folder to prevent clustering in the hierarchy
        if (container == null)
        {
            container = new GameObject("ActivePropSFX Holder");
        }
        tempGO.transform.SetParent(container.transform);

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;

        aSource.spatialBlend = 1.0f;
        aSource.rolloffMode = AudioRolloffMode.Linear;                      

        aSource.minDistance = 1f;
        aSource.maxDistance = maxDistance;                                              // sound area is bound to this distance

        aSource.Play();

        float strictCutoff = 2f;
        float destroyDelay = (clip != null && clip.length > 0) ? Mathf.Min(clip.length, strictCutoff) : strictCutoff;       // strict deletion of clones

        Destroy(tempGO, destroyDelay);
        Destroy(tempGO, clip.length);
    }


    public void ChangeMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.clip.LoadAudioData();
        musicSource.Play();
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }
}
