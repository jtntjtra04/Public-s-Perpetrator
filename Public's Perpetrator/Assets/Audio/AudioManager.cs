using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource footstepSource;    // separate AudioSource for footsteps, assigned in Inspector
    [SerializeField] AudioSource TypeSound;         // when typing a dialogue
    public AudioClip BGM_windy_indoors;
    public AudioClip BGM_crime_scene;
    public AudioClip SFX_door;
    public AudioClip SFX_shelf_drag;
    public AudioClip SFX_inventory_open;
    public AudioClip SFX_item_obtained;
    public AudioClip SFX_flashlight_click;
    public AudioClip SFX_type_blip;
    public AudioClip SFX_paper;
    public AudioClip walking_wood;
    public AudioClip metal_screech;
    public AudioClip lamp_flicker;
    public AudioClip water_drip;
    public AudioClip fridge_hum;


    // =============================== //


    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Chapter 1")
        {
            musicSource.clip = BGM_windy_indoors;
            musicSource.Play();
        }
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
    }


    public void PlayTypeSFX(AudioClip sound)
    {
        TypeSound.clip = sound;
        TypeSound.Play();
    }


    public void StopTypeSFX()
    {
        TypeSound.Stop();
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
