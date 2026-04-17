using TMPro;
using UnityEngine;
using System.Collections;

public class Dialogue2 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public int index;

    public GameObject buttonPrefab01;
    public GameObject buttonPrefab02;
    public Transform buttonParent;

    public bool canPressE;
    // private bool _isCursorLocked = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
        canPressE = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (canPressE == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index]; // Skip to end
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        index = 0;
        // Display E 
        canPressE = true;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length + 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            Destroy(textComponent);// End dialogue
        }



        /*if (index == 1)
        {
            //gameObject.SetActive(false);
            SpawnNewButton();
            canPressE = false;
            UnlockCursor();
           //make button do something

        }
        */
    }

    public void SpawnNewButton()
    {
        GameObject newButton01 = Instantiate(buttonPrefab01);

        newButton01.transform.SetParent(buttonParent, false);

        GameObject newButton02 = Instantiate(buttonPrefab02);

        newButton02.transform.SetParent(buttonParent, false);
    }

    /* public void LockCursor()
     {
         Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
         _isCursorLocked = true;
     }*/

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //_isCursorLocked = false;
    }
}