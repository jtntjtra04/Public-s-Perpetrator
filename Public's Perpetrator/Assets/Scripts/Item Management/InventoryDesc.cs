using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryDesc : MonoBehaviour
{
    public static InventoryDesc Instance;

    public GameObject desc_BG;              // The UI panel container
    public Text desc_text;                  // The description in the panel


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        HideDesc();
    }


    private void Update()
    {
        if (desc_BG.activeSelf)
        {
            Vector2 mousePosition = Input.mousePosition;                            // description panel follows mouse
            desc_BG.transform.position = mousePosition + new Vector2(7f, -7f);    // slight offsset
        }
    }


    public void ShowDesc(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;     // doesn't show if there's no description.

        desc_text.text = description;
        desc_BG.SetActive(true);
    }


    public void HideDesc()
    {
        desc_BG.SetActive(false);                               // hide panel
    }
}

