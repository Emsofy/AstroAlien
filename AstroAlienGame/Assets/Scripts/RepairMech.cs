using UnityEngine;
using System.Collections.Generic;
using System;

public class RepairMech : MonoBehaviour
{
    [Header("Repair Bools")]
    public bool hasScrap;
    public bool inRepairZone;
    public bool repairing;

    [Header("Scaling Vars")]
    public int baseCost = 2;
    public float baseRepairTime = 10f; //replace w hours later

    public int currentRepairLevel = 0;
    public int currentCost;
    public float currentRepairTime;

    [Header("Time Vars")]
    private DateTime endTime;
    private TimeSpan repairDuration; 
    //public float repairTimer = 0f;

    [Header("Text Vars")]
    public GameObject RepairPromptTXT;
    public GameObject yesRepairTXT;
    public GameObject noRepairTXT;
    public GameObject repairingTXT;



    //NEED PERSISTENCE
    //endTime
    //currentRepairLevel
    //repairing
    public void Init (RepairSaveData data)
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasScrap = false;
        inRepairZone = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        hasScrap = GameManager.Instance.scrapMetalCount > 0;

        currentCost = baseCost + currentRepairLevel;
        currentRepairTime = baseRepairTime + (currentRepairLevel * 5f);

        //redo logic for start

        //if has scrap and in repair zone and not repairing/ prompt player
        //if is repairing notify player 
        //if says yes/ check cost
        // if have enough/ start repair 
        //repair runs, repair level go up
        if (inRepairZone && !repairing)
        {
            RepairPromptTXT.SetActive(true);
        }
        if (hasScrap && inRepairZone && Input.GetKeyDown(KeyCode.Y) && !repairing) 
        {
            RepairPromptTXT.SetActive(false);
            CheckCost(currentCost);
        }
        if (inRepairZone && repairing) //alert player repair is underway
        {
            repairingTXT.SetActive(true);
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

       if (repairing)
       {
                TimeSpan remaining = endTime - DateTime.UtcNow; //calculate time left
                if (remaining < TimeSpan.Zero) 
                    remaining = TimeSpan.Zero;
                if (remaining <= TimeSpan.Zero)
                {
                    CompleteRepair();
                }
       }



    }

    private void OnTriggerEnter(Collider other)
    {
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
            repairingTXT.SetActive(false) ;
        }
    }

    public void CheckCost(int cost)
    {
        //if scraps => cost we can repair 
        if (GameManager.Instance.scrapMetalCount >= cost)
        {
            GameManager.Instance.scrapMetalCount -= cost;
            SmallRepair(currentRepairTime);
        }
        else
        {
            Debug.Log("need more scrap!");
        }
    }

    public void SmallRepair(float repairTime)
    {
        repairing = true;
        if (inRepairZone) yesRepairTXT.SetActive(true);

        repairDuration = TimeSpan.FromMinutes(repairTime); //sets timer
        //will take 2 mins to repair, switch later
        endTime = DateTime.UtcNow.Add(repairDuration); //calculate end time
      
    }


    public void CompleteRepair()
    {
        if (!repairing) return;

        repairing = false;
        
        currentRepairLevel++;
        Debug.Log("repair completed");
    }


    public RepairSaveData GetSaveData()
    {
        return new RepairSaveData
        {
            //NEED PERSISTENCE
            //endTime
            //currentRepairLevel
            //repairing
            currentRepairLevel = currentRepairLevel,
            repairing = repairing,
            endTime = endTime

        };
    }
}
