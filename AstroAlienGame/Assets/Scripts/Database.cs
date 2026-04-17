using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabaseSO", menuName = "Game/DialogueDatabaseSO")]
public class Database : ScriptableObject
{
    public List<DialogueEventSO> dialogues;

    public DialogueEventSO GetDialogue(string id)
    {
        return dialogues.Find(d => d.dialogueID == id);
    }
}
