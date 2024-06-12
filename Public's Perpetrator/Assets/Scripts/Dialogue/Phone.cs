using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public GameObject phone;
    private bool condi = false;
    public bool phoneout = false;
    void Start()
    {
        phone.SetActive(false);
    }

    public void ShowPhone()
    {
        phone.SetActive(true);
        phoneout = true;
    }
}
