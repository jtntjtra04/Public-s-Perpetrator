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
