using UnityEngine;
using System.Collections.Generic;

public class RepairMech : MonoBehaviour
{
    public bool hasScrap;
    public bool inRepairZone;
    public bool repairing;

    
    public int repairNumber;

    public GameObject RepairPromptTXT;
    public GameObject yesRepairTXT;
    public GameObject noRepairTXT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasScrap = false;
        inRepairZone = false;
        repairNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasScrap && inRepairZone && Input.GetKeyDown(KeyCode.Y))
        {
            //begin repair
            Debug.Log("repair has begun");
            RepairPromptTXT.SetActive(false);
            yesRepairTXT.SetActive(true);
            SmallRepair();
        }
        if (hasScrap && inRepairZone && Input.GetKeyDown(KeyCode.N))
        {
            //exit repair
            Debug.Log("repair cancelled");
            RepairPromptTXT.SetActive(false);
        }

        if(!hasScrap && inRepairZone)
        {
            RepairPromptTXT.SetActive(false);
            noRepairTXT.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("ScrapMetal"))
        {
           
            hasScrap = true;
        }

            if (other.CompareTag("RepairZone"))
        {
            inRepairZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RepairZone"))
        {
            inRepairZone = false;
            noRepairTXT.SetActive(false);
            yesRepairTXT.SetActive(false);
        }
    }


    void SmallRepair()
    {
        repairing = true;
        repairNumber++; 
    }
}
