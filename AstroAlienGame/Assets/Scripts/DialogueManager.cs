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

        // Ensure panel starts off
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyUp(KeyCode.E) && !isTyping)
        {
            // nextActionTime = Time.time + cooldown; // Prevents double-firing
            dialogueID = "Alien_Intro";
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

   public NPC currentNPC; // Set this via the NPC's OnTriggerEnter

    public void Begin()
    {
        
        dialogueID = currentNPC.GetCurrentDialogueID();
    
        var finalEntry = database.GetDialogue(dialogueID);

        if (finalEntry == null)
        {
            return;
        }

        AnyDialogueRunning = true;
        Quest.isDialogueActive = true;
        dialogue = finalEntry.conversation;

        index = -1; // This ensures we start at the beginning
        dialoguePanel.SetActive(true);
        NextLine();
    }
    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            QuestManager.Instance.MarkSeen(dialogueID);
            zeroText();
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