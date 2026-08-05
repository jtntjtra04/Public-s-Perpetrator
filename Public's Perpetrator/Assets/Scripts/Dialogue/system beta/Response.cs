using UnityEngine;

[System.Serializable]

public class Response       // mainly to store data
{
    [SerializeField] private string response_Text;
    [SerializeField] private DialogueObject dialogue_Object;

    public string ResponseText => response_Text;
    public DialogueObject DialogueObject => dialogue_Object;
}   