using UnityEngine;

public class DialogueNeedsToEnd : MonoBehaviour
{
    public DialogueManager Manager;
    void Update()
    {
        if (Manager.playerIsClose && Input.GetKeyDown(KeyCode.E))
        {
            // Toggle: If the panel is off, start it; if it's on, go to next line
            if (!DialogueManager.Instance.dialoguePanel.activeInHierarchy)
            {
                DialogueManager.Instance.Begin();
            }
            else
            {
                DialogueManager.Instance.NextLine();
            }
        }
    }
}
