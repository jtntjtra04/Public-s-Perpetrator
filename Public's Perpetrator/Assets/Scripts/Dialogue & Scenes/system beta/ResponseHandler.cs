using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform response_Box;
    [SerializeField] private RectTransform response_Button_Template;
    [SerializeField] private RectTransform response_Container;
    private DialogueUI dialogueUI;
    private List<GameObject> temp_response_Buttons = new List<GameObject>();


    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }


    public void ShowResponses(Response[] responses)
    {
        float response_box_height = 0;
         
        foreach (Response response in responses)    // loop through responses
        {
            GameObject response_Button = Instantiate(response_Button_Template.gameObject, response_Container);      // declares response option buttons
            response_Button.gameObject.SetActive(true);                                                             // rectTransform cannot SetActive
            response_Button.GetComponent<TMP_Text>().text = response.ResponseText;
            response_Button.GetComponent<Button>().onClick.AddListener(call:() => OnPickedResponse(response));      // a built-in OnClick() that triggere OnPickedResponse()

            temp_response_Buttons.Add(response_Button);

            response_box_height += response_Button_Template.sizeDelta.y;        // increment height of response boxes
        }
         
        response_Box.sizeDelta = new Vector2(response_Box.sizeDelta.x, y:response_box_height);
        response_Box.gameObject.SetActive(true);
    }


    private void OnPickedResponse(Response response)
    {
        response_Box.gameObject.SetActive(false);               // removes response box

        foreach(GameObject button in temp_response_Buttons)     // removes response option buttons after picking a response
        {   
            Destroy(button);
        }
        temp_response_Buttons.Clear();

        dialogueUI.ShowDialogue(response.DialogueObject);       // shows resultant dialogue of the picked response
    }
}
