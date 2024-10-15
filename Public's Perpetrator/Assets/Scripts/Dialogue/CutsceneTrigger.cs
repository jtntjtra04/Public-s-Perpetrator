using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Cutscenes cutscene;

    public void TriggerCutscene()
    {
        GetComponent<Cutscene>().StartCutscene(cutscene);
    }
}
