using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    public Notification notif;
    public GameObject notification_box;
    public TextMeshProUGUI notif_text;
    public bool notifbox_active = false;
    private bool notif_on = true;
    private float text_speed = 0.04f;
    AudioManager audionotif;

    // Player Movement References
    private PlayerMovement player_movement;

    public static NotificationTrigger instance;
    private void Awake()
    {
        instance = this;
        player_movement = FindAnyObjectByType<PlayerMovement>();
        audionotif = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void ShowNotification()
    {
        notification_box.SetActive(true);
        //notif_text.text = notif.line;
        notifbox_active = true;
        player_movement.DisableMovement();
        StartCoroutine(TypeLines(notif.line));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && notifbox_active)
        {
            if (notif_on)
            {
                text_speed = 0.04f;
            }
            else
            {
                EndNotification();
            }
        }
    }
    private IEnumerator TypeLines(string line)
    {
        audionotif.PlaySFX(audionotif.item);
        notif_text.text = "";
        foreach (char letter in line.ToCharArray())
        {
            notif_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }
        notif_on = false;
    }
    private void EndNotification()
    {
        notification_box.SetActive(false);
        notifbox_active = false;
        player_movement.EnableMovement();

        ItemController item_controller = GetComponent<ItemController>();
        item_controller.PickUpItem();
    }
}
