using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mikewazowski : MonoBehaviour
{
    private DialogueTrigger dialogue_trigger;
    private bool CanTriggerDialogue = false;
    private void Start()
    {
        dialogue_trigger = GetComponent<DialogueTrigger>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanTriggerDialogue)
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
