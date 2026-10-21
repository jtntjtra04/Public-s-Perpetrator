using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> items = new List<Item>();

    public GameObject inventory;
    public Transform item_content;
    public GameObject inventory_item;
    public PlayerMovement player;

    AudioManager audio_manager;
    public DialogueManager dialogue_manager;


    private void Awake()
    {
        Instance = this;
        audio_manager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    private void Start()
    {
        inventory.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !dialogue_manager.dialogue_ongoing)
        {
            if (!inventory.activeSelf)
            {
                audio_manager.PlaySFX(audio_manager.SFX_inventory_open, 2f, 0.15f);
                inventory.SetActive(true);
                InputItems();
                player.DisableMovement();
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
        player.EnableMovement();
        if (InventoryDesc.Instance != null) InventoryDesc.Instance.HideDesc();          // hide dewcription when inventory is closed
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
        foreach (Transform item in item_content) Destroy(item.gameObject);

        foreach (var item in items)
        {
            GameObject input_item = Instantiate(inventory_item, item_content);                              // item slot instantiation

            var item_icon = input_item.transform.Find("ItemImage").GetComponent<UnityEngine.UI.Image>();    // item icon
            item_icon.sprite = item.icon;

            var hoverScript = input_item.GetComponent<InventorySlotHover>();                                // hovering script for description
            if (hoverScript != null) hoverScript.SetupSlot(item.description);
        }
    }
}
