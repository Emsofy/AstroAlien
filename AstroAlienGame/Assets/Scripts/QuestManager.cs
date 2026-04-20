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
    private void Start()
    {
        {
            LoadData();
        }
    }

    void Update()
    {
       
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && currentFruit != null)
        {
            PickUpFruit();
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("QuestFinished", Finished ? 1 : 0);
        PlayerPrefs.SetInt("HasFruit", HasFruit);
        // Note: Saving Lists is complex, so we save the active quest status as an Int
        PlayerPrefs.SetInt("SpecialFruitActive", activeQuests.Contains("SpecialFruit") ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        Finished = PlayerPrefs.GetInt("QuestFinished", 0) == 1;
        HasFruit = PlayerPrefs.GetInt("HasFruit", 0);

        if (PlayerPrefs.GetInt("SpecialFruitActive", 0) == 1)
        {
            if (!activeQuests.Contains("SpecialFruit")) activeQuests.Add("SpecialFruit");
        }
    }
    public string DetermineDialogueID(string npcID)
    {
       
        if (!GameManager.Instance.HasSeenDialogue(npcID))
        {
            return npcID;
           
        }

        
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

<<<<<<< HEAD
    public void OnDialogueComplete(string id)
    {
        
        
        if (id == "SpecialFruit_Complete")
=======
    
    public void OnDialogueComplete(string dialogueID)
    {
       
        if (dialogueID == "Alien_Intro")
>>>>>>> parent of e134f85 (Before I break it AGAIN)
        {
            StartQuest("SpecialFruit");
        }

       
        if (dialogueID == "SpecialFruit_Complete")
        {
            GameManager.Instance.RemoveApple(1);
            activeQuests.Remove("SpecialFruit");
            Finished = true; 
            GameManager.Instance.SaveGame();
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

   
}