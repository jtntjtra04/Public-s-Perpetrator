using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomSwitch : MonoBehaviour
{
    public Transform destination;
    private bool isTransition = false;                                  // isTransition acts as a safe guard to prevent multiple coroutines
    public Animator transition_anim;
    private PlayerMovement player_movement;

    [SerializeField]
    private string destination_room_id;

    [Header("Darkness Settings")]
    [Tooltip("Destination IDs with faulty lights.")]
    [SerializeField] private string faultLight_rooms;                   // dynamic list for multiple rooms


    private void Awake()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || isTransition) 
        {
            if(Vector2.Distance(collision.transform.position, transform.position) > 0.3f) StartCoroutine(TransitionRoom(collision));
         }
    }


    private IEnumerator TransitionRoom(Collider2D collision)
    {
        isTransition = true;

        if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.ExitRoom();                            // ANALYTICS → room exit

        player_movement.DisableMovement();                                                                      // disable movement & begin transition
        transition_anim.Play("StartFade");
        yield return new WaitForSeconds(1.5f);
        collision.transform.position = new Vector2(destination.position.x, destination.position.y);
        transition_anim.Play("EndFade");

        AmbientDarkness darknessSystem = FindFirstObjectByType<AmbientDarkness>();
        FaintLight faintlampSystem = FindFirstObjectByType<FaintLight>();
        if (darknessSystem != null && faintlampSystem != null)
        {
            bool faultyRoom = faultLight_rooms.Contains(destination_room_id);
            darknessSystem.SetBlackoutState(faultyRoom);                            // determine blackout based on room
            faintlampSystem.SetFaintState(faultyRoom);                              // determine faint lamp based on room
        }

        if(destination_room_id == "Basement") AnalyticsManager.Instance.LogMilestone("Milestone: Discovered Basement");
        if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.EnterRoom(destination_room_id);                    // ANALYTICS → room entry

        player_movement.EnableMovement();                                                                                   // re-enable movement after transition
        isTransition = false;
    }
}
