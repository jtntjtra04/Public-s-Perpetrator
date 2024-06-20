using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    // References
    public InvestigationBoard investigation_board;
    public BoxCollider2D board_collider;
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
                board_collider.enabled = false;
                item_completed = true;
            }
        }
    }
}
