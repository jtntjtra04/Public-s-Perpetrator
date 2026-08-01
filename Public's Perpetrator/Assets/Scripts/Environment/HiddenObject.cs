using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HiddenObject : MonoBehaviour
{
    public GameObject hidden_object;
    public bool object_revealed = false;


    void Start()
    {
        hidden_object.SetActive(false);
    }


    public void RevealObject()
    {
        hidden_object.SetActive(true);
        object_revealed = true;
    }
}
