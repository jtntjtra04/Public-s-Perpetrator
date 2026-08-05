using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogue_Box;
    [SerializeField] private TMP_Text text_Label;         
    [SerializeField] private DialogueObject test_Dialogue;
    private TypewriterEffect typewriterEffect;
    private ResponseHandler responseHandler;


    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogue();
        ShowDialogue(test_Dialogue);
    }


    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogue_Box.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));    // pass test object that starts a coroutine
    }


    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)     // coroutine steps through each entries in dialogue object
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return typewriterEffect.Run(dialogue, text_Label);            // calls typewriter effect
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)
                                          || Input.GetMouseButtonDown(0));      // waits until input to proceed

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.has_responses == true) break;     // pauses dialogue if there're response options
        }

        if(dialogueObject.has_responses == true)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);        // displays response options
        } else
        {
            CloseDialogue();                                                // closes dialogue in the end 
        }
    }


    private void CloseDialogue()
    {
        dialogue_Box.SetActive(false);       // disables dialogue box
        text_Label.text = string.Empty;     // empties the dialogue
    }
}