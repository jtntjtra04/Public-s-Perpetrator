using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InventorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string item_desc;


    public void SetupSlot(string description)       // called from InventorySlot.cs to set the description for the item in this slot
    {
        item_desc = description;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrWhiteSpace(item_desc)) InventoryDesc.Instance.ShowDesc(item_desc);     // show deescription when hovering over item
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryDesc.Instance.HideDesc();                                                         // dismisses description if out of bounds
    }


    private void OnDisable()
    {
        if (InventoryDesc.Instance != null)InventoryDesc.Instance.HideDesc();                      // safeguard incase of inventory closing while hovering over item
    }
}

