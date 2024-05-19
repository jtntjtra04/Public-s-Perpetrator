using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDialogue : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    public DialogueManager dialogue_manager;
    private void Start()
    {
        dialogue_trigger = GetComponent<DialogueTrigger>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue && dialogue_manager.dialoguebox_on == false)
        {
            dialogue_trigger.TriggerDialogue();
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
