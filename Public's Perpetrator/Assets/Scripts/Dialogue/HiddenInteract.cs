using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenInteract : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    private DialogueManager dialogue_manager;
    private HiddenObject phone;
    private HighlightObject object_status;


    private void Start()
    {
        dialogue_trigger = GetComponent<DialogueTrigger>();
        dialogue_manager = GetComponent<DialogueManager>();
        object_status = GetComponent<HighlightObject>();
    }


    private void Awake()
    {
        phone = GetComponent<HiddenObject>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanTriggerDialogue && dialogue_manager.dialoguebox_on == false && !phone.object_revealed)
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
