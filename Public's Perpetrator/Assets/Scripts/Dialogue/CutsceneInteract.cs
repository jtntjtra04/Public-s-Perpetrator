using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutsceneInteract : MonoBehaviour
{
    private CutsceneTrigger cutscene_trigger;
    private CutsceneManager cutscene_manager;


    private void Start()
    {
        cutscene_trigger = GetComponent<CutsceneTrigger>();
        cutscene_manager = GetComponent<CutsceneManager>();
        cutscene_trigger.TriggerCutscene();
    }
}
