using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutsceneTrigger : MonoBehaviour
{
    public CutsceneData cutscene;


    public void TriggerCutscene()
    {
        GetComponent<CutsceneManager>().StartCutscene(cutscene);
    }
}
