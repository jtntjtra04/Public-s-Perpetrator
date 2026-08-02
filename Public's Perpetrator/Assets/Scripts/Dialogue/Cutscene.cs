using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Cutscene : MonoBehaviour
{
    [Header("Cutscene components")]
    private Queue<string> lines_queue;
    private Queue<string> names_queue;
    private Queue<Sprite> background_queue;
    private Queue<Sprite> character_queue;

    [Header("Variables")]
    public GameObject cutscene_box;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI cutscene_text;
    public Image BG_image;
    public Image character_image;

    public Animator transition_fade;
    public SceneAudioManager scene_audio;

    public float text_speed = 0.04f;
    private bool cutscene_on = false;
    public bool cutscenebox_on = false;
    public float isplaying = 0f;
    public float scenes = 0f;


    // =============================== //


    private void Start()
    {
        scene_audio.ChangeSceneMusic(scene_audio.BGM_mysterious);
        lines_queue = new Queue<string>();
        names_queue = new Queue<string>();
        background_queue = new Queue<Sprite>();
        character_queue = new Queue<Sprite>();
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
        Debug.Log("Begin cutscene");
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

        if (scenes == 5)
        {
            scene_audio.ChangeSceneMusic(scene_audio.BGM_relaxed);       
        }

        if (scenes == 16)
        {
            scene_audio.StopSceneMusic();
        }

        if (scenes == 17)
        {
            scene_audio.PlaySceneSFX(scene_audio.SFX_notification);
        }

        if (scenes == 19)
        {
            scene_audio.PlaySceneSFX(scene_audio.SFX_click);
        }

        if (scenes == 27)
        {
            scene_audio.ChangeSceneMusic(scene_audio.BGM_nighttime);
        }

        if (scenes == 29 || scenes == 30)
        {
            scene_audio.PlaySceneSFX(scene_audio.SFX_doorknock);
        }

        text_speed = 0.02f;

        if (lines_queue.Count == 0)
        {
            EndCutscene();
            return;
        }

        string name = names_queue.Dequeue();
        string line = lines_queue.Dequeue();
        Sprite background = background_queue.Dequeue();
        Sprite character = character_queue.Dequeue();

        cutscene_on = true;
        name_text.text = name;
        BG_image.sprite = background;
        character_image.sprite = character;

        StopAllCoroutines();
        StartCoroutine(TypeLines(line));
    }


    private IEnumerator TypeLines(string sentence)
    {
        cutscene_text.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            cutscene_text.text += letter;
            yield return new WaitForSeconds(text_speed);
        }

        cutscene_on = false;
        isplaying = 0;
    }


    private IEnumerator NextPart()
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");

        cutscene_box.SetActive(false);
        cutscene_text.text = "";
        name_text.text = "";
        character_image.enabled = false;
        scene_audio.PlaySceneSFX(scene_audio.SFX_opendoor);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transition_fade.Play("EndFade");
    }


    public void EndCutscene()
    {
        StartCoroutine(NextPart());
    }
}
