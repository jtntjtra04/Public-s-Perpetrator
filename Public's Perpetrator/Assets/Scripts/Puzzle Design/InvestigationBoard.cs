using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InvestigationBoard : MonoBehaviour
{
    public GameObject investigation_board;          // secret link puzzle
    private bool can_open_board = false;
    private bool secret_puzzle_solved = false;
    public bool on_board = false;

    [Header("Requirements")]
    public Mail mail_status;
    public bool have_finger = false;                // requirements
    public bool have_knife = false;
    public bool have_certificate = false;

    [Header("Connector Related")]
    public Button[] connectors;                     // connector buttons
    public GameObject[] connections;                // links between evidence
    private bool[] connector_selected;
    private bool[] connecter_solved = new bool[3];
    private int curr_connector;

    [Header("Answer / Connections Related")]
    private int[] selected_answers = { -1, -1, -1 };
    private int[] correct_answers = { 2, 1, 0 };

    [Header("After the Puzzle")]
    public Animator transition_anim;                // change of scene after secret link puzzle is solved
    public DialogueTrigger dialogue_after;
    public DialogueManager dialogue_manager;

    [SerializeField] private string interaction_id;
    [SerializeField] private string puzzle_id;


    // =============================== //
    // Puzzle Updates & Start          //\    // =============================== //


    private void Start()
    {
        investigation_board.SetActive(false);
        on_board = false;
        connector_selected = new bool[3] {false, false, false};

        foreach (GameObject connection in connections) connection.SetActive(false);

        if (string.IsNullOrWhiteSpace(interaction_id))
        {
            Debug.LogError($"An interaction ID must be assigned to {gameObject.name}.", this);     // ID must be assigned to the object
            return;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && can_open_board && !secret_puzzle_solved && CompletedItems())
        {
            if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.RecordInteract(interaction_id);
            if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.BeginPuzzleAttempt(puzzle_id);

            investigation_board.SetActive(true);
            on_board = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && on_board) CloseBoard();
    }


    public bool CompletedItems()
    {
        if(have_finger && have_knife && have_certificate && (mail_status.opened_mail == true)) return true;
        else return false;
    }


    // =============================== //
    // When Interacted With            //
    // =============================== //


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched the board");
        if (collision.CompareTag("Player")) can_open_board = true;
    }


    private void OnTriggerExit2D(Collider2D collision)  
    {
        if (collision.CompareTag("Player")) can_open_board = false;
    }


    // =============================== //
    // Puzzle Adjustments              //
    // =============================== //


    public void ConnectorButton(int connector)
    {
        if(connector == 0 && !connector_selected[0])
        {
            Debug.Log("connector 1 selected");
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
        if (connector == 1 && !connector_selected[1])
        {
            Debug.Log("connector 2 selected");
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
        if (connector == 2 && !connector_selected[2])
        {
            Debug.Log("connector 3 selected");
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
    }


    public void SelectAnswer(int answer)
    {
        if (curr_connector < 0)
            return;

        selected_answers[curr_connector] = answer;

        if (selected_answers[0] == correct_answers[0] &&
            selected_answers[1] == correct_answers[1] &&
            selected_answers[2] == correct_answers[2])
        {
            StartCoroutine(SecretLinkCleared());
        }
    }


    private void ConnectorSolved(int connector)
    {
        if (connecter_solved[connector]) return;
        connecter_solved[connector] = true;
        if (connecter_solved[0] && connecter_solved[1] && connecter_solved[2])
        {
            StartCoroutine(SecretLinkCleared());
        }
    }


    // =============================== //
    // When the Puzzle is Solved       //
    // =============================== //


    private IEnumerator SecretLinkCleared()
    {
        secret_puzzle_solved = true;
        yield return new WaitForSeconds(3f);
        CloseBoard();
        dialogue_after.TriggerDialogue();

        DialogueManager dialogue_manager = FindAnyObjectByType<DialogueManager>();
        yield return new WaitUntil(() => !dialogue_manager.dialogue_ongoing);           // wait until dialogue is over

        transition_anim.Play("StartFade");                                              // switches scenes with transition
        yield return new WaitForSeconds(2f);
        transition_anim.Play("EndFade");

        WorldSceneManager.world_scene_ToPlay = WorldSceneType.Epilogue;                 // switches to epilogue
        SceneManager.LoadScene("Ch 1 World Scenes");
    }


    private void CloseConnection(int number)        // other choices are ridden of
    {
        connections[number].SetActive(false);
        connector_selected[number] = false;
    }


    private void CloseBoard()
    {
        if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.EndPuzzleAttempt(puzzle_id, secret_puzzle_solved);
        investigation_board.SetActive(false);
        on_board = false;
    }
}
