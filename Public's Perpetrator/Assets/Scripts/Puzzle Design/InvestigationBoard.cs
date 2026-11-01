using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


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
    private int curr_connector;
    private bool connector_open = true;

    [Header("Descriptions")]
    public TMP_Text descriptive_text;

    [Header("Answer / Connections Related")]
    private int[] selected_answers = { -1, -1, -1 };
    private int[] correct_answers = { 0, 1, 0 };

    [Header("After the Puzzle")]
    public Animator transition_anim;                // change of scene after secret link puzzle is solved
    public DialogueTrigger dialogue_after;
    public DialogueManager dialogue_manager;
    public Dialogue dialogue;

    [SerializeField] private string interaction_id;
    [SerializeField] private string puzzle_id;


    // =============================== //
    // Puzzle Updates & Start          //
    // =============================== //


    private void Start()
    {
        investigation_board.SetActive(false);
        on_board = false;
        connector_selected = new bool[3] {false, false, false};
        descriptive_text.text = "After searching the house, these are the several clues I could find. I wonder how they're all connected..";

        foreach (GameObject connection in connections) connection.SetActive(false);

        if (string.IsNullOrWhiteSpace(interaction_id))
        {
            Debug.LogError($"An interaction ID must be assigned to {gameObject.name}.", this);     // ID must be assigned to the object
            return;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && can_open_board && !secret_puzzle_solved && CompletedItems() && !dialogue_manager.dialogue_ongoing)
        {
            dialogue_manager.StartDialogue(dialogue, gameObject);
            if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.RecordInteract(interaction_id);
            if (AnalyticsManager.Instance != null) AnalyticsManager.Instance.BeginPuzzleAttempt(puzzle_id);

            investigation_board.SetActive(true);
            on_board = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && on_board)
        {
            CloseBoard();
        }
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
        for (int i = 0; i < connectors.Length; i++)
        {
            if (connectors[i] != null && !dialogue_manager.dialogue_ongoing && connector_open)
            {
                connectors[i].image.color = (i == connector) ? new Color32(186, 178, 254, 255) : Color.white;
                descriptive_text.text = "";
            }
        }

        if (connector == 0 && !connector_selected[0] && !dialogue_manager.dialogue_ongoing && connector_open)
        {
            Debug.Log("connector 1 selected");
            CloseConnection(1);
            CloseConnection(2);
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
        if (connector == 1 && !connector_selected[1] && !dialogue_manager.dialogue_ongoing && connector_open)
        {
            Debug.Log("connector 2 selected");
            CloseConnection(0);
            CloseConnection(2);
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
        if (connector == 2 && !connector_selected[2] && !dialogue_manager.dialogue_ongoing && connector_open)
        {
            Debug.Log("connector 3 selected");
            CloseConnection(0);
            CloseConnection(1);
            connections[connector].SetActive(true);
            connector_selected[connector] = true;
            curr_connector = connector;
        }
    }


    public void SelectAnswer(int answer)
    {
        if (curr_connector < 0) return;

        selected_answers[curr_connector] = answer;

        Button[] string_options = connections[curr_connector].GetComponentsInChildren<Button>();
        for (int i = 0; i < string_options.Length; i++)
        {
            string_options[i].image.color = (i == answer) ? new Color32(186, 178, 254, 255) : Color.white;
        }

        if (selected_answers[0] != correct_answers[0])
        {
            Debug.Log("Wrong answer(s) selected");
        }
        else if (selected_answers[1] != correct_answers[1])
        {
            Debug.Log("Wrong answer(s) selected");
        }
        else if (selected_answers[2] != correct_answers[2])
        {
            Debug.Log("Wrong answer(s) selected");
        }

        if (selected_answers[0] == correct_answers[0] &&
            selected_answers[1] == correct_answers[1] &&
            selected_answers[2] == correct_answers[2])
        {
            connector_open = false;
            StartCoroutine(SecretLinkCleared());
        }
    }


    // =============================== //
    // When the Puzzle is Solved       //
    // =============================== //


    private IEnumerator SecretLinkCleared()
    {
        CloseConnection(0);
        CloseConnection(1);
        CloseConnection(2);

        secret_puzzle_solved = true;
        descriptive_text.text = "That must be it..";

        yield return new WaitForSeconds(2f);
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
