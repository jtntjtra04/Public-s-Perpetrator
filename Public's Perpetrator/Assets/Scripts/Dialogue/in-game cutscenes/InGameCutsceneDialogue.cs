using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class InGameCutsceneDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    public PlayableDirector director;


    public void PlayDialogue()
    {
        director.Pause();
        dialogueManager.StartDialogue(dialogue, gameObject);
    }


    public void ResumeTimeline()
    {
        director.Resume();                         // Continue the timeline / cutscene
    }
}
