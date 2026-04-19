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
    public string npcID; // Set this to "BlueAlien" or "RedAlien" in Inspector
    public string[] dialogue;
    public int index;
    private bool isTyping = false;
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine typingCoroutine;
    public TextMeshProUGUI Bubble;
    public static bool AnyDialogueRunning = false;

    private void Start()
    {
        if (Bubble != null) Bubble.enabled = false;
        // Automatically find QuestManager if not assigned
        if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            // IF PANEL IS HIDDEN: Only start if NO OTHER dialogue is running
            if (!dialoguePanel.activeInHierarchy)
            {
                if (!AnyDialogueRunning)
                {
                    Begin();
                }
            }
            else
            {
                NextLine();
            }
        }
    }

    public void Begin()
    {
        AnyDialogueRunning = true; // LOCK: No one else can start now
        Quest.isDialogueActive = true;

        // 1. Look up the specific NPC's intro status
        var introEntry = database.GetDialogue(npcID);
        bool introFinished = introEntry != null && introEntry.seen;

        // 2. Logic Priority
        if (!introFinished)
        {
            dialogueID = npcID;
        }
        else if (Quest.activeQuests.Contains("SpecialFruit") && Quest.HasFruit(1))
        {
            dialogueID = "SpecialFruit_Complete";
            Quest.RemoveFruit(1);
            Quest.activeQuests.Remove("SpecialFruit");
            Quest.Finished = true;
        }
        else if (Quest.Finished)
        {
            dialogueID = "Alien_PostQuest";
        }
        else if (Quest.activeQuests.Contains("SpecialFruit"))
        {
            dialogueID = "SpecialFruit_Reminder";
        }

        // 3. Fetch and Display
        var finalEntry = database.GetDialogue(dialogueID);
        if (finalEntry != null)
        {
            dialogue = finalEntry.conversation;
            index = 0;
            dialoguePanel.SetActive(true);
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueText.text = "";
            typingCoroutine = StartCoroutine(Typing());
        }
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
            // Use the specific ID we just finished to mark it seen
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
        AnyDialogueRunning = false; // UNLOCK: Others can talk now
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        if (Quest != null) Quest.isDialogueActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (Bubble != null) Bubble.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            if (Bubble != null) Bubble.enabled = false;
        }
    }
}