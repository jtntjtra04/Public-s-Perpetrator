using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--Audio Source--")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    
    [Header("--Audio Clips--")]
    public AudioClip BackgroundM;
    public AudioClip door;
    public AudioClip shelfdrag;

    private void Start()
    {
        musicSource.clip = BackgroundM;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
