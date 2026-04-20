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
    public string currentDialogueID;
    public string[] dialogue;
    public int index;
    private bool isTyping = false;
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine typingCoroutine;
    public TextMeshProUGUI Bubble;
    public bool hasMet = false;

    [Header("CoolDown")]
    public float Cooldown = 1.5f;
    private float nextInteractTime = 0f;
    public static bool AnyDialogueRunning = false;

    public NPC currentNPC;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();
        dialoguePanel.SetActive(false);
  
        hasMet = PlayerPrefs.GetInt("HasMetAlien", 0) == 1;
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyUp(KeyCode.E) && !isTyping && Time.time > nextInteractTime)
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

    public void Begin()
    {
        if (currentNPC != null)
        {
            
            if (!hasMet)
            {
                currentDialogueID = "Alien_Intro";
            }
            else
            {
                
                currentDialogueID = currentNPC.GetCurrentDialogueID();
            }

            dialogueID = currentDialogueID;
            var finalEntry = database.GetDialogue(dialogueID);

            if (finalEntry != null)
            {
                StartDialogue(finalEntry.conversation, dialogueID);
                AnyDialogueRunning = true;
            }
            else
            {
                Debug.LogError($"Dialogue ID {dialogueID} not found");
            }
        }
    }

    public void StartDialogue(string[] newDialogue, string id)
    {
        currentDialogueID = id;
        dialogue = newDialogue;
        index = -1;
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
            // End conversation
            if (QuestManager.Instance != null) QuestManager.Instance.MarkSeen(currentDialogueID);
            OnDialogueComplete(currentDialogueID);
            zeroText();
        }
    }

    public void OnDialogueComplete(string id)
    {
        string cleanID = id.Trim();
        Debug.Log("Finished Dialogue ID: " + cleanID);

        if (cleanID == "Alien_Intro")
        {
            hasMet = true;
            PlayerPrefs.SetInt("HasMetAlien", 1);
            PlayerPrefs.Save();

            if (!QuestManager.Instance.activeQuests.Contains("SpecialFruit"))
            {
                QuestManager.Instance.activeQuests.Add("SpecialFruit");
            }
            Debug.Log("Intro finished, quest startted.");
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
        index = -1;
        nextInteractTime = Time.time + Cooldown;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (Quest != null) Quest.isDialogueActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (Bubble != null) Bubble.enabled = true;
            currentNPC = other.GetComponent<NPC>();

            if (currentNPC == null)
            {
                currentNPC = GetComponentInParent<NPC>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Bubble != null) Bubble.enabled = false;
            playerIsClose = false;
            currentNPC = null;
        }
    }
}