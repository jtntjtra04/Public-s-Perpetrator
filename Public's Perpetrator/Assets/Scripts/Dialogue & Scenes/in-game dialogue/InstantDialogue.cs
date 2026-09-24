using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDialogue : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    public DialogueManager dialogue_manager;
    private HighlightObject object_status;


    [SerializeField]
    private string interaction_id;


    private void Start()
    {
        dialogue_trigger = GetComponent<DialogueTrigger>();
        object_status = GetComponent<HighlightObject>();

        if (string.IsNullOrWhiteSpace(interaction_id))
        {
            Debug.LogError(
                $"An interaction ID must be assigned to {gameObject.name}.", this);     // ID must be assigned to the object

            return;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanTriggerDialogue == false)
        {
            if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.RecordInteract(interaction_id);

            dialogue_trigger.TriggerDialogue();
            CanTriggerDialogue = true;

            if (object_status != null)
            {
                object_status.is_interacted = true;
            }
        }
    }
}