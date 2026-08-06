using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogue_Manager;


    private void Awake()
    {
        dialogue_Manager = FindAnyObjectByType<DialogueManager>();
    }


    public void TriggerDialogue()
    {
        dialogue_Manager.StartDialogue(dialogue, gameObject);
    }
}
