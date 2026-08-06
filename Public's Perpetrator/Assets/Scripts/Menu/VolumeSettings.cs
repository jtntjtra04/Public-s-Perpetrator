using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer volume_mixer;
    [SerializeField] private Slider music_slider;
    [SerializeField] private Slider sfx_slider;


    private void Start()
    {
        if (PlayerPrefs.HasKey("music volume") || PlayerPrefs.HasKey("sfx volume"))
        {
            LoadMusicVolume();
            LoadSFXVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }


    // =============================== //
    // Music Volume                    //
    // =============================== //


    public void SetMusicVolume()    // sets the volume according to slider
    {
        float volume = music_slider.value;
        volume_mixer.SetFloat("music volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("music volume", volume);
    }


    public void LoadMusicVolume()   // loads the game with the value on the slider on start
    {
        music_slider.value = PlayerPrefs.GetFloat("music volume");
        SetMusicVolume();
    }


    // =============================== //
    // Sound & Effects Volume          //
    // =============================== //


    public void SetSFXVolume()
    {
        float volume = sfx_slider.value;
        volume_mixer.SetFloat("sfx volume", Mathf.Log10(volume) * 20);
    }


    public void LoadSFXVolume()
    {
        sfx_slider.value = PlayerPrefs.GetFloat("sfx volume");
        SetSFXVolume();
    }
}
