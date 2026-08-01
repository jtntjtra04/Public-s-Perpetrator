using System.Collections;
using System.Collections.Generic;
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
    public bool have_poster = false;

    [Header("Connector Related")]
    private int curr_connector;
    public Sprite connector_selected;               // connector sprites
    public Sprite connector_unselected;
    public Sprite connector_solved;

    public Button[] connectors;                     // connector buttons
    private bool[] connector_states;
    private bool[] connector_done;

    public GameObject[] connections;                // links between evidence

    private int connection_count;                   // connections made
    public Text connection_text;
    public Text connection_shade;

    public Animator transition_anim;                // change of scene after secret link puzzle is solved


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
        if (Input.GetKeyDown(KeyCode.E) && can_open_board && !secret_puzzle_solved && CompletedItems())
        {
            investigation_board.SetActive(true);            // interacting with the secret link puzzle
            on_board = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && on_board)   // leave the board to explore again
        {
            CloseBoard();
        }
    }


    public bool CompletedItems()    // all necessary items / evidence gathered
    {
        if(have_finger && have_knife && have_certificate && (mail_status.opened_mail == true))
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


    private void UpdateConnectorSprite(int connector)       // selecting connectors will change their appearance
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


    public void TheSymbols()    // correct answer 1
    {
        if(curr_connector == 0 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }


    public void TheFingerStamp()    // correct answer 2
    {
        if(curr_connector == 1 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }


    public void TheBlood()    // correct answer 3
    {
        if (curr_connector == 2 && connection_count < 3)
        {
            ConnectorSolved(curr_connector);
        }
    }


    private void ConnectorSolved(int connector)     // if answer is correct
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
            StartCoroutine(SecretLinkCleared());
        }
    }


    private IEnumerator SecretLinkCleared()
    {
        secret_puzzle_solved = true;
        yield return new WaitForSeconds(3f);
        CloseBoard();
        transition_anim.Play("StartFade");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // switches scenes when the puzzle is solved
        transition_anim.Play("EndFade");
    }


    private void CloseConnection(int number)        // other choices are ridden of
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
