using UnityEngine;

[CreateAssetMenu(fileName = "DialogueEventSO", menuName = "Game/DialogueEventSO")]
public class DialogueEventSO : ScriptableObject
{

    [SerializeField] public string dialogueID;

    [SerializeField] public string[] conversation;
    public bool seen; //runtime flag
}
