using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mail : MonoBehaviour
{
    public GameObject mail;
    private bool can_open_mail = false;
    public bool on_mail = false;
    public bool opened_mail = false;


    private void Start()
    {
        CloseMail();        // the secret mail hasn't been opened
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && can_open_mail)
        {
            mail.SetActive(true);
            on_mail = true;
            opened_mail = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && on_mail)
        {
            CloseMail();
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


    public void CloseMail()
    {
        mail.SetActive(false);
        on_mail = false;
    }
}
