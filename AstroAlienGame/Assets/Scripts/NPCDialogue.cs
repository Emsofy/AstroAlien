using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public string dialogueID = "npc_intro";

    public void Interact()
    {
        if (!GameManager.Instance.HasSeenDialogue(dialogueID))
        {
            Debug.Log("First time dialogue");
            GameManager.Instance.MarkDialogue(dialogueID);
        }
        else
        {
            Debug.Log("Repeat dialogue");
        }
    }
}