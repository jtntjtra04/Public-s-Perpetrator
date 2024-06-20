using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRight : MonoBehaviour
{
    public InvestigationBoard investigation_board;
    private bool already_interact = false;
    public void InteractCounterRight()
    {
        if (!already_interact)
        {
            investigation_board.interact_counter_R = true;
            already_interact = true;
        }
    }
}
