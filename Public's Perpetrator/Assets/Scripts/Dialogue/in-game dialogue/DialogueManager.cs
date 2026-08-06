using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;


public class DialogueManager : MonoBehaviour
{
    private Queue<string> lines;                // queue -> change per dialogue
    private Queue<string> names;
    private Queue<Sprite> images;

    [Header("Call to Dialogue Manager & Other References")]
    private GameObject current_source;
    public AudioManager type_audio;

    [Header("UI")]
    public GameObject dialogue_box;             // UI
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialogue_text;
    public Image npc_image;

    [Header("Dialogue Status")]
    public float text_speed = 0.04f;
    private bool dialogue_on = false;
    public bool dialoguebox_on = false;
    public float isplaying = 0f;

    [Header("Choice Event")]
    public GameObject choice_panel;
    public Button option_1_button;
    public Button option_2_button;
    public TextMeshProUGUI option_1_text;
    public TextMeshProUGUI option_2_text;
    public TextMeshProUGUI option_1_desc;
    public TextMeshProUGUI option_2_desc;
    private Dialogue current_dialogue;

    [Header("Player Detection")]
    private PlayerMovement player_movement;     // player movement


    private void Awake()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
        lines = new Queue<string>();
        names = new Queue<string>();
        images = new Queue<Sprite>();
    }


    // =============================== //
    // General Dialogue Player         //
    // =============================== //


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


    public void StartDialogue(Dialogue dialogue, GameObject source)
    {
        current_dialogue = dialogue;
        current_source = source;

        isplaying = 1;                      // in regards to faster text typing
        if (dialoguebox_on) return;
        dialogue_box.SetActive(true);
        dialoguebox_on = true;

        if (player_movement != null)
        {
            player_movement.DisableMovement();      // player no longer moves during a dialogue (not applicable during in-game cutscenes)
        }

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
            if (current_dialogue.choice != null && current_dialogue.choice.hasChoice)  // reveal choices if there are any
            {
                ShowChoices();
            }

            else
            {
                EndDialogue();                      // dialogue finishes if none
            }
            return;
        }

        string name = names.Dequeue();
        string line = lines.Dequeue();
        Sprite image = images.Dequeue();

        dialogue_on = true;
        name_text.text = name;
        npc_image.sprite = image;
        StopAllCoroutines();
        type_audio.PlayTypeSFX(type_audio.SFX_type_blip);
        StartCoroutine(TypeLines(line));
    }


    // =============================== //
    // Typewriter Effect               //
    // =============================== //


    private IEnumerator TypeLines(string sentence)
    {
        dialogue_text.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialogue_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }

        type_audio.StopTypeSFX();
        dialogue_on = false;
        isplaying = 0;
    }


    // =============================== //
    // When a Dialogue Ends            //
    // =============================== //


    private void ShowChoices()
    {
        if (choice_panel == null)
        {
            Debug.LogError("Choice Panel is not assigned!");
            EndDialogue();
            return;
        }

        dialogue_box.SetActive(false);
        choice_panel.SetActive(true);

        option_1_text.text = current_dialogue.choice.option_1_text;
        option_2_text.text = current_dialogue.choice.option_2_text;
        option_1_desc.text = current_dialogue.choice.option_1_desc;
        option_2_desc.text = current_dialogue.choice.option_2_desc;
    }


    public void ChooseOption1()
    {
        choice_panel.SetActive(false);
        dialoguebox_on = false;
        dialogue_box.SetActive(true);
        current_dialogue.choice.option_1_dialogue.TriggerDialogue();
    }


    public void ChooseOption2()
    {
        choice_panel.SetActive(false);
        dialoguebox_on = false;
        dialogue_box.SetActive(true);
        current_dialogue.choice.option_2_dialogue.TriggerDialogue();
    }


    public void EndDialogue()
    {
        dialogue_box.SetActive(false);
        dialoguebox_on = false;

        if (player_movement != null)
        {
            player_movement.EnableMovement();   // player can move again
        }

        NotificationTrigger trigger_notif = current_source.GetComponent<NotificationTrigger>();
        if (trigger_notif != null)
        {
            trigger_notif.ShowNotification();
        }

        HiddenObject phone = current_source.GetComponent<HiddenObject>();   // requirement to interact with phone
        if (phone != null)
        {
            phone.RevealObject();
        }

        InGameCutsceneDialogue in_game_cutscene = current_source.GetComponent<InGameCutsceneDialogue>();
        if (in_game_cutscene != null)
        {
            in_game_cutscene.ResumeTimeline();
        }
    }
}
