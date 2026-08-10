using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class WorldSceneDialogue : MonoBehaviour
{
    public Dialogue world_dialogue;
    public DialogueManager world_dialogue_manager;
    public PlayableDirector director;


    public void PlayDialogue()
    {
        director.Pause();
        world_dialogue_manager.StartDialogue(world_dialogue, gameObject);
    }


    public void ResumeTimeline()
    {
        director.Resume();                         // Continue the timeline / cutscene
    }
}
