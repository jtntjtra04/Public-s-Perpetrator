using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;

    [Header("Time factors")]
    private float session_StartTime;


    // =============================== //
    // Initialize Manager & Services   //
    // =============================== //


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);                                            // destroys any clones and NO duplicate managers
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);                             // sets the first instance as THE manager
            session_StartTime = Time.realtimeSinceStartup;
        }
    }


    private async void Start()
    {
        if (Instance != this) return;                                       // prevents an unintentional initialization from clone
        await UnityServices.InitializeAsync();                              // initializing Unity services for analytics
        AnalyticsService.Instance.StartDataCollection();
        isInitialized = true;
    }
    

    // =============================== //
    // Metric Measurement Classes      //
    // =============================== //


    //public void NextLevel(int currentLevel)
    //{
    //    if(!isInitialized) return;
    //    CustomEvent nextlevel_event = new CustomEvent("Next Level")
    //    {
    //          {"CurrentLevel", currentLevel }
    //    };

    //    AnalyticsService.Instance.RecordEvent(nextlevel_event);
    //    AnalyticsService.Instance.Flush();
    //}


    //public void RestartGame()
    //{
    //    if (!isInitialized) return;
    //    AnalyticsService.Instance.RecordEvent("Restart Game");
    //    AnalyticsService.Instance.Flush();
    //}


    private void LogTotalPlaytime()
    {
        if (!isInitialized) return;
        float totalPlaytime = Time.realtimeSinceStartup - session_StartTime;                // calculate cumulative runtime in seconds since the first scene opened
        CustomEvent playtime_event = new CustomEvent("Session Playtime")
        {
            { "TotalDurationSeconds", totalPlaytime }
        };

        try
        {
            AnalyticsService.Instance.RecordEvent(playtime_event);
            AnalyticsService.Instance.Flush();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to log session data: {e.Message}");
        }
    }


    public void LogFails()
    {
        if (!isInitialized) return;
        CustomEvent fail_event = new CustomEvent("Triggered Trap")
        {
            { "FailCount", 1 }
        };

        AnalyticsService.Instance.RecordEvent(fail_event);
        AnalyticsService.Instance.Flush();
    }


    private void OnApplicationQuit()
    {
        if (!isInitialized) return;
        LogTotalPlaytime();
        AnalyticsService.Instance.Flush();                                  // force a final data dump before the executable closes
    }
}
