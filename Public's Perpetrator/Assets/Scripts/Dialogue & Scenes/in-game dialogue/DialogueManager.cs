using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
    public ObjectiveManager objective_manager;

    [Header("UI")]
    public GameObject dialogue_box;             // UI
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI dialogue_text;
    public Image npc_image;

    [Header("Dialogue Status")]
    public float text_speed = 0.04f;
    private bool dialogue_typing = false;
    public bool dialogue_ongoing = false;
    public float isplaying = 0f;

    [Header("Choice Event")]
    public GameObject choice_panel;
    public Button option1_button;
    public Button option2_button;
    public TextMeshProUGUI option1_text;
    public TextMeshProUGUI option2_text;
    public TextMeshProUGUI option1_desc;
    public TextMeshProUGUI option2_desc;
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
        if(Input.GetMouseButtonUp(0) && dialogue_ongoing)
        {
            isplaying += 1;
            if (!dialogue_typing)
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
        if (dialogue_ongoing) return;
        dialogue_box.SetActive(true);
        dialogue_ongoing = true;

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

        dialogue_typing = true;
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
        dialogue_typing = false;
        isplaying = 0;
    }


    // =============================== //
    // Choice Panel                    //
    // =============================== //


    private void ShowChoices()      // reveal choice panel and options
    {
        if (choice_panel == null)
        {
            Debug.LogError("Choice Panel is not assigned!");
            EndDialogue();
            return;
        }

        dialogue_box.SetActive(false);
        choice_panel.SetActive(true);

        option1_text.text = current_dialogue.choice.option1_text;
        option2_text.text = current_dialogue.choice.option2_text;
        option1_desc.text = current_dialogue.choice.option1_desc;
        option2_desc.text = current_dialogue.choice.option2_desc;
    }


    public void ChooseOption1()
    {
        if (current_dialogue.choice.option1_worldscene)
        {
            WorldSceneManager.world_scene_ToPlay = current_dialogue.choice.option1_playworld;
            WorldSceneManager.world_scene_pending = true;
        }

        if (current_dialogue.choice.option1_cutscene)                                      // refers to DialogueChoice.cs to check if the choice leads to a cutscene or not (checked if YES)
        {
            CutsceneLoader.cutscene_ToPlay = current_dialogue.choice.option1_playscene;    // calls cutscene loader and loads the cutscene_type to play according to inspector
        }

        choice_panel.SetActive(false);
        dialogue_ongoing = false;

        if (current_dialogue.choice.option1_dialogue != null)
        {
            dialogue_box.SetActive(true);
            current_dialogue.choice.option1_dialogue.TriggerDialogue();                    // trigger dialogue from object that contains the dialogue from StartDialogue() until EndDialogue()
        }
        else
        {
            if (WorldSceneManager.world_scene_pending)
            {
                WorldSceneManager.world_scene_pending = false;
                SceneManager.LoadScene("Ch 1 World Scenes");
            }
        }
    }


    public void ChooseOption2()
    {
        if (current_dialogue.choice.option2_worldscene)
        {
            WorldSceneManager.world_scene_ToPlay = current_dialogue.choice.option2_playworld;
            WorldSceneManager.world_scene_pending = true;
        }

        if (current_dialogue.choice.option2_cutscene)
        {
            CutsceneLoader.cutscene_ToPlay = current_dialogue.choice.option2_playscene;
        }

        choice_panel.SetActive(false);
        dialogue_ongoing = false;
        dialogue_box.SetActive(true);

        if (current_dialogue.choice.option2_dialogue != null)
        {
            current_dialogue.choice.option2_dialogue.TriggerDialogue();
        }
        else
        {
            if (WorldSceneManager.world_scene_pending)
            {
                WorldSceneManager.world_scene_pending = false;
                SceneManager.LoadScene("Ch 1 World Scenes");
            }
        }
    }


    // =============================== //
    // After the Dialogue Ends         //
    // =============================== //


    public void EndDialogue()
    {
        dialogue_box.SetActive(false);
        dialogue_ongoing = false;

        if (player_movement != null)                                                                        // player can MOVE again
        {
            player_movement.EnableMovement();   
        }

        NotificationTrigger trigger_notif = current_source.GetComponent<NotificationTrigger>();             // NOTIFICATION pop ups after certain interactions
        if (trigger_notif != null)
        {
            trigger_notif.ShowNotification();
        }

        HiddenObject phone = current_source.GetComponent<HiddenObject>();                                   // requirement to interact with HIDDEN OBJECTS (e.g. phone)
        if (phone != null)
        {
            phone.RevealObject();
        }

        WorldSceneDialogue world_scene = current_source.GetComponent<WorldSceneDialogue>();                 // timeline continues after dialogue plays (during in-game WORLD SCENE)
        if (world_scene != null)
        {
            world_scene.ResumeTimeline();
        }

        DialogueTrigger object_trigger = current_source.GetComponent<DialogueTrigger>();                    // plays a CUTSCENE if there is any.
        if (object_trigger != null && object_trigger.cutscene_AfterDialogue)
        {
            SceneManager.LoadScene("Ch 1 Cutscenes");
        }

        if (WorldSceneManager.world_scene_pending)                                                          // plays a WORLD SCENE if there is any
        {
            WorldSceneManager.world_scene_pending = false;
            SceneManager.LoadScene("Ch 1 World Scenes");
        }

        if (current_dialogue.objective_exists)                                                              // sets the next OBJECTIVE if there is any
        {
            objective_manager.SetObjective(current_dialogue.objective_type);
        }

        Mail mail = current_source.GetComponent<Mail>();                                                    // ONLY for Mail object
        if(mail != null)
        {
            mail.mail_read = true;
            mail.OpenMail();
        }
    }
}