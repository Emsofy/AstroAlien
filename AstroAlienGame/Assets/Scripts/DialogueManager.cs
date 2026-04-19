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
        else
        {
            Destroy(gameObject); // Prevent duplicate managers
        }
    }
    private void Start()
    {
        if (Bubble != null) Bubble.enabled = false;
        if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();

        // Ensure panel starts off
        dialoguePanel.SetActive(false);
    }
 
    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            // If the panel is ALREADY open, we just want to go to the next line
            if (dialoguePanel.activeInHierarchy)
            {
                NextLine();
            }
            // If the panel is CLOSED, we start the conversation
            else if (!AnyDialogueRunning)
            {
                Begin();
            }
        }
    }

    public void Begin()
    {
        var introEntry = database.GetDialogue(npcID);
        bool introFinished = introEntry != null && introEntry.seen;

        // Logic Priority
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

        var finalEntry = database.GetDialogue(dialogueID);

        if (finalEntry != null)
        {
            // ONLY start if we actually found dialogue to play
            AnyDialogueRunning = true;
            Quest.isDialogueActive = true;

            dialogue = finalEntry.conversation;
            index = 0;

            dialoguePanel.SetActive(true); // Open the panel here!

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

}