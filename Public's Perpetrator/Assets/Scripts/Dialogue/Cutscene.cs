using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    //Queue -> Change per dialogue
    private Queue<string> lines2;
    private Queue<string> names2;
    private Queue<Sprite> images2;
    private Queue<Sprite> images3;

    //UI
    public GameObject cutscene_box;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI cutscene_text;
    public Image BG_image2;
    public Image npc_image2;

    public Animator transition_fade;
    public MenuAudioManager menuaudiomanager;

    public float text_speed = 0.04f;
    private bool cutscene_on = false;
    public bool cutscenebox_on = false;
    public float isplaying = 0f;
    public float scenes = 0f;

    private void Start()
    {
        lines2 = new Queue<string>();
        names2 = new Queue<string>();
        images2 = new Queue<Sprite>();
        images3 = new Queue<Sprite>();
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
            if (isplaying >= 2)
            {
                text_speed = 0f;
                isplaying = 0;
            }
        }
    }
    public void StartCutscene(Cutscenes cutscene)
    {
        if (cutscenebox_on) return;
        cutscene_box.SetActive(true);
        cutscenebox_on = true;
        //name_text.text = dialogue.name;
        //npc_image.sprite = dialogue.image;

        names2.Clear();
        lines2.Clear();
        images2.Clear();
        images3.Clear();


        foreach (string name in cutscene.names2)
        {
            names2.Enqueue(name);
        }
        foreach (string line in cutscene.lines2)
        {
            lines2.Enqueue(line);
        }
        foreach (Sprite image in cutscene.images2)
        {
            images2.Enqueue(image);
        }
        foreach (Sprite npc in cutscene.images3)
        {
            images3.Enqueue(npc);
        }
        NextCutscene();

    }
    public void NextCutscene()
    {
        Debug.Log(scenes);
        scenes++;
        if (scenes == 5)
        {
            menuaudiomanager.ChangeMenuMusic(menuaudiomanager.BGMScene1);
        }
        if (scenes == 16)
        {
            menuaudiomanager.StopMusic();
        }
        if (scenes == 17)
        {
            menuaudiomanager.PlayMenuSFX(menuaudiomanager.Xtra);
        }
        if (scenes == 19)
        {
            menuaudiomanager.PlayMenuSFX(menuaudiomanager.Xtra2);
        }
        if (scenes == 27)
        {
            menuaudiomanager.ChangeMenuMusic(menuaudiomanager.BGMScene2);
        }
        if (scenes == 29 || scenes == 30)
        {
            menuaudiomanager.PlayMenuSFX(menuaudiomanager.Xtra3);
        }
        text_speed = 0.02f;

        if (lines2.Count == 0)
        {
            EndCutscene();
            return;
        }
        string name = names2.Dequeue();
        string line = lines2.Dequeue();
        Sprite image = images2.Dequeue();
        Sprite npc = images3.Dequeue();

        cutscene_on = true;
        name_text.text = name;
        BG_image2.sprite = image;
        npc_image2.sprite = npc;


        StopAllCoroutines();
        menuaudiomanager.PlayBackNoise(menuaudiomanager.BGMScene3);
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
        menuaudiomanager.StopBackNoise();
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
        npc_image2.enabled = false;
        menuaudiomanager.PlayMenuSFX(menuaudiomanager.ButtonPress);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transition_fade.Play("EndFade");
    }

    public void EndCutscene()
    {
        StartCoroutine(NextPart());
    }
}
