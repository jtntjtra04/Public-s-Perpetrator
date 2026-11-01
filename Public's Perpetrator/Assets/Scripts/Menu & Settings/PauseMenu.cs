using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [Header("Pause variables")]
    public GameObject pausePanel;
    public bool isPaused = false;
    public Animator transition_anim;                            // change of scene after secret link puzzle is solved
    public PlayerMovement player_movement;
    public DialogueManager dialogue_manager;
    public GameTimer game_timer;

    [Header("Research purposes")]
    [SerializeField] private GameObject pause_message;          // extra message for research purposes


    void Start()
    {
        pausePanel.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dialogue_manager.dialogue_ongoing)
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

        if (player_movement != null) player_movement.EnableMovement();
        if (pause_message != null) pause_message.SetActive(false);
    }


    public void BackToMenu()
    {
        if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.CancelPuzzleAttempt();     // precautionary measure incase of exiting mid puzzle
        if (AnalyticsManager.Instance == null && game_timer.twenty_mins_passed)
        {
            Debug.Log("No ending reached. 20 mins marked passed.");
            AnalyticsManager.Instance.EndRun("No Ending - 20 mins mark reached");
        }

        if (pause_message != null) pause_message.SetActive(false);
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
