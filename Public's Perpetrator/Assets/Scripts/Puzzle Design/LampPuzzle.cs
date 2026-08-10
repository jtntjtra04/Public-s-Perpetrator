using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class LampPuzzle : MonoBehaviour
{
    public GameObject lamp_puzzle;              // lamp puzzle
    private bool can_trigger_puzzle = false;
    public bool on_puzzle = false;
    public bool have_battery = false;

    public Toggle[] switches;                   // switch toggles and sprites
    public CanvasGroup switch_box;
    public Sprite switch_off;                
    public Sprite switch_on;

    public Image[] lamps;                       // lamp image, sprites, and states
    public Sprite lamp_on;                     
    public Sprite lamp_off;
    private bool[] lamp_states;

    public Animator book_anim;                  // to trigger the sliding bookshelf
    AudioManager audiomanager;


    private void Start()
    {
        lamp_puzzle.SetActive(false);
        on_puzzle = false;
        int lamps_number = 7;

        if (switches.Length != lamps_number || lamps.Length != lamps_number)
        {
            Debug.LogError("Number of switches and lamps must be " + lamps_number);     // debug to ensure the number switches and arrays are matching
            return;
        }

        lamp_states = new bool[7] { false, false, false, false, false, false, false};   // all the lamps are initially off
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && can_trigger_puzzle && have_battery)   // if the puzzle isn't done and batteries are in hand, puzzle can be triggered
        {
            lamp_puzzle.SetActive(true);
            on_puzzle = true;
            Debug.Log("interacted with puzzle");
        }

        if(Input.GetKeyDown(KeyCode.Escape) && on_puzzle)                       // when uncertain, the player can leave the puzzle for now
        {
            ClosePuzzle();
        }
    }


    private void Awake()
    {
        audiomanager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_trigger_puzzle = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_trigger_puzzle = false;
        }
    }


    public void ToggleSwitch(int index)         // toggling the switches light up different lamps
    {
        if (index < 0 || index >= lamp_states.Length)
        {
            Debug.LogError("Index out of range: " + index);
            return;
        }
        lamp_states[index] = !lamp_states[index];

        lamps[index].sprite = lamp_states[index] ? lamp_on : lamp_off;

        switches[index].GetComponentInChildren<Image>().sprite = switches[index].isOn ? switch_on : switch_off;

        GoalCondition();
    }


    private void GoalCondition()                // condition to solve the puzzle
    {
        if (lamp_states[2] && lamp_states[3] && lamp_states[4] && !lamp_states[0] && !lamp_states[1] && !lamp_states[5] && !lamp_states[6])
        {
            StartCoroutine(HiddenDoorAnimation());
        }
    }


    private IEnumerator HiddenDoorAnimation()   // used to trigger the sliding bookshelf
    {
        switch_box.interactable = false;
        yield return new WaitForSeconds(2f);
        book_anim.SetTrigger("Move");
        audiomanager.PlaySFX(audiomanager.SFX_shelf_drag);
        ClosePuzzle();
    }


    public void ClosePuzzle()                   // whenever the player exits the puzzle
    {
        lamp_puzzle.SetActive(false);
        on_puzzle = false;
    }
}
