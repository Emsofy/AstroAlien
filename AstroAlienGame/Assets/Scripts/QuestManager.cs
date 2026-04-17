using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.MaterialUpgrader;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public DialogueManager Manager;

    public List<string> activeQuests = new List<string>();
    public int fruitCount = 0;
    public bool playerIsClose;
    public GameObject currentFruit;
    public string questID;
    public bool Finished = false;
    public bool nearAlien;
    public bool isDialogueActive = false;
    public bool seen = false;
    void Awake() { Instance = this; }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fruit"))
        {
            playerIsClose = true;
            currentFruit = other.gameObject;
        }

        if (other.CompareTag("Alien")) // Trigger for the NPC
        {
            nearAlien = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fruit")) { playerIsClose = false; currentFruit = null; }
        if (other.CompareTag("Alien")) { nearAlien = false; }
    }
    void Update()
    {
        // PICK UP FRUIT
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && currentFruit != null)
        {
            Destroy(currentFruit);
            fruitCount++;
            playerIsClose = false;
        }

        // TALK TO ALIEN
        // Added !isDialogueActive so you can't restart it while reading
        if (Input.GetKeyDown(KeyCode.E) && nearAlien && !isDialogueActive)
        {
            Begin("conversation");
        }
    }

    public void StartQuest(string questName)
    {
        if (!activeQuests.Contains(questName))
        {
            activeQuests.Add(questName);
            Debug.Log("Quest Started: " + questName);
            // Update UI or spawn quest items here
        }
    }


    public bool HasFruit(int amount)
    {
        return fruitCount >= amount;
    }

    public void RemoveFruit(int amount)
    {
        fruitCount -= amount;
    }
    public void MarkSeen(string ID)
    {
        var entry = Manager.database.GetDialogue(ID);

        if (entry != null)
        {
            entry.seen = true; // This changes the data in the SO
            Debug.Log(ID + " has been marked as seen!");

            if (ID == "conversation")
            {
                seen = true;
                StartQuest("SpecialFruit");
                
            }
        }
    }

    public void Begin(string dialogueID)
    {
        isDialogueActive = true;

        // 1. FORCED INTRODUCTION: Play "conversation" until it's been seen once.
        var introEntry = Manager.database.GetDialogue("conversation");
        if (introEntry != null && !introEntry.seen)
        {
            dialogueID = "conversation";
        }
        // 2. DELIVERY: If intro is done and we have the fruit
        else if (activeQuests.Contains("SpecialFruit") && HasFruit(1))
        {
            dialogueID = "SpecialFruit_Complete";
            RemoveFruit(1);
            activeQuests.Remove("SpecialFruit");
            Finished = true;
        }
        // 3. FINISHED: Quest is done
        else if (Finished)
        {
            dialogueID = "Alien_PostQuest";
        }
        // 4. REMINDER: Intro is done, quest is active, but no fruit yet
        else if (activeQuests.Contains("SpecialFruit"))
        {
            dialogueID = "SpecialFruit_Reminder";
        }

        // FETCH AND DISPLAY (rest of your existing code...)
        var finalEntry = Manager.database.GetDialogue(dialogueID);
        if (finalEntry != null)
        {
            Manager.dialogue = finalEntry.conversation;
            Manager.index = 0;
            Manager.dialoguePanel.SetActive(true);
            Manager.StopAllCoroutines();
            Manager.dialogueText.text = "";
            StartCoroutine(Manager.Typing());
        }
    }
    //when player has fruit
    //f to give
    //decreases has fruit back to 0
    //Quest is completed
    //if quest is completed--> they help you
}