using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterLeft1 : MonoBehaviour
{
    public InvestigationBoard investigation_board;
    private bool already_interact = false;
    public void InteractCounterLeft1()
    {
        if (!already_interact)
        {
            investigation_board.interact_counter_L = true;
            already_interact = true;
        }
    }
}
