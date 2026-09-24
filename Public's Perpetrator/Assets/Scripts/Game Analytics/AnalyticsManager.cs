using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;

    [Header("Time factors")]
    private float session_StartTime;

    [Header("Run factors")]
    private readonly HashSet<string> interacted_object_ids = new HashSet<string>();

    [Header("Room factors")]
    private string current_room_id;
    private float room_enter_time;


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
    // Story Progression Metrics       //
    // =============================== //


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
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to log session data: {e.Message}");
        }
    }


    public void LogSkipCutscene(CutsceneType cutscene_type)
    {
        if (!isInitialized) return;
        CustomEvent skip_cutscene = new CustomEvent("Skipped Cutscene")
        {
            { "Skipped Cutscene", 1 },
            { "Cutscene Name", cutscene_type.ToString() }
        };
        AnalyticsService.Instance.RecordEvent(skip_cutscene);
    }


    public void LogFails(string fail_type)
    {
        if (!isInitialized) return;
        CustomEvent fail_event = new CustomEvent("Triggered Failure")
        {
            { "Failure Type", fail_type },
            { "Fail Count", 1 }                                              // can be aggregated in the dashboard to calculate TOTAL
        };
        AnalyticsService.Instance.RecordEvent(fail_event);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene 1") LogRuns();
        else return;

        ExitRoom();
        EnterRoom("Living Room");
    }


    public void LogRuns()
    {
        if (!isInitialized) return;
        CustomEvent run_event = new CustomEvent("Game Run Started")
        {
            { "Runs", 1 },
        };
        AnalyticsService.Instance.RecordEvent(run_event);
    }


    // =============================== //
    // Terrain Exploration Metrics     //
    // =============================== //


    public void RecordInteract(string object_id)
    {
        if (string.IsNullOrWhiteSpace(object_id))
        {
            Debug.LogWarning("Interaction was ignored because its ID is empty.");
            return;
        }

        bool is_exclusive = interacted_object_ids.Add(object_id);   // first interaction is TRUE, otherwise is FALSE

        if (is_exclusive) LogExclusiveInteract(object_id);
        else LogRepeatInteract(object_id);
    }


    public void LogRepeatInteract(string object_id)
    {
        if (!isInitialized) return;
        CustomEvent rep_interaction = new CustomEvent("Object Interacted With")
        {
            { "Object ID", object_id },
            { "Interaction Count", 1 }
        };
        AnalyticsService.Instance.RecordEvent(rep_interaction);
    }


    public void LogExclusiveInteract(string object_id)
    {
        if (!isInitialized) return;
        CustomEvent ex_interaction = new CustomEvent("Exclusive Interaction")
        {
            { "Object ID", object_id },
            { "Interaction Count", 1 }
        };
        AnalyticsService.Instance.RecordEvent(ex_interaction);
    }


    public void EnterRoom(string room_id)
    {
        if (string.IsNullOrWhiteSpace(room_id)) return;

        current_room_id = room_id;
        room_enter_time = Time.realtimeSinceStartup;
    }


    public void ExitRoom()
    {
        if (string.IsNullOrWhiteSpace(current_room_id)) return;

        float room_duration = Time.realtimeSinceStartup - room_enter_time;
        LogRoomVisit(current_room_id, room_duration);

        current_room_id = null;
        room_enter_time = 0f;
    }


    private void LogRoomVisit(string room_id, float duration)
    {
        if (!isInitialized) return;

        CustomEvent room_timed = new CustomEvent("Room Visit")
        {
            { "Room ID", room_id },
            { "Duration Seconds", duration }
        };
        AnalyticsService.Instance.RecordEvent(room_timed);
    }


    // =============================== //
    // Item Collection Metrics         //
    // =============================== //


    public void LogItemCollected(string item_id, bool is_clue)
    {
        if (!isInitialized) return;

        CustomEvent item_collect = new CustomEvent("Item Collected")
        {
            { "Item ID", item_id },
            { "Item Category", is_clue ? "Clue" : "Others" },
            { "Item Count", 1 }
        };
        AnalyticsService.Instance.RecordEvent(item_collect);
    }


    // =============================== //
    // Final Analytics Flush           //
    // =============================== //


    private void OnApplicationQuit()
    {
        if (!isInitialized) return;
        LogTotalPlaytime();
        AnalyticsService.Instance.Flush();                                  // force a final data dump before the executable closes
    }
}
