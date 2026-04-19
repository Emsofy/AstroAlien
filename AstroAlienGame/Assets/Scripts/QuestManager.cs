using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("References")]
    public DialogueManager Manager;

    [Header("Quest State")]
    public List<string> activeQuests = new List<string>();
    public bool Finished = false;
    public bool isDialogueActive = false;

    [Header("Interaction State")]
    public bool playerIsClose;
    public GameObject currentFruit;
    public int HasFruit;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Handle Fruit Pickup
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && currentFruit != null)
        {
            PickUpFruit();
            
        }
    }

    // --- NEW: THE ID DECIDER ---
    // This removes the hardcoding from DialogueManager. 
    // It checks GameManager to see what has been seen/collected.
    public string DetermineDialogueID(string npcID)
    {
        // 1. Check if we have ever finished the intro for this NPC
        if (!GameManager.Instance.HasSeenDialogue(npcID))
        {
            return npcID;
        }

        // 2. Check for Quest Progress
        if (activeQuests.Contains("SpecialFruit"))
        {
            // Check GameManager inventory instead of local fruitCount
            if (GameManager.Instance.appleCount >= 1)
                return "SpecialFruit_Complete";

            return "SpecialFruit_Reminder";
        }

        // 3. Post-Quest State
        if (Finished)
        {
            return npcID + "_PostQuest";
        }

        // 4. Default if nothing else matches
        return npcID + "_Default";
    }

    // --- NEW: THE CONSEQUENCE HANDLER ---
    // This is called by DialogueManager.NextLine() when a conversation ends.
    public void OnDialogueComplete(string dialogueID)
    {
        // Save the fact that we saw this dialogue in the GameManager
        GameManager.Instance.MarkDialogue(dialogueID);

        // Check if we need to start a quest
        if (dialogueID == "conversation" || dialogueID == "BlueAlienConvo")
        {
            StartQuest("SpecialFruit");
        }

        // Check if we just finished the fruit quest
        if (dialogueID == "SpecialFruit_Complete")
        {
            GameManager.Instance.RemoveApple(1);
            activeQuests.Remove("SpecialFruit");
            Finished = true;
            GameManager.Instance.SaveGame(); // Save progress immediately
        }
    }

    private void PickUpFruit()
    {
        HasFruit++;
        Destroy(currentFruit);
        // Use GameManager to track items so they persist through saves
        GameManager.Instance.AddApple(1);
        playerIsClose = false;
        currentFruit = null;
    }

    // Legacy support for your MarkSeen call
    public void MarkSeen(string ID)
    {
        OnDialogueComplete(ID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            playerIsClose = true;
            currentFruit = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            playerIsClose = false;
            currentFruit = null;
        }
    }

    public void StartQuest(string questName)
    {
        if (!activeQuests.Contains(questName))
        {
            activeQuests.Add(questName);
        }
    }
}