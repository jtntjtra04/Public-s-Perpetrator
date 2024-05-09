using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Dialogue
{
    public string[] names;
    [TextArea(3, 10)]
    public string[] lines;
    public Sprite[] images;
}
