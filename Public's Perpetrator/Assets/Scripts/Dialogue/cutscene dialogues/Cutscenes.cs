using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Cutscenes
{
    public string[] cutscene_name;
    [TextArea(3, 10)]
    public string[] cutscene_lines;
    public Sprite[] cutscene_BG;
    public Sprite[] cutscene_character;
}
