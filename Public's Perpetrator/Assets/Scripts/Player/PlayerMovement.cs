using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 move_input;
    public bool can_walk;

    //References
    private Rigidbody2D rb;
    private Animator anim;
    public LampPuzzle lamp_puzzle;
    public RoomSwitch stairs;
    public LampPuzzle batteryinhand;
    public BoxCollider2D puzzlebox;
    public InvestigationBoard board;
    public Mail letter;
    AudioManager audiomanager;

    private void Awake()
    {
        can_walk = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (!can_walk)
        {
            StopPlayer();
            return;
        }
        if (lamp_puzzle.on_puzzle)
        {
            StopPlayer();
            return;
        }
        if(board.on_board)
        {
            StopPlayer();
            return;
        }
        if (letter.on_mail)
        {
            StopPlayer();
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
    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        move_input = Vector2.zero;
        anim.SetFloat("Speed", 0f);
        anim.SetFloat("Horizontal", 0f);
        anim.SetFloat("Vertical", 0f);
    }
    public void DisableMovement()
    {
        can_walk = false;
    }
    public void EnableMovement()
    {
        can_walk = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Batteries"))
        {
            batteryinhand.have_battery = true;
            puzzlebox.enabled = false;
        }
        if (collision.gameObject.CompareTag("Door"))
        {
            audiomanager.PlaySFX(audiomanager.door);
        }
        if (collision.gameObject.CompareTag("Scene2"))
        {
            audiomanager.ChangeMusic(audiomanager.BGM2);
            Destroy(collision);
        }
        if (collision.gameObject.CompareTag("Scene3"))
        {
            audiomanager.ChangeMusic(audiomanager.BGM3);
            Destroy(collision);
        }
    }
}
