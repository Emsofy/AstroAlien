using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcID; // Set this in the Inspector for each alien

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.npcID = npcID;
            DialogueManager.Instance.playerIsClose = true;
            if (DialogueManager.Instance.Bubble != null)
            {
                DialogueManager.Instance.Bubble.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.playerIsClose = false;
            if (DialogueManager.Instance.dialoguePanel.activeInHierarchy)
            {
                DialogueManager.Instance.zeroText();
            }
            DialogueManager.Instance.Bubble.enabled = false;
        }
    }
}