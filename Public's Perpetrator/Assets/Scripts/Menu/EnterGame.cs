using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGame : MonoBehaviour
{
    public Animator transition_anim;

    public void Start()
    {
        transition_anim.Play("EndFade");
    }

}
