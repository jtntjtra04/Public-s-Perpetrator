using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;
    public Animator transition_anim;                // change of scene after secret link puzzle is solved
    public PlayerMovement player_movement;
    public DialogueManager dialogue_manager;


    void Start()
    {
        pausePanel.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dialogue_manager.dialoguebox_on)
        {
            if (isPaused)
                Resume();
        
            else
                Pause();
        }
    }


    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (player_movement != null)
        {
            player_movement.DisableMovement();
        }
    }


    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (player_movement != null)
        {
            player_movement.EnableMovement();
        }
    }


    public void BackToMenu()
    {
        StartCoroutine(FadeTransition());
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }


    private IEnumerator FadeTransition()
    {
        transition_anim.Play("StartFade");
        yield return new WaitForSeconds(2f);
        transition_anim.Play("EndFade");
    }
}
