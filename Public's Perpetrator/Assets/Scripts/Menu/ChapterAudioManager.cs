using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ChpMusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip BGMChp;
    public AudioClip ButtonPress;
    public AudioClip Xtra;

    public void Start()
    {
        ChpMusicSource.clip = BGMChp;
        ChpMusicSource.Play();
    }

    public void StopMusic()
    {
        ChpMusicSource.Stop();        
    }

    public void PlayMenuSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
