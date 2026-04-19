using UnityEngine;
using static DialogueManager;

[System.Serializable]
public class DialogueStateRule
{
    public string requiredQuest;      // e.g., "SpecialFruit"
    public string idIfRequirementMet;   // e.g., "SpecialFruit_Complete"
    public string idIfRequirementNotMet; // e.g., "SpecialFruit_Reminder"
    public string idAfterQuestFinished; // e.g., "Alien_PostQuest"
}
public class NPC : MonoBehaviour
{
    

    public string npcID; // The Intro ID
    public DialogueStateRule questLogic; // Fill this in the Inspector!
    public bool playerIsClose;

    public string GetCurrentDialogueID()
    {
        // 1. If Intro not seen, return Intro ID
       // if (!GameManager.Instance.HasSeenDialogue(npcID)) return npcID;

        // 2. Check if the specific quest for this NPC is active
        if (QuestManager.Instance.activeQuests.Contains(questLogic.requiredQuest))
        {
            // Check if player has the items (Assuming GameManager handles apples/fruit)
            if (GameManager.Instance.appleCount >= 1)
            {
                return questLogic.idIfRequirementMet;
            }

            return questLogic.idIfRequirementNotMet;
        }

        // 3. If the quest is finished, show the post-quest dialogue
        if (QuestManager.Instance.Finished)
            return questLogic.idAfterQuestFinished;

        // 4. Default fallback
        return npcID + "_Default";
    }

    // Trigger logic...
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
           // DialogueManager.Instance.currentNPC = this; // Pass the whole NPC script
            DialogueManager.Instance.playerIsClose = true;
        }
    }
}