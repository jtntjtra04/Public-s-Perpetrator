using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool isInitialized = false;

    [Header("Run & session")]
    public static string participant_id;
    private string game_session_id;
    private float run_startTime;
    private int run_attempt;
    private bool run_active;
    private readonly Queue<CustomEvent> pending_events = new Queue<CustomEvent>();      // data or metric queue incase Unity Analytics isn't initialized yet

    [Header("Story progression factors")]
    private bool skipped_cutscene_thisRun;
    private int failures_thisRun;

    private readonly HashSet<string> interacted_object_ids = new HashSet<string>();
    private readonly HashSet<string> collected_item_ids = new HashSet<string>();
    private readonly HashSet<string> collected_clue_ids = new HashSet<string>();

    [Header("Room factors")]
    private string current_room_id;
    private float room_enter_time;

    [Header("Item and clue factors")]
    private const int total_clues = 4;
    private int clues_gathered_thisRun;
    private bool clues_timeLogged;

    private float clue_startTime;
    private bool clue_timer_active;

    [Header("Puzzle factors")]
    private readonly Dictionary<string, int> puzzle_attempt_counts = new Dictionary<string, int>();
    private string active_puzzle;
    private float puzzle_attempt_start;
    private bool puzzle_attempt_active;

    [Header("Milestone indicator")]
    private int milestone_queue;                                                         // floor 1 → floor 2 → first puzzle → basement → final puzzle / ending determinant


    // =============================== //
    // Initialize Manager & Services   //
    // =============================== //


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);                                            // destroys any clones and NO duplicate managers
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);                             // sets the first instance as THE MANAGER

            game_session_id = Guid.NewGuid().ToString("N");                 // unique session ID for this game session
        }
    }


    private async void Start()
    {
        if (Instance != this) return;                                                   // prevents an unintentional initialization from clone

        try
        {
            InitializationOptions initialization_options = new InitializationOptions().SetEnvironmentName("pre-test-lab");

            await UnityServices.InitializeAsync(initialization_options);                // initializing Unity services for analytics & begin data collection
            AnalyticsService.Instance.StartDataCollection();

            isInitialized = true;                                                       // Unity services has been INITIALIZED
            FlushPendingEvents();
            Debug.Log("Unity Analytics initialized.");
        }

        catch (Exception exception)
        {
            Debug.LogError($"Unity Analytics initialization failed: {exception}");
        }
    }


    private void SubmitEvent(CustomEvent analytics_event)                               // safety buffer to queue events before initialization
    {
        if (!isInitialized)
        {
            pending_events.Enqueue(analytics_event);
            return;
        }
        AnalyticsService.Instance.RecordEvent(analytics_event);
    }


    private void FlushPendingEvents()
    {
        while (pending_events.Count > 0) AnalyticsService.Instance.RecordEvent(pending_events.Dequeue());
    }


    // =============================== //
    // Story Progression Metrics       //
    // =============================== //


    public void BeginRun()
    {
        if (run_active) return;

        run_attempt++;
        run_active = true;
        run_startTime = Time.realtimeSinceStartup;

        skipped_cutscene_thisRun = false;                                   // resets skip cutscene & number of failures every NEW RUN
        failures_thisRun = 0;

        clues_gathered_thisRun = 0;                                         // resets clue solving time & puzzle variables every NEW RUN
        clues_timeLogged = false;
        collected_clue_ids.Clear();
        clue_timer_active = false;

        active_puzzle = null;
        puzzle_attempt_start = 0f;
        puzzle_attempt_counts.Clear();
        puzzle_attempt_active = false;

        milestone_queue = 0;                                                 // reset milestone progressed by player during new run

        Debug.Log($"Run {run_attempt} initiated.");
    }


    public void EndRun(string ending_type)
    {
        Debug.Log($"EndRun called. Ending: {ending_type}, Active: {run_active}, " + $"Run: {run_attempt}, Initialized: {isInitialized}");
        if (!run_active) return;

        LogMilestone("Milestone: Reached an Ending");
        ExitRoom();                                                                    // accounts for the basement before ending the run
        float run_playtime = Time.realtimeSinceStartup - run_startTime;                // calculate cumulative runtime in seconds since the first scene opened

        CustomEvent end_run_event = new CustomEvent("Game_Run_Ended")
        {
            { "run_duration", run_playtime },
            { "ending_type", ending_type },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(end_run_event);

        run_active = false;
    }


    public void LogSkipCutscene(CutsceneType cutscene_type)
    {
        if (!run_active) return;
        skipped_cutscene_thisRun = true;

        CustomEvent skip_cutscene_event = new CustomEvent("Skipped_Cutscene")
        {
            { "cutscene_type", cutscene_type.ToString() },
            { "skip_cutscene_count", 1 },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(skip_cutscene_event);
    }


    public void LogFails(string fail_type)
    {
        if (!run_active) return;
        failures_thisRun ++;

        CustomEvent fail_event = new CustomEvent("Triggered_Failure")
        {
            { "failure_type", fail_type },
            { "fail_count", 1 },                                                        // can be aggregated in the dashboard to calculate TOTAL
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(fail_event);
    }


    // =============================== //
    // Terrain Exploration Metrics     //
    // =============================== //


    public void RecordInteract(string object_id)
    {
        Debug.Log($"RecordInteract called. ID: '{object_id}', " +$"Run active: {run_active}, Initialized: {isInitialized}");

        if (!run_active) return;
        if (string.IsNullOrWhiteSpace(object_id))
        {
            Debug.LogWarning("Interaction was ignored because its ID is empty.");
            return;
        }

        bool is_exclusive = interacted_object_ids.Add(object_id);                       // first interaction is TRUE, otherwise is FALSE

        if (is_exclusive) LogExclusiveInteract(object_id);                              // interaction is recorded as exclusive or repeated
        else LogRepeatInteract(object_id);

        Debug.Log($"Submitting interaction. ID: '{object_id}', " + $"Exclusive: {is_exclusive}");
    }


    public void LogRepeatInteract(string object_id)
    {
        CustomEvent rep_interaction = new CustomEvent("Repeat_Interaction")
        {
            { "object_ID", object_id },
            { "interaction_count", 1 },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(rep_interaction);
    }


    public void LogExclusiveInteract(string object_id)
    {
        CustomEvent ex_interaction = new CustomEvent("Exclusive_Interaction")   
        {
            { "object_ID", object_id },
            { "interaction_count", 1 },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(ex_interaction);
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
        if (scene.name != "Scene 1" || mode != LoadSceneMode.Single) return;
        ExitRoom();
        EnterRoom("Living Room");                                                       // ensures that the first entered and observed room is the living room.
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
        CustomEvent room_roam_event = new CustomEvent("Room_Visit")
        {
            { "room_ID", room_id },
            { "duration_seconds", duration },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(room_roam_event);
    }


    // =============================== //
    // Item Collection Metrics         //
    // =============================== //


    public void BeginClueCounter()
    {
        Debug.Log($"BeginClueCounter called. Active: {run_active}, " +$"Timer active: {clue_timer_active}");

        if (!run_active || clue_timer_active) return;

        clue_startTime = Time.realtimeSinceStartup;
        clue_timer_active = true;
    }


    public void LogItemCollected(string item_id, bool is_clue)
    {
        if (string.IsNullOrWhiteSpace(item_id)) return;
        bool is_newClue_thisRun = run_active && clue_timer_active && is_clue && collected_clue_ids.Add(item_id);    // if it's a new discovered CLUE in a RUN

        if (is_newClue_thisRun && !clues_timeLogged)
        {
            clues_gathered_thisRun++;
            if (clues_gathered_thisRun >= total_clues) {
                LogCluesCollected();
                LogMilestone("Milestone: All Clues Collected");
            }
        }

        bool is_newItem = collected_item_ids.Add(item_id);                                                          // first collection is TRUE, otherwise is FALSE
        if (!is_newItem) return;

        CustomEvent item_interaction = new CustomEvent("Item_Collected")
        {
            { "item_ID", item_id },
            { "item_categorized", is_clue ? "Clue" : "Others" },
            { "item_count", 1 },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(item_interaction);
    }


    private void LogCluesCollected()
    {
        float clue_collection_time = Time.realtimeSinceStartup - clue_startTime;
        clues_timeLogged = true;
        clue_timer_active = false;

        if (!run_active) return;

        CustomEvent clue_event = new CustomEvent("All_Clues_Collected")
        {
            { "clues_gathered", clues_gathered_thisRun },
            { "time_required", clue_collection_time },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(clue_event);
    }


    // =============================== //
    // Puzzle Solving Metrics          //
    // =============================== //


    public void BeginPuzzleAttempt(string puzzle_id)
    {
        if (string.IsNullOrWhiteSpace(puzzle_id) || puzzle_attempt_active) return;

        if (!puzzle_attempt_counts.ContainsKey(puzzle_id)) puzzle_attempt_counts.Add(puzzle_id, 0);
        puzzle_attempt_counts[puzzle_id]++;

        active_puzzle = puzzle_id;
        puzzle_attempt_start = Time.realtimeSinceStartup;
        puzzle_attempt_active = true;
    }


    public void EndPuzzleAttempt(string puzzle_id, bool solved)
    {
        if (!puzzle_attempt_active || active_puzzle != puzzle_id) return;

        float attempt_duration = Time.realtimeSinceStartup - puzzle_attempt_start;
        int attempt_number = puzzle_attempt_counts[puzzle_id];

        CustomEvent puzzle_event = new CustomEvent("Puzzle_Attempt")
        {
            { "puzzle_ID", puzzle_id },
            { "attempt_number", attempt_number },
            { "attempt_duration_seconds", attempt_duration },
            { "solved", solved },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };
        SubmitEvent(puzzle_event);

        active_puzzle = null;
        puzzle_attempt_start = 0f;
        puzzle_attempt_active = false;
    }


    public void CancelPuzzleAttempt()                                       // precautionary puzzle cancel if death or restarted when doing a puzzle
    {
        active_puzzle = null;
        puzzle_attempt_start = 0f;
        puzzle_attempt_active = false;
    }


    // =============================== //
    // Milestone & Progress Time       //
    // =============================== //


    public void LogMilestone(string milestone_id)
    {
        if (!run_active || string.IsNullOrWhiteSpace(milestone_id)) return;
        float elapsedTime = Time.realtimeSinceStartup - run_startTime;

        CustomEvent milestone_event = new CustomEvent("Milestone_Reached")
        {
            { "milestone_name", milestone_id },
            { "elapsed_time", Time.realtimeSinceStartup - run_startTime },
            { "milestone_order", ++milestone_queue },
            { "run_number", run_attempt },
            { "session_ID", game_session_id },
            { "participant_ID", participant_id }
        };

        SubmitEvent(milestone_event);   
    }


    // =============================== //
    // Final Analytics Flush           //
    // =============================== //


    private void OnApplicationQuit()
    {
        ExitRoom();                                                         // calls ExitRoom() to log time INCASE the game closes & forces a data flush
        if (isInitialized) AnalyticsService.Instance.Flush();
    }
}
