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
    public float isplaying = 0f;

    // Player Movement
    private PlayerMovement player_movement;

    //public static DialogueManager instance;

    private void Awake()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
        //instance = this;
    }
    private void Start()
    {
        lines = new Queue<string>();
        names = new Queue<string>();
        images = new Queue<Sprite>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && dialoguebox_on)
        {
            isplaying += 1;
            if (!dialogue_on)
            {
                NextDialogue();
            }
            if (isplaying >= 2)
            {
                text_speed = 0f;
                isplaying = 0;
            }
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        isplaying = 1;
        if (dialoguebox_on) return;
        dialogue_box.SetActive(true);
        dialoguebox_on = true;
        player_movement.DisableMovement();
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
        isplaying = 0;
    }
    public void EndDialogue()
    {
        Debug.Log("End dialogue");
        dialogue_box.SetActive(false);
        dialoguebox_on = false;
        player_movement.EnableMovement();

        NotificationTrigger trigger_notif = GetComponent<NotificationTrigger>();

        if (trigger_notif != null)
        {
            trigger_notif.ShowNotification();
        }

        // For Phone
        Phone nokia = GetComponent<Phone>();
        if (nokia != null)
        {
            nokia.ShowPhone();
        }

        // For Investigation Board
        CounterLeft1 counter_L_1 = GetComponent<CounterLeft1>();
        if(counter_L_1 != null)
        {
            counter_L_1.InteractCounterLeft1();
        }
        CounterLeft2 counter_L_2 = GetComponent<CounterLeft2>();
        if(counter_L_2 != null)
        {
            counter_L_2.InteractCounterLeft2();
        }
        CounterRight counter_R = GetComponent<CounterRight>();
        if(counter_R != null)
        {
            counter_R.InteractCounterRight();
        }
    }
}
