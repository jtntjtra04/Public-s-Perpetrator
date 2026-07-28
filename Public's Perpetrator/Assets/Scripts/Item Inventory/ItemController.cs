using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;

    public void PickUpItem()
    {
        if(item != null)
        {
            InventoryManager.Instance.AddItem(item);

            InvestigationBoard investigation_board = FindAnyObjectByType<InvestigationBoard>();

            if (investigation_board != null)
            {
                if (item.name == "Pocket Knife")
                {
                    Debug.Log("Pickup a knife");
                    investigation_board.have_knife = true;
                }
                else if (item.name == "Severed Finger")
                {
                    Debug.Log("Pickup a finger");
                    investigation_board.have_finger = true;
                }
                else if (item.name == "Journalist's Certificate")
                {
                    Debug.Log("Pickup a certificate");
                    investigation_board.have_certificate = true;
                }
                else if (item.name == "Poster")
                {
                    Debug.Log("Pickup a poster");
                    investigation_board.have_poster = true;
                }
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Item Not Available");
        }
        
    }
    private void Update()
    {
        
    }
}
