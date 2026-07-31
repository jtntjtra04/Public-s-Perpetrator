using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterGame : MonoBehaviour
{
    public Animator transition_anim;
    public PlayerMovement player_movement;


    public void Start()
    {
        transition_anim.Play("EndFade");
        player_movement.SetFacingDirection(1f, 0f);     // facing right, only at game launch
    }

}
