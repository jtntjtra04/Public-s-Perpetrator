using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LampPuzzle : MonoBehaviour
{
    // Toggle Switch
    public Toggle[] switches;

    // Switch Sprite
    public Sprite switch_off;
    public Sprite switch_on;

    // Lamps
    public Image[] lamps;

    // Lamp Sprite
    public Sprite lamp_on;
    public Sprite lamp_off;

    // Lamp Condition
    private bool[] lamp_states;

    private void Start()
    {
        lamp_states = new bool[7] { true, false, false, false, false, false, false};

        for(int i = 0; i < lamps.Length; i++)
        {
            lamps[i].sprite = lamp_states[i] ? lamp_on : lamp_off;
            switches[i].onValueChanged.AddListener(delegate { ToggleSwitch(i); });
        }
    }

    public void ToggleSwitch(int index)
    {

    }
}
