using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]


public class DialogueChoice
{
    public bool hasChoice;


    [Header("Button Text")]
    public string option1_text;
    public string option2_text;


    [Header("Button Text")]
    public string option1_desc;
    public string option2_desc;


    [Header("Next Dialogue")]
    public DialogueTrigger option1_dialogue;
    public DialogueTrigger option2_dialogue;


    [Header("Choice Panel Paths")]
    public bool option1_cutscene;
    public CutsceneType option1_playscene;
    public bool option2_cutscene;
    public CutsceneType option2_playscene;

    public bool option1_worldscene;
    public WorldSceneType option1_playworld;
    public bool option2_worldscene;
    public WorldSceneType option2_playworld;
}
