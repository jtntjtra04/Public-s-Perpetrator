using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public Item item;


    // =============================== //
    // Items & Puzzle Requirements     //
    // =============================== //


    public void PickUpItem()
    {
        if(item != null)
        {
            InventoryManager.Instance.AddItem(item);        // add item to inventory

            InvestigationBoard investigation_board = FindAnyObjectByType<InvestigationBoard>();

            if (investigation_board != null)
            {
                if (item.name == "Pocket Knife")                    // evidence 1
                {
                    Debug.Log("Pickup a knife");
                    investigation_board.have_knife = true;
                }

                else if (item.name == "Severed Finger")             // evidence 2
                {
                    Debug.Log("Pickup a finger");
                    investigation_board.have_finger = true;
                }

                else if (item.name == "Journalist's Certificate")   // evidence 3
                {
                    Debug.Log("Pickup a certificate");
                    investigation_board.have_certificate = true;
                }

                else if (item.name == "Poster")                     // optional item
                {
                    Debug.Log("Pickup a poster");
                }
            }

            Destroy(gameObject);                            // remove the item from map after pickup
        }

        else
        {
            Debug.Log("Item Not Available");
        }
    }
}
