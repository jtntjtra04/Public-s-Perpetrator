using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();

    public GameObject inventory;
    public Transform item_content;
    public GameObject inventory_item;
    public bool inventory_active = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        inventory.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!inventory_active)
            {
                inventory.SetActive(true);
                InputItems();
                inventory_active = true;
            }
            else
            {
                CloseInventory();
            }
            
        }
    }
    public void CloseInventory()
    {
        inventory.SetActive(false);
        inventory_active = false;
    }
    public void AddItem(Item item)
    {
        items.Add(item);
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }
    public void InputItems()
    {
        foreach (Transform item in item_content)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in items)
        {
            GameObject input_item = Instantiate(inventory_item, item_content);
            var item_icon = input_item.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>();

            item_icon.sprite = item.icon;
        }
    }
}
