using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]


public class DialogueChoice
{
    public bool hasChoice;


    [Header("Button Text")]
    public string option_1_text;
    public string option_2_text;


    [Header("Next Dialogue")]
    public DialogueTrigger option_1_dialogue;
    public DialogueTrigger option_2_dialogue;
}
