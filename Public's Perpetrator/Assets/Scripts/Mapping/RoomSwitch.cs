using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    public Transform destination;
    public bool isTransition = false;
    public Animator transition_anim;
    private PlayerMovement player_movement;

    private void Awake()
    {
        player_movement = FindAnyObjectByType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Vector2.Distance(collision.transform.position, transform.position) > 0.3f)
            {
                 StartCoroutine(TransitionRoom(collision));
            } 
         }
    }

    private IEnumerator TransitionRoom(Collider2D collision)
    {
        isTransition = true;
        player_movement.DisableMovement();
        transition_anim.Play("StartFade");
        yield return new WaitForSeconds(1.5f);
        collision.transform.position = new Vector2(destination.position.x, destination.position.y);
        transition_anim.Play("EndFade");
        player_movement.EnableMovement();
        isTransition = false;
    }
}
