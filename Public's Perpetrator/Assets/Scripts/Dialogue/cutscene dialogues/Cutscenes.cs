using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CutsceneManager;


[System.Serializable]


public class Cutscenes
{
    public string[] cutscene_name;
    [TextArea(3, 10)]
    public string[] cutscene_lines;
    public Sprite[] cutscene_BG;
    public Sprite[] cutscene_character;

    public CutsceneType cutscene_type;
}
