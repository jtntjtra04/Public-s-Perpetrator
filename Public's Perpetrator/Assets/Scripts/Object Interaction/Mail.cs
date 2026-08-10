using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mail : MonoBehaviour
{
    public GameObject mail;
    private bool can_open_mail = false;
    public bool on_mail = false;
    public bool opened_mail = false;
    public bool dialogue_played = false;
    public bool mail_read = false;
    public DialogueTrigger dialogue_after;
    public AudioManager audio_manager;

    public float fade_duration = 0.3f;
    public CanvasGroup mail_canvas;


    private void Start()
    {
        CloseMail();        // the secret mail hasn't been opened
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && can_open_mail)
        {
            if (dialogue_played == false)
            {
                dialogue_after.TriggerDialogue();
                dialogue_played = true;
            }

            OpenMail();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_open_mail = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_open_mail = false;
        }
    }


    public void OpenMail()
    {
        if (mail_read == true)
        {
            audio_manager.PlaySFX(audio_manager.SFX_paper, 1.3f, 0.7f);
            StartCoroutine(FadeIn());
            mail.SetActive(true);
            on_mail = true;
            opened_mail = true;
        }
    }


    public void CloseMail()
    {
        mail.SetActive(false);
        on_mail = false;
    }


    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fade_duration)
        {
            timer += Time.deltaTime;
            mail_canvas.alpha = Mathf.Lerp(0f, 1f, timer / fade_duration);
            yield return null;
        }

        mail_canvas.alpha = 1f;
    }
}
