using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YetiDialogue : MonoBehaviour
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


    private void Start()
    {
        //if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();
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

        if (!hasMet)
        {
            currentDialogueID = "Yeti_Intro";
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

    public void StartDialogue(string[] newDialogue, string id)
    {
        currentDialogueID = id;
        dialogue = newDialogue;
        index = 0;
        dialoguePanel.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(Typing());
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
            // End convo
            if (QuestManager.Instance != null)
                OnDialogueComplete(currentDialogueID);
            zeroText();
        }
    }

    public void OnDialogueComplete(string id)
    {
        if (id == "Yeti_Intro")
        {
            hasMet = true;
            PlayerPrefs.SetInt("HasMetAlien", 1);
            PlayerPrefs.Save();

            if (!QuestManager.Instance.activeQuests.Contains("SpecialFruit"))
            {
                QuestManager.Instance.activeQuests.Add("SpecialFruit");
                QuestManager.Instance.SaveData();
            }
        }
        QuestManager.Instance.MarkSeen(currentDialogueID);
        if (id == "SpecialFruit_Complete")
        {
            GameManager.Instance.RemoveApple(1);
            QuestManager.Instance.activeQuests.Remove("SpecialFruit");
            QuestManager.Instance.Finished = true;
            hasMet = true;
            QuestManager.Instance.MarkSeen(currentDialogueID);
            GameManager.Instance.AddScrap(1);
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

            if (currentNPC == null) currentNPC = GetComponentInParent<NPC>();

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