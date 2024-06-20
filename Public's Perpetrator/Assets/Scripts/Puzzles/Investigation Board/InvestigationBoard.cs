using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InvestigationBoard : MonoBehaviour
{
    // Puzzle
    public GameObject investigation_board;
    private bool can_open_board = false;
    private bool figure_done = false;
    public bool on_board = false;
    public bool have_finger = false;
    public bool have_knife = false;
    public bool have_certificate = false;
    public bool have_poster = false;
    public bool interact_counter_L = false;
    public bool interact_counter_L_2 = false;
    public bool interact_counter_R = false;
    private int curr_connector;

    // Connector Sprites
    public Sprite connector_selected;
    public Sprite connector_unselected;
    public Sprite connector_solved;

    // Connectors
    public Button[] connectors;
    private bool[] connector_states;
    private bool[] connector_done;

    // Connections Condition
    private bool a_fingerprint = false;
    private bool the_signature = false;
    private bool the_finger_stamp = false;

    // Connections Button
    public GameObject[] connections;

    // Connection Count
    private int connection_count;
    public Text connection_text;
    public Text connection_shade;

    // Change Scene Transition
    public Animator transition_anim;
    private void Start()
    {
        investigation_board.SetActive(false);
        on_board = false;

        connector_states = new bool[3] { false, false, false};
        connector_done = new bool[3] { false, false, false};
        connection_count = 0;
        connection_text.text = "Connections : " + connection_count;
        connection_shade.text = "Connections : " + connection_count;

        foreach (GameObject connection in connections)
        {
            connection.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && can_open_board && !figure_done && CompletedItems())
        {
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
        if(have_finger && have_knife && have_certificate && have_poster && interact_counter_L && interact_counter_L_2 && interact_counter_R)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Touched the board");
        if (collision.CompareTag("Player"))
        {
            can_open_board = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            can_open_board = false;
        }
    }
    public void ConnectorButton(int connector)
    {
        if(connector == 0 && !connector_done[0])
        {
            CloseConnection(1);
            CloseConnection(2);
            connections[connector].SetActive(true);
            connector_states[connector] = true;
            UpdateConnectorSprite(connector);
            curr_connector = connector;
        }
        if (connector == 1 && !connector_done[1])
        {
            CloseConnection(0);
            CloseConnection(2);
            connections[connector].SetActive(true);
            connector_states[connector] = true;
            UpdateConnectorSprite(connector);
            curr_connector = connector;
        }
        if (connector == 2 && !connector_done[2])
        {
            CloseConnection(0);
            CloseConnection(1);
            connections[connector].SetActive(true);
            connector_states[connector] = true;
            UpdateConnectorSprite(connector);
            curr_connector = connector;
        }
    }
    private void UpdateConnectorSprite(int connector)
    {
        if (!connector_done[connector])
        {
            if (connector_states[connector])
            {
                Debug.Log("Change connector to Red");
                connectors[connector].GetComponent<Image>().sprite = connector_selected;
            }
            else
            {
                Debug.Log("Change connector to Blue");
                connectors[connector].GetComponent<Image>().sprite = connector_unselected;
            }
        }
    }
    public void TheSymbols()
    {
        if(curr_connector == 0 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }
    public void TheFingerStamp()
    {
        if(curr_connector == 1 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }
    public void TheBlood()
    {
        if (curr_connector == 2 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }
    private void ConnectorSolved(int connector)
    {
        Debug.Log("Change Connector to Purple");
        connector_done[connector] = true;
        connectors[connector].GetComponent<Image>().sprite = connector_solved;
        CloseConnection(connector);
        connection_count++;
        connection_text.text = "Connections : " + connection_count;
        connection_shade.text = "Connections : " + connection_count;
        if (connection_count >= 3)
        {
            StartCoroutine(ScenarioFigured());
        }
    }
    private IEnumerator ScenarioFigured()
    {
        figure_done = true;
        yield return new WaitForSeconds(3f);
        CloseBoard();
        transition_anim.Play("StartFade");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transition_anim.Play("EndFade");
    }
    private void CloseConnection(int number)
    {
        connections[number].SetActive(false);
        connector_states[number] = false;
        UpdateConnectorSprite(number);
    }
    private void CloseBoard()
    {
        investigation_board.SetActive(false);
        on_board = false;
    }
}
