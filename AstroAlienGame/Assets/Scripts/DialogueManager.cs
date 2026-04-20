using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    
    

    public static DialogueManager Instance;
    public QuestManager Quest;

    [Header("Dialogue Database")]
    public DialogueDatabaseSO database;
    public string dialogueID;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("NPC Settings")]
    public string npcID;
    public string[] dialogue;
    public int index;
    private bool isTyping = false;
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine typingCoroutine;
    public TextMeshProUGUI Bubble;
    public bool hasMet = false;



    public static bool AnyDialogueRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    private void Start()
    {
        
        if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();

        
        dialoguePanel.SetActive(false);
        hasMet = PlayerPrefs.GetInt("HasMetAlien", 0) == 1;
    }
    public void SaveMetStatus()
    {
        hasMet = true;
        PlayerPrefs.SetInt("HasMetAlien", 1);
        PlayerPrefs.Save();
    }

    private void Update()
    {
<<<<<<< HEAD
        currentDialogueID = QuestManager.Instance.DetermineDialogueID(currentNPC.npcID);

        //var finalEntry = database.GetDialogue(currentDialogueID);

        if (playerIsClose && Input.GetKeyUp(KeyCode.E) && !isTyping && Time.time > nextInteractTime)
=======
        if (playerIsClose && Input.GetKeyUp(KeyCode.E) && !isTyping)
>>>>>>> parent of e134f85 (Before I break it AGAIN)
        {
            

            if (dialoguePanel.activeInHierarchy)
            {
                NextLine();
            }
            else if (!AnyDialogueRunning)
            {
                Begin();
            }
        }
    }

   public NPC currentNPC;
<<<<<<< HEAD
    public void StartDialogue(string[] newDialogue, string id)
    {
        currentDialogueID = id;
        dialogue = newDialogue;
        index = -1;
        dialoguePanel.SetActive(true);
        NextLine();
        // if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        //dialogueText.text = "";
        //QuestManager.Instance.MarkSeen(currentDialogueID);
        //typingCoroutine = StartCoroutine(Typing());
    }
    public void Begin()
    {
        if (currentNPC != null)
        {
            if (hasMet == false)
            {
                currentDialogueID = "Alien_Intro";
            }
            else
            {
                // currentDialogueID = QuestManager.Instance.DetermineDialogueID(currentNPC.npcID);
                currentDialogueID = currentNPC.GetCurrentDialogueID();
            }

            dialogueID = currentDialogueID;

            var finalEntry = database.GetDialogue(dialogueID);
            if (finalEntry != null)
            {
                StartDialogue(finalEntry.conversation, dialogueID);
                AnyDialogueRunning = true;
            }

            /*if (index >= 0 && index < dialogue.Length)
            {
                dialogueText.text = dialogue[index];
            }
=======

    public void Begin()
    {
        
        dialogueID = currentNPC.GetCurrentDialogueID();
    
        var finalEntry = database.GetDialogue(dialogueID);

       if (finalEntry != null)
        {
>>>>>>> parent of e134f85 (Before I break it AGAIN)
            AnyDialogueRunning = true;
            dialogue = finalEntry.conversation;
            index = -1;

            dialoguePanel.SetActive(true);

<<<<<<< HEAD

=======
            dialogueText.text = dialogue[index];
>>>>>>> parent of e134f85 (Before I break it AGAIN)
        }
    }

    public void NextLine()
    {
<<<<<<< HEAD
        if (index < dialogue.Length - 1)
=======
        index++;
        if (index < dialogue.Length)
>>>>>>> parent of e134f85 (Before I break it AGAIN)
        {
            dialogueText.text = "";
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
<<<<<<< HEAD
            // 1. Tell both managers the dialogue ended
            if (QuestManager.Instance != null) QuestManager.Instance.MarkSeen(currentDialogueID);

            // 2. Run the logic to flip the 'hasMet' switch
            OnDialogueComplete(currentDialogueID);

=======
            QuestManager.Instance.MarkSeen(dialogueID);
>>>>>>> parent of e134f85 (Before I break it AGAIN)
            zeroText();
        }
    }

    // Ensure this is INSIDE the DialogueManager class
    public void OnDialogueComplete(string id)
    {
        string cleanID = id.Trim();
        Debug.Log("Finished Dialogue ID: " + cleanID);

        if (cleanID == "Alien_Intro")
        {
            hasMet = true;
            PlayerPrefs.SetInt("HasMetAlien", 1); // Saves so it stays true after restart
            PlayerPrefs.Save();

            // Start the quest
            if (!QuestManager.Instance.activeQuests.Contains("SpecialFruit"))
                QuestManager.Instance.activeQuests.Add("SpecialFruit");

            Debug.Log("Intro finished, quest started.");
        }
    }

    public IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    public void zeroText()
    {
        AnyDialogueRunning = false;
        dialogueText.text = "";
        index = 0;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (Quest != null) Quest.isDialogueActive = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            Bubble.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Bubble.enabled = false;
            playerIsClose = false;
        }
    }
}