using System;
using UnityEngine;

public class SnowCollect : MonoBehaviour
{
    public float fillDuration = 60f; //switch to hrs later

    public bool isPlaced = false;
    public bool isCollected = false;

    private DateTime fillStartTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (isPlaced && !isCollected)
        {
            //secondPassed = (DateTime.UtcNow - fillStartTime)
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SnowZone") && !isPlaced)
        {
            StartSnowCollect();
        }
    }

    private void StartSnowCollect()
    {
        isPlaced = true;
        fillStartTime = DateTime.UtcNow;

    }

    private void SnowCollected()
    {
        isCollected = true;
        Debug.Log("Bowl is full!");
    }
    //player gets bowl from alien
    //place bowl in snow terrain
    //if bowl is placed, begin snow fill timer 
    //timer persists while game is closed
    //after timer, bowl is filled
    //new item tag?
}
