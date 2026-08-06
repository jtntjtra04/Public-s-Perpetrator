using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource TypeSound;
    public AudioClip BGM_menu;
    public AudioClip BGM_chapters;
    public AudioClip BGM_mysterious;
    public AudioClip BGM_relaxed;
    public AudioClip BGM_nighttime;
    public AudioClip SFX_type_blip;
    public AudioClip SFX_opendoor;
    public AudioClip SFX_notification;
    public AudioClip SFX_click;
    public AudioClip SFX_doorknock;
    public AudioClip SFX_carkeys;
    public AudioClip SFX_gunshot;
    public AudioClip SFX_closecardoor;

    private Coroutine fade_Coroutine;
    private float default_volume = 0.35f;


    // =============================== //
    // Regular Track Manipulation      //
    // =============================== //


    public void StopSceneMusic()
    {
        MusicSource.Stop();        
    }


    public void PlaySceneSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
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


    // =============================== //
    // Fade In & Out for Music         //
    // =============================== //


    public void ChangeSceneMusic(AudioClip music)
    {
        MusicSource.Stop();
        MusicSource.clip = music;
        MusicSource.Play();
    }


    public void FadeInSceneMusic(AudioClip music, float fade_duration = 1f)
    {
        if (fade_Coroutine != null) StopCoroutine(fade_Coroutine);
        fade_Coroutine = StartCoroutine(FadeInMusic(music, fade_duration));
    }


    public void FadeOutSceneMusic(float fade_duration = 1f)
    {
        if (fade_Coroutine != null) StopCoroutine(fade_Coroutine);
        fade_Coroutine = StartCoroutine(FadeOutMusic(null, fade_duration));
    }


    private IEnumerator FadeOutMusic(AudioClip music, float duration)   // music fades out slowly though playing
    {
        float start_volume = MusicSource.volume;

        if (MusicSource.isPlaying && start_volume > 0)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                MusicSource.volume = Mathf.Lerp(start_volume, 0f, t / duration);
                yield return null;
            }

            MusicSource.volume = 0f;
            MusicSource.Stop();
        }
    }


    private IEnumerator FadeInMusic(AudioClip music, float duration)    // smothly fade in the next music track
    {
        if (music == null) yield break;

        MusicSource.clip = music;
        MusicSource.volume = 0f;
        MusicSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            MusicSource.volume = Mathf.Lerp(0f, default_volume, t / duration);
            yield return null;
        }

        MusicSource.volume = default_volume;
    }
}
