using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractDialogue : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    private DialogueManager dialogue_manager;
    private HighlightObject object_status;


    private void Start()
    {
        dialogue_trigger = this.gameObject.GetComponent<DialogueTrigger>();
        object_status = this.gameObject.GetComponent<HighlightObject>();
        dialogue_manager = FindFirstObjectByType<DialogueManager>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTriggerDialogue && dialogue_manager.dialoguebox_on == false)
        {
            dialogue_trigger.TriggerDialogue();
            if (object_status != null)
            {
                object_status.is_interacted = true;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.CompareTag("Player"))
        {
            CanTriggerDialogue = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CanTriggerDialogue = false;
        }
    }
}
