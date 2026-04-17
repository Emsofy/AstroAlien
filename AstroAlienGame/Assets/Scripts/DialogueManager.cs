using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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
    public string[] dialogue;
    public int index;
    private bool isTyping = false;
    public float wordSpeed;
    public bool playerIsClose;

    private Coroutine typingCoroutine;

    public TextMeshProUGUI Bubble;


    private void Start()
    {
        Bubble.enabled = false;
        DialogueManager Manager = GetComponent<DialogueManager>();
    }
    public void StartDialogue(string[] newDialogue)
    {
        dialogue = newDialogue;
        index = 0;
        dialoguePanel.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(Typing());
    }

    // In your Update(), ONLY handle "NextLine" logic if the panel is already open
    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                NextLine();
            }
        }
        //if panel is active in quest manager script then next line
    }
    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        // ADD THIS LINE:
        if (QuestManager.Instance != null) QuestManager.Instance.isDialogueActive = false;
    }

    public IEnumerator Typing()
    {
        isTyping = true; // Block input
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false; // Allow input again
    }

    public void NextLine()
    {
        // If we have more sentences left in the array...
        if (index < dialogue.Length - 1)
        {
            index++; // Move to the next element
            dialogueText.text = ""; // Clear current text

            // Stop the typing effect if it's still running
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(Typing());



        }
        else
        {
            // NO MORE SENTENCES LEFT:
            QuestManager.Instance.MarkSeen("conversation");

            zeroText();       // This turns off the dialoguePanel
            //QuestManager.Instance.MarkSeen(dialogueID);
        }
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
            playerIsClose = false;
            zeroText();
            Bubble.enabled = false;
        }
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional if you want it persistent
    }

    // Mark a dialogue as seen
    public void MarkSeen(string dialogueID)
    {
        var dialogue = database.GetDialogue(dialogueID);
        if (dialogue != null)
        {
            dialogue.seen = true;
            //GameManager.Instance.completedDialogues.Add(dialogueID); // keep persistent save state
            //GameManager.Instance.unsavedChanges = true;
        }
    }

    // Reset a dialogue
    public void ResetDialogue(string dialogueID)
    {
        var dialogue = database.GetDialogue(dialogueID);
        if (dialogue != null)
        {
            dialogue.seen = false;
            GameManager.Instance.completedDialogues.Remove(dialogueID);
            GameManager.Instance.unsavedChanges = true;
        }
    }

    // Check if dialogue has been seen
    public bool HasSeen(string dialogueID)
    {
        var dialogue = database.GetDialogue(dialogueID);
        return dialogue != null && dialogue.seen;
    }

    // Reset all dialogues
    public void ResetAll()
    {
        foreach (var d in database.dialogues)
            d.seen = false;

        GameManager.Instance.completedDialogues.Clear();
        GameManager.Instance.unsavedChanges = true;
    }

    public void Begin()
    {
        Quest.isDialogueActive = true;

        // 1. Check the SO directly to see if the intro is done
        var introEntry = database.GetDialogue("conversation");
        bool introFinished = introEntry != null && introEntry.seen;

        // 2. LOGIC PRIORITY
        if (!introFinished)
        {
            // Force the introduction if never seen
            dialogueID = "conversation";
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

        // 3. FETCH AND DISPLAY
        var finalEntry = database.GetDialogue(dialogueID);
        if (finalEntry != null)
        {
            dialogue = finalEntry.conversation;
            index = 0;
            dialoguePanel.SetActive(true);
            StopAllCoroutines();
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
    }
}
