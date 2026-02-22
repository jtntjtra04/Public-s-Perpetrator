using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LampPuzzle : MonoBehaviour
{
    // Puzzle
    public GameObject lamp_puzzle;
    private bool can_trigger_puzzle = false;
    private bool puzzle_done = false;
    public bool on_puzzle = false;
    public bool have_battery = false;

    // Toggle Switch
    public Toggle[] switches;
    public CanvasGroup switch_box;

    // Switch Sprite
    public Sprite switch_off;
    public Sprite switch_on;

    // Lamps
    public Image[] lamps;

    // Lamp Sprite
    public Sprite lamp_on;
    public Sprite lamp_off;

    // Lamp Condition
    private bool[] lamp_states;

    // Bookshelf references
    public Animator book_anim;
    AudioManager audiomanager;

    private void Start()
    {
        lamp_puzzle.SetActive(false);
        on_puzzle = false;
        int lamps_number = 7;

        // Ensure the switches and lamps arrays have the correct number of elements
        if (switches.Length != lamps_number || lamps.Length != lamps_number)
        {
            Debug.LogError("Number of switches and lamps must be " + lamps_number);
            return;
        }
        lamp_states = new bool[7] { false, false, false, false, false, false, false};

        Debug.Log(lamps.Length);
/*        for(int i = 0; i < lamps_number; i++)
        {
            lamps[i].sprite = lamp_states[i] ? lamp_on : lamp_off;
            switches[i].onValueChanged.AddListener((bool isOn) => { ToggleSwitch(i); });
            switches[i].GetComponentInChildren<Image>().sprite = switches[i].isOn ? switch_on : switch_off;
        }*/
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && can_trigger_puzzle && !puzzle_done && have_battery)
        {
            lamp_puzzle.SetActive(true);
            on_puzzle = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && on_puzzle)
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
    public void ToggleSwitch(int index)
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
    private void GoalCondition()
    {
        if (lamp_states[2] && lamp_states[3] && lamp_states[4] && !lamp_states[0] && !lamp_states[1] && !lamp_states[5] && !lamp_states[6])
        {
            StartCoroutine(HiddenDoorAnimation());
        }
    }
    private IEnumerator HiddenDoorAnimation()
    {
        switch_box.interactable = false;
        yield return new WaitForSeconds(2f);
        book_anim.SetTrigger("Move");
        audiomanager.PlaySFX(audiomanager.shelfdrag);
        puzzle_done = true;
        ClosePuzzle();
    }
    public void ClosePuzzle()
    {
        lamp_puzzle.SetActive(false);
        on_puzzle = false;
    }
}
