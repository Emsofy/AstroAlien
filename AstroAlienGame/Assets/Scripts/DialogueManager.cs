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

   public NPC currentNPC; // Set this via the NPC's OnTriggerEnter

public void Begin() 
{
    // Ask the specific NPC for its current ID based on its inspector settings
    dialogueID = currentNPC.GetCurrentDialogueID();

    var finalEntry = database.GetDialogue(dialogueID);
    if (finalEntry != null) 
    {
        AnyDialogueRunning = true;
        Quest.isDialogueActive = true;
        dialogue = finalEntry.conversation;
        index = 0;
        dialoguePanel.SetActive(true);
        NextLine();
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