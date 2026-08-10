using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum ObjectiveType
{
    None,
    LookAround,
    InvestigateScene,
    FinalDeduction
}


public class ObjectiveManager : MonoBehaviour
{
    public ObjectiveType objective_current;
    public bool objective_complete;
    public TextMeshProUGUI objective_text;
    public TextMeshProUGUI objective_desc;
    public DialogueManager dialogue_manager;

    public CanvasGroup objective_canvas;
    public float fade_duration = 5f;
    private Coroutine fade_coroutine;


    public void SetObjective(ObjectiveType objective)
    {
        objective_complete = false;

        switch (objective)
        {
            case ObjectiveType.None:
                objective_text.text = "";
                objective_desc.text = "";
                break;

            case ObjectiveType.LookAround:

                if (!dialogue_manager.dialoguebox_on)
                {
                    objective_text.text = "Look around the house.";
                    objective_desc.text = "Something is off since I stepped foot into this house, it's way too quiet..";
                }
                if (fade_coroutine != null)
                    StopCoroutine(fade_coroutine);
                fade_coroutine = StartCoroutine(FadeInAndOut());
                break;

            case ObjectiveType.InvestigateScene:
                if (!dialogue_manager.dialoguebox_on)
                {
                    objective_text.text = "Further investigate the 2nd floor.";
                    objective_desc.text = "I don't know who's blood this belongs to. Hopefully it isn't the stranger's..";
                }
                if (fade_coroutine != null)
                    StopCoroutine(fade_coroutine);
                fade_coroutine = StartCoroutine(FadeInAndOut());
                break;

            case ObjectiveType.FinalDeduction:
                if (!dialogue_manager.dialoguebox_on)
                {
                    objective_text.text = "Deduce what happened.";
                    objective_desc.text = "There must be some leftover evidence that I can use to connect the dots..";
                }
                if (fade_coroutine != null)
                    StopCoroutine(fade_coroutine);
                fade_coroutine = StartCoroutine(FadeInAndOut());
                break;
        }
    }


    public void CompleteObjective()
    {
        objective_complete = true;

        if (fade_coroutine != null)
            StopCoroutine(fade_coroutine);

        fade_coroutine = StartCoroutine(FadeOut());
    }


    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fade_duration)
        {
            timer += Time.deltaTime;
            objective_canvas.alpha = Mathf.Lerp(0f, 1f, timer / fade_duration);
            yield return null;
        }

        objective_canvas.alpha = 1f;
    }


    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fade_duration)
        {
            timer += Time.deltaTime;
            objective_canvas.alpha = Mathf.Lerp(1f, 0f, timer / fade_duration);
            yield return null;
        }

        objective_canvas.alpha = 0f;
    }


    IEnumerator FadeInAndOut()
    {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(15f);
        StartCoroutine(FadeOut());
    }
}
