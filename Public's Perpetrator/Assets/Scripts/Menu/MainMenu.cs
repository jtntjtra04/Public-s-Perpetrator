using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public Animator transition_fade;
    public SceneAudioManager menu_audio;


    // =============================== //


    public void Start() 
    {
        if(tag == "Main Menu")
        {
            menu_audio.ChangeSceneMusic(menu_audio.BGM_menu);
        }

        if (tag == "Chapters")
        {
            menu_audio.StopSceneMusic();
            menu_audio.ChangeSceneMusic(menu_audio.BGM_chapters);
        }

        transition_fade.Play("EndFade");
    }
    

    public void OpenMenu()  
    {
        menu_audio.StopSceneMusic();
        menu_audio.PlaySceneSFX(menu_audio.SFX_click);
        StartCoroutine(PreviousPart());
    }


    public void ChooseChapter()
    {
        menu_audio.StopSceneMusic();
        menu_audio.PlaySceneSFX(menu_audio.SFX_click);
        StartCoroutine(NextPart());
    }


    public void PlayGame()
    {
        menu_audio.PlaySceneSFX(menu_audio.SFX_click);
        StartCoroutine(NextPart());
    }


    public void ExitGame()
    {
        menu_audio.PlaySceneSFX(menu_audio.SFX_click);
        Application.Quit();
    }


    private IEnumerator NextPart()      // fade transition and scene switching
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transition_fade.Play("EndFade");
    }


    private IEnumerator PreviousPart()  // fade transition and scene switching
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        transition_fade.Play("EndFade");
    }
}
