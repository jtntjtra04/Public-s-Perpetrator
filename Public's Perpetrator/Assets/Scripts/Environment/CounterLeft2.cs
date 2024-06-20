using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterLeft2 : MonoBehaviour
{
    public InvestigationBoard investigation_board;
    private bool already_interact = false;
    public void InteractCounterLeft2()
    {
        if (!already_interact)
        {
            investigation_board.interact_counter_L_2 = true;
            already_interact = true;
        }
    }
}
