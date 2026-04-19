using UnityEngine;


// 1. This defines the "list" of options
public enum ItemType { Apple, Wood, Seed, Egg, GoldenEgg, Scrap }

[System.Serializable]
public class DialogueStateRule
{
    public string requiredQuest;
    

    // 2. This is the actual variable that shows up in the Inspector
    public ItemType itemToCheck;

    public int amountNeeded = 1;
    public string idIfRequirementMet;
    public string idIfRequirementNotMet;
    public string idAfterQuestFinished;
}

public class NPC : MonoBehaviour
{
    public string npcID;
    public DialogueStateRule questLogic;
    [HideInInspector] public bool playerIsClose;

    public string GetCurrentDialogueID()
    {
        if (QuestManager.Instance.activeQuests.Contains(questLogic.requiredQuest))
        {
            // Use the helper to check the inventory
            if (HasEnoughItems())
            {
                return questLogic.idIfRequirementMet;
            }
            return questLogic.idIfRequirementNotMet;
        }

        if (QuestManager.Instance.Finished)
            return questLogic.idAfterQuestFinished;

        return npcID;
    }

    // This checks the GameManager based on what you picked in the dropdown
    private bool HasEnoughItems()
    {
        switch (questLogic.itemToCheck)
        {
            case ItemType.Apple: return GameManager.Instance.appleCount >= questLogic.amountNeeded;
            case ItemType.Wood: return GameManager.Instance.woodCount >= questLogic.amountNeeded;
            case ItemType.Seed: return GameManager.Instance.seedCount >= questLogic.amountNeeded;
            case ItemType.Egg: return GameManager.Instance.eggCount >= questLogic.amountNeeded;
            case ItemType.GoldenEgg: return GameManager.Instance.goldenEggCount >= questLogic.amountNeeded; // Added this!
            case ItemType.Scrap: return GameManager.Instance.scrapMetalCount >= questLogic.amountNeeded;
            default: return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            DialogueManager.Instance.currentNPC = this;
            DialogueManager.Instance.playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            DialogueManager.Instance.playerIsClose = false;
        }
    }
}