using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //Queue -> Change per dialogue
    private Queue<string> lines;
    private Queue<string> names;
    private Queue<Sprite> images;

    //UI
    public GameObject dialogue_box;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialogue_text;
    public Image npc_image;

    public float text_speed = 0.04f;
    private bool dialogue_on = false;
    public bool dialoguebox_on = false;

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        lines = new Queue<string>();
        names = new Queue<string>();
        images = new Queue<Sprite>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && dialoguebox_on)
        {
            text_speed = 0f;

            if (!dialogue_on)
            {
                NextDialogue();
            }
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        dialogue_box.SetActive(true);
        dialoguebox_on = true;
        //name_text.text = dialogue.name;
        //npc_image.sprite = dialogue.image;

        names.Clear();
        lines.Clear();
        images.Clear();

        foreach(string name in dialogue.names)
        {
            names.Enqueue(name);
        }
        foreach(string line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        foreach(Sprite image in dialogue.images)
        {
            images.Enqueue(image);
        }
        NextDialogue();
    }
    public void NextDialogue()
    {
        text_speed = 0.05f;
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        string name = names.Dequeue();
        string line = lines.Dequeue();
        Sprite image = images.Dequeue();

        dialogue_on = true;
        name_text.text = name;
        npc_image.sprite = image;
        StopAllCoroutines();
        StartCoroutine(TypeLines(line));
    }
    private IEnumerator TypeLines(string sentence)
    {
        dialogue_text.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogue_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }
        dialogue_on = false;
    }
    public void EndDialogue()
    {
        Debug.Log("End dialogue");
        dialogue_box.SetActive(false);
        dialoguebox_on = false;

        NotificationTrigger trigger_notif = GetComponent<NotificationTrigger>();

        if (trigger_notif != null)
        {
            trigger_notif.ShowNotification();
        }
    }
}
