using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDialogue : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    public DialogueManager dialogue_manager;
    private void Start()
    {
        dialogue_trigger = GetComponent<DialogueTrigger>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanTriggerDialogue == false)
        {
            dialogue_trigger.TriggerDialogue();
            CanTriggerDialogue = true;
        }
    }
}