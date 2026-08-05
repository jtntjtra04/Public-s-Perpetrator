using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;   // list of dialogues
    [SerializeField] private Response[] responses;          // list of responses

    public string[] Dialogue => dialogue;       // prevents overwriting from other sources
    public Response[] Responses => responses;
    public bool has_responses => Responses != null && Responses.Length > 0;     // checks if there's any options for responses
}
