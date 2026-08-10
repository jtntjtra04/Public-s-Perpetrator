using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 move_input;
    public bool can_walk;

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Important Interactions")]
    public LampPuzzle lamp_puzzle;
    public LampPuzzle batteryinhand;
    public BoxCollider2D puzzlebox;
    public InvestigationBoard board;
    public Mail letter;

    AudioManager audiomanager;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    private bool wasMoving = false;


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

        move_input.x = Input.GetAxisRaw("Horizontal");              // input movement
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
        rb.MovePosition(move_input.normalized * speed * Time.fixedDeltaTime + rb.position);     // consistent movement update

        bool isMoving = move_input.magnitude > 0.1f;

        if (isMoving && !footstepSource.isPlaying)                                              // for footstep noises
        {
            footstepSource.pitch = 1.2f;
            footstepSource.PlayOneShot(audiomanager.walking_wood);
        }
        else if (!isMoving && wasMoving)
        {
            StartCoroutine(FadeOutFootsteps());
        }

        wasMoving = isMoving;
    }


    private IEnumerator FadeOutFootsteps(float duration = 0.15f)
    {
        float startVolume = 0.05f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            footstepSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        footstepSource.Stop();
        footstepSource.volume = startVolume; // reset for next play
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


    public void SetFacingDirection(float x, float y)
    {
        anim.SetFloat("HorizontalIdle", x);
        anim.SetFloat("VerticalIdle", y);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("batteries"))
        {
            batteryinhand.have_battery = true;
            puzzlebox.enabled = false;
        }
        if (collision.gameObject.CompareTag("door"))
        {
            audiomanager.PlaySFX(audiomanager.SFX_door, 1f, 0.2f);
        }
        if (collision.gameObject.CompareTag("crime scene"))
        {
            audiomanager.ChangeMusic(audiomanager.BGM_crime_scene);
            Destroy(collision);
        }
        if (collision.gameObject.CompareTag("basement"))
        {
            audiomanager.StopMusic();
            Destroy(collision);
        }
    }
}
