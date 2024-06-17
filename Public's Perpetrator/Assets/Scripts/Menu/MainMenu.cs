using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition_fade;

    public void Start()
    {
        transition_fade.Play("EndFade");
    }
    
    public void OpenMenu()
    {
        StartCoroutine(PreviousPart());
    }

    public void ChooseChapter()
    {
        StartCoroutine(NextPart());
    }

    public void PlayGame()
    {
        StartCoroutine(NextPart());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator NextPart()
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transition_fade.Play("EndFade");
                Debug.Log("faded");
    }

    private IEnumerator PreviousPart()
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        transition_fade.Play("EndFade");
                Debug.Log("faded");
    }
}
