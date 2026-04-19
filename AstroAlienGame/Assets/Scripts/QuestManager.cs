using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("References")]
    public DialogueManager Manager; // Ensure this is assigned in the Inspector

    [Header("Quest State")]
    public List<string> activeQuests = new List<string>();
    public int fruitCount = 0;
    public bool Finished = false;
    public bool isDialogueActive = false;

    [Header("Interaction State")]
    public bool playerIsClose;
    public GameObject currentFruit;
    
    
    public DialogueManager currentAlien; // The specific alien we are touching

        void Awake() { Instance = this; }

    void Update()
    {
        // Check if player is near a fruit AND presses E
        if (playerIsClose && Input.GetKeyDown(KeyCode.E))
        {
            PickUpFruit();
        }
    }

    private void PickUpFruit()
    {
        Destroy(currentFruit);
        fruitCount++;
        playerIsClose = false; // Reset so we don't try to destroy it again
        currentFruit = null;
        Debug.Log("Fruit Picked Up! Total: " + fruitCount);
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
            Debug.Log("Quest Started: " + questName);
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

    // This is called by DialogueManager when a conversation finishes
    public void MarkSeen(string ID)
    {
        if (Manager == null || Manager.database == null) return;

        var entry = Manager.database.GetDialogue(ID);
        if (entry != null)
        {
            entry.seen = true;
            Debug.Log(ID + " has been marked as seen in the database!");

            // LOGIC FOR STARTING THE QUEST
            // If the ID that just finished was the NPC's intro, start the fruit quest
            if (ID == "conversation" || ID == "BlueAlienConvo")
            {
                StartQuest("SpecialFruit");
            }
        }
    }
}