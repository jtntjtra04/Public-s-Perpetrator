using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CutsceneManager;


[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscene/Cutscene Data")]


public class CutsceneData : ScriptableObject
{
    public CutsceneType cutscene_type;

    public string[] cutscene_name;

    [TextArea(3, 10)]

    public string[] cutscene_lines;
    public Sprite[] cutscene_BG;
    public Sprite[] cutscene_character;
}
