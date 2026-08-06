using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public enum CutsceneType
{
    Prologue,
    Epilogue_ending_1
}


public class CutsceneManager : MonoBehaviour
{
    [Header("Cutscene components")]
    private Queue<string> lines_queue;
    private Queue<string> names_queue;
    private Queue<Sprite> background_queue;
    private Queue<Sprite> character_queue;

    [Header("Cutscene component file types")]
    public GameObject cutscene_box;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI cutscene_text;
    public Image BG_image;
    public Image character_image;

    [Header("Transitions & audio")]
    public Animator transition_fade;
    public Animator transition_character;
    public SceneAudioManager scene_audio;

    [Header("Variables")]
    public float text_speed = 0.04f;
    private bool cutscene_on = false;
    public bool cutscenebox_on = false;
    public float isplaying = 0f;
    public int scenes = 0;
    private bool skipped = false;

    [Header("Cutscene data")]
    private CutsceneType current_cutscene_type;
    public Cutscenes prologue;
    public Cutscenes epilogue_ending1;


    // =============================== //
    // General Cutscene Reel           //
    // =============================== //


    private void Start()
    {
        lines_queue = new Queue<string>();
        names_queue = new Queue<string>();
        background_queue = new Queue<Sprite>();
        character_queue = new Queue<Sprite>();

        switch (CutsceneLoader.cutscene_ToPlay)
        {
            case CutsceneType.Prologue:
                StartCutscene(prologue);
                break;


            case CutsceneType.Epilogue_ending_1:
                StartCutscene(epilogue_ending1);
                break;

            default:
                Debug.LogError("No cutscene selected!");
                break;
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && cutscenebox_on)
        {
            isplaying += 1;

            if (!cutscene_on)
            {
                NextCutscene();
            }

            if (isplaying >= 2)     // speed up the dialogue if clicked again
            {
                text_speed = 0f;
                isplaying = 0;
            }
        }
    }


    public void StartCutscene(Cutscenes cutscene)
    {
        scenes = 0;                                                             // reset scene counter everytime a new specific cutscene plays
        current_cutscene_type = cutscene.cutscene_type;                        

        switch (cutscene.cutscene_type)                                         // beginning music of the cutscene event
        {
            case CutsceneType.Prologue:
                scene_audio.ChangeSceneMusic(scene_audio.BGM_mysterious);
                break;

            case CutsceneType.Epilogue_ending_1:
                break;
        }

        Debug.Log($"Begin cutscene {current_cutscene_type}");

        transition_fade.Play("EndFade");
        if (cutscenebox_on) return;
        cutscene_box.SetActive(true);
        cutscenebox_on = true;

        names_queue.Clear();
        lines_queue.Clear();
        background_queue.Clear();
        character_queue.Clear();


        foreach (string name in cutscene.cutscene_name)
        {
            names_queue.Enqueue(name);
        }

        foreach (string line in cutscene.cutscene_lines)
        {
            lines_queue.Enqueue(line);
        }

        foreach (Sprite background in cutscene.cutscene_BG)
        {
            background_queue.Enqueue(background);
        }

        foreach (Sprite character in cutscene.cutscene_character)
        {
            character_queue.Enqueue(character);
        }

        NextCutscene();
    }


    public void NextCutscene()
    {
        Debug.Log("Next scene playing");    // keep count of dialogues
        scenes++;

        HandleSceneEvents(current_cutscene_type, scenes);

        text_speed = 0.02f;

        if (lines_queue.Count == 0)
        {
            scene_audio.PlaySceneSFX(scene_audio.SFX_opendoor);
            EndCutscene();
            return;
        }

        string name = names_queue.Dequeue();
        string line = lines_queue.Dequeue();
        Sprite background = background_queue.Dequeue();
        Sprite character = character_queue.Dequeue();

        cutscene_on = true;
        name_text.text = name;
        HandleSceneFade(current_cutscene_type, scenes, background, character);
        scene_audio.PlayTypeSFX(scene_audio.SFX_type_blip);
        StartCoroutine(TypeLines(line));
    }


    private void HandleSceneFade(CutsceneType current_cutscene_type, int scenes, Sprite background, Sprite character)
    {
        switch (current_cutscene_type)
        {
            case CutsceneType.Prologue:

                switch (scenes)
                {
                    case 8: StartCoroutine(BGFade(background)); character_image.sprite = character; break;
                    case 10: BG_image.sprite = background; character_image.sprite = character; transition_character.Play("slide in"); break;
                    case 31: BG_image.sprite = background; character_image.sprite = character; transition_character.Play("slide in"); break;
                    case 33: BG_image.sprite = background; character_image.sprite = character; transition_character.Play("slide out"); break;
                    case 35: BG_image.sprite = background; character_image.sprite = character; transition_character.Play("slide in"); break;
                    case 44: StartCoroutine(BGFade(background)); character_image.sprite = character; break;
                    default: BG_image.sprite = background; character_image.sprite = character; break;
                }

                break;
        }
    }


    private void HandleSceneEvents(CutsceneType current_cutscene_type, int scenes)
    {
        switch (current_cutscene_type)
        {
            case CutsceneType.Prologue:

                switch (scenes)
                {
                case 6: scene_audio.FadeOutSceneMusic(); break;
                case 7: scene_audio.FadeInSceneMusic(scene_audio.BGM_relaxed); break;
                case 22: scene_audio.FadeOutSceneMusic(); break;
                case 24: scene_audio.PlaySceneSFX(scene_audio.SFX_notification); break;
                case 26: scene_audio.PlaySceneSFX(scene_audio.SFX_click); break;
                case 37: scene_audio.PlaySceneSFX(scene_audio.SFX_carkeys); break;
                case 42: scene_audio.FadeInSceneMusic(scene_audio.BGM_nighttime); break;
                case 43: scene_audio.PlaySceneSFX(scene_audio.SFX_closecardoor); break;
                case 45: scene_audio.PlaySceneSFX(scene_audio.SFX_doorknock); break;
                case 48: scene_audio.PlaySceneSFX(scene_audio.SFX_doorknock); break;
                }

                break;
        }
    }


    IEnumerator BGFade(Sprite background)
    {
        BG_image.CrossFadeAlpha(0f, 0.2f, false);

        yield return new WaitForSeconds(0.3f);

        BG_image.sprite = background;

        BG_image.CrossFadeAlpha(1f, 0.2f, false);
    }


    // =============================== //
    // Cutscene Customization          //
    // =============================== //


    private IEnumerator TypeLines(string sentence)
    {
        cutscene_text.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            cutscene_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }

        scene_audio.StopTypeSFX();
        cutscene_on = false;
        isplaying = 0;
    }


    // =============================== //
    // Proceeding to the Game          //
    // =============================== //


    public void SkipCutscene()
    {
        if (skipped) return;
        skipped = true;

        StopAllCoroutines();

        transition_character.Rebind();
        transition_character.Update(0f);

        cutscene_on = false;
        cutscenebox_on = false;

        names_queue.Clear();
        lines_queue.Clear();
        background_queue.Clear();
        character_queue.Clear();

        EndCutscene();
    }


    private IEnumerator NextPart()
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");

        cutscene_box.SetActive(false);
        cutscene_text.text = "";
        name_text.text = "";
        character_image.enabled = false;
        scene_audio.FadeOutSceneMusic();

        yield return new WaitForSeconds(1f);
        
        switch (current_cutscene_type)
        {
            case CutsceneType.Prologue:
                SceneManager.LoadScene("Chapter 1");
                break;

            case CutsceneType.Epilogue_ending_1:
                SceneManager.LoadScene("Ch 1 Ending");
                break;
        }

        transition_fade.Play("EndFade");
    }


    public void EndCutscene()
    {
        StartCoroutine(NextPart());
    }
}
