using UnityEngine;

public class RepairTrigger : MonoBehaviour
{
    public GameObject popUpText; //connect in inspector
    public GameObject savePopUp;

    public bool inSaveZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inSaveZone = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (inSaveZone && savePopUp.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            SaveSystem.SaveGame();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSaveZone = true;
            popUpText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.N))
            {
                popUpText.SetActive(false);
                savePopUp.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popUpText.SetActive(false);

        }
    }
}
