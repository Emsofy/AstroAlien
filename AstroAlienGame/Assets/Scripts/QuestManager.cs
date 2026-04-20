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
       
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && currentFruit != null)
        {
            PickUpFruit();
            
        }
    }

    
    public string DetermineDialogueID(string npcID)
    {
  
        
        if (activeQuests.Contains("SpecialFruit"))
        {
            
            if (GameManager.Instance.appleCount >= 1)
                return "SpecialFruit_Complete";

            return "SpecialFruit_Reminder";
        }

        
        if (Finished)
        {
            return npcID + "_PostQuest";
        }

        return npcID + "_Default";
    }


    public void OnDialogueComplete(string id)
    {
        if (id == "SpecialFruit_Complete")
        {
            GameManager.Instance.RemoveGoldenApple(1);
            activeQuests.Remove("SpecialFruit");
            Finished = true;
        }
    }


    private void PickUpFruit()
    {
        HasFruit++;
        Destroy(currentFruit);
        
        GameManager.Instance.AddApple(1);
        playerIsClose = false;
        currentFruit = null;
    }

   
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