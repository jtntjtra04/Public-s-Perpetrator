using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 move_input;

    //References
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (DialogueManager.instance != null && DialogueManager.instance.dialoguebox_on)
        {
            return;
        }
        if (NotificationTrigger.instance != null && NotificationTrigger.instance.notifbox_active) 
        {
            return;
        }
        // Input movement
        move_input.x = Input.GetAxisRaw("Horizontal");
        move_input.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", move_input.x);
        anim.SetFloat("Vertical", move_input.y);

        if (move_input != Vector2.zero)
        {
            anim.SetFloat("HorizontalIdle", move_input.x);
            anim.SetFloat("VerticalIdle", move_input.y);
        }

        anim.SetFloat("Speed", move_input.magnitude);

    }
    private void FixedUpdate()
    {
        // Movement
        rb.MovePosition(move_input.normalized * speed * Time.fixedDeltaTime + rb.position);
    }
}
