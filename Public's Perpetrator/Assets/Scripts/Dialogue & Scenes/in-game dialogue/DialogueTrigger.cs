using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogue_Manager;
    public bool cutscene_AfterDialogue;        // mainly to trigger a cutscene after SPECIFIC dialogues


    private void Awake()
    {
        dialogue_Manager = FindAnyObjectByType<DialogueManager>();
    }


    public void TriggerDialogue()
    {
        dialogue_Manager.StartDialogue(dialogue, gameObject);
    }
}
