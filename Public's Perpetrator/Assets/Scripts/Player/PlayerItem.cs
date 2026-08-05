using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItem : MonoBehaviour
{
    public InvestigationBoard investigation_board;
    private bool item_completed = false;


    private void Start()
    {
        item_completed = false;
    }


    private void Update()
    {
        if(!item_completed)
        {
            if(investigation_board.CompletedItems())
            {
                Debug.Log("Completed all items");
                item_completed = true;
            }
        }
    }
}
