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

    public string currentDialogueID;

    [Header("CoolDown")]
    public float Cooldown = 1.5f;
    private float nextInteractTime = 0f;
    

    public static bool AnyDialogueRunning = false;

    private void Start()
    {
        //if (Quest == null) Quest = FindAnyObjectByType<QuestManager>();
        
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyUp(KeyCode.E) && !isTyping && Time.time > nextInteractTime)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                NextLine();
            }
            else
            {
                Begin();
            }
        }
    }

   public NPC currentNPC;
    public void StartDialogue(string[] newDialogue, string id)
    {
        currentDialogueID = id;
        dialogue = newDialogue;
        index = -1;
        dialoguePanel.SetActive(true);
        NextLine();
        // if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        //dialogueText.text = "";
        QuestManager.Instance.MarkSeen(currentDialogueID);
        //typingCoroutine = StartCoroutine(Typing());
        Debug.Log("hit");
    }
    public void Begin()
    {
        var finalEntry = database.GetDialogue(dialogueID);
        dialogueID = currentNPC.GetCurrentDialogueID();
    

       if (finalEntry != null)
        {
        StartDialogue(finalEntry.conversation, dialogueID);
            AnyDialogueRunning = true;
        }
       
            /*if (index >= 0 && index < dialogue.Length)
            {
                dialogueText.text = dialogue[index];
            }
            AnyDialogueRunning = true;
            dialogue = finalEntry.conversation;
            //index = -1;

            dialoguePanel.SetActive(true);*/

        
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
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.MarkSeen(currentDialogueID);
            }
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