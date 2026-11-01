using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    PauseMenu pause_menu;
    DialogueManager dialogue_manager;

    [Header("Timer variables")]
    public float min_duration = 0.1f;                                // 20 minutes
    private float elapsedTime = 0f;
    private bool timerIsRunning = true;
    public bool twenty_mins_passed = false;

    [Header("UI references")]
    [SerializeField] private GameObject menu_panel;                 // drag your menu panel here
    [SerializeField] private GameObject menu_text;                  // external message text (outside the pause panel)


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        pause_menu = FindAnyObjectByType<PauseMenu>();
        dialogue_manager = FindAnyObjectByType<DialogueManager>();
    }


    private void Update()
    {
        if (timerIsRunning)
        {
            elapsedTime += Time.deltaTime;                                                    // Time.unscaledTime to count down despite pausing, Time.deltaTime otherwise

            if (elapsedTime >= min_duration * 60f && !dialogue_manager.dialogue_ongoing)      // 20 minutes * 60 seconds = 1200 seconds
            {
                twenty_mins_passed = true;
                pause_menu.isPaused = true;
                TriggerTimedEvent();
                timerIsRunning = false;
            }
        }
    }


    private void TriggerTimedEvent()
    {
        Debug.Log("20-minutes reached.");

        pause_menu.Pause();
        if (menu_text != null) menu_text.SetActive(true);

        Time.timeScale = 0f;
    }


    public void ResetTimer()                            // optional reset
    {
        elapsedTime = 0f;
        timerIsRunning = true;
    }
}
