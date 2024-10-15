using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInteract : MonoBehaviour
{
    private CutsceneTrigger cutscene_trigger;
    private bool CanTriggerDialogue = false;
    private Cutscene cutscene_manager;

    private void Start()
    {
        cutscene_trigger = GetComponent<CutsceneTrigger>();
        cutscene_manager = GetComponent<Cutscene>();
        cutscene_trigger.TriggerCutscene();
    }
}
