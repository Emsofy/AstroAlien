using UnityEngine;


// 1. This defines the "list" of options
public enum ItemType { Apple, Wood, Seed, Egg, GoldenEgg, Scrap }

[System.Serializable]
public class DialogueStateRule
{
    public string requiredQuest;
    
    public ItemType itemToCheck;

    public int amountNeeded = 1;
    public string idIfRequirementMet;
    public string idIfRequirementNotMet;
    public string idAfterQuestFinished;

    public bool hasMet = false;
}

public class NPC : MonoBehaviour
{
    public string npcID;
    public DialogueStateRule questLogic;
    [HideInInspector] public bool playerIsClose;
    private void Start()
    {
        DialogueManager.Instance.Bubble.enabled = false;
    }
    public string GetCurrentDialogueID()
    {
        
        if (QuestManager.Instance.activeQuests.Contains(questLogic.requiredQuest))
        {
            return HasEnoughItems() ? questLogic.idIfRequirementMet : questLogic.idIfRequirementNotMet;
        }

        
        if (QuestManager.Instance.Finished) return questLogic.idAfterQuestFinished;

        
        return npcID;
    }
   
    private bool HasEnoughItems()
    {
        switch (questLogic.itemToCheck)
        {
            case ItemType.Apple: return GameManager.Instance.appleCount >= questLogic.amountNeeded;
            case ItemType.Wood: return GameManager.Instance.woodCount >= questLogic.amountNeeded;
            case ItemType.Seed: return GameManager.Instance.seedCount >= questLogic.amountNeeded;
            case ItemType.Egg: return GameManager.Instance.eggCount >= questLogic.amountNeeded;
            case ItemType.GoldenEgg: return GameManager.Instance.goldenEggCount >= questLogic.amountNeeded; 
            case ItemType.Scrap: return GameManager.Instance.scrapMetalCount >= questLogic.amountNeeded;
            default: return false;
        }
    }

    
}