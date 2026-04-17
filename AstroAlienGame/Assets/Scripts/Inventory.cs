using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int[] inventory = new int[8];
    public GameObject[] imageDisplay = new GameObject[8];
    public TreeCollect treescript;
    //[0] = scrap metal
    //[1] = wood          store the amount in each position
    //[2] = seed
    //[3] = eggs
    //[4] = Gold egg
    //[5] = fruit
    //[6] = fruitGold
    //[7] = chikin
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        treescript = GetComponent<TreeCollect>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //UpdateInventory(1, treescript.woodCount);
    }
    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("ScrapMetal")) //look for scraptag
    //    {
    //        Debug.Log("You Collected Scrap Metal!");
    //        Destroy(other.gameObject);
    //       // AddItem(0, 1);   //adding +1 to scrap metal holder (0)
    //        UpdateInventory(0, 1);
    //    }
    //    if (other.CompareTag("Wood")) //1
    //    {
    //        Debug.Log("You Collected Spare Wood!");
    //        Destroy(other.gameObject);
    //        //AddItem(1, 1);
    //        UpdateInventory(1, 1);
    //    }
    //    if (other.CompareTag("Seed")) //2
    //    {
    //        Debug.Log("You Collected Seeds!");
    //        Destroy(other.gameObject);
    //        //AddItem(2, 1);
    //        UpdateInventory(2, 1);
    //    }
    //    if (other.CompareTag("Egg")) //3
    //    {
    //        Debug.Log("You Collected and Egg!");
    //        Destroy(other.gameObject);
    //        //AddItem(3, 1);
    //        UpdateInventory(3, 1);
    //    }
    //    if (other.CompareTag("GoldEgg")) //4
    //    {
    //        Debug.Log("You Collected a Golden Egg!");
    //        Destroy(other.gameObject);
    //        //AddItem(4, 1);
    //        UpdateInventory(4, 1);
    //    }
    //    if (other.CompareTag("Fruit")) //5
    //    {
    //        Debug.Log("You Collected a Fruit!");
    //        Destroy(other.gameObject);
    //        //AddItem(5, 1);
    //        UpdateInventory(5, 1);
    //    }
    //    if (other.CompareTag("GoldFruit")) //6
    //    {
    //        Debug.Log("You Collected a Golden Fruit!");
    //        Destroy(other.gameObject);
    //        //AddItem(6, 1);
    //        UpdateInventory(6, 1);
    //    }
    //    if (other.CompareTag("Chicken")) //7
    //    {
    //        Debug.Log("You Collected a Chicken!");
    //        Destroy(other.gameObject);
    //        UpdateInventory(7, 1);
    //    }
       

    //}

   

    public void RemoveItem(int inventoryIndex, int amount)
    {
        if (inventory[inventoryIndex] >= amount) //if index is >= amount needed, 
        {
            inventory[inventoryIndex] -= amount; //then we can use those items
            Debug.Log("Used Item " + inventoryIndex);
        }
        else
        {
            Debug.Log("You do not have enough");
        }
    }
   

     public void UpdateInventory(int inventoryIndex, int amount)
    {

        //add amount to index
        inventory[inventoryIndex] += amount;

        //create variable for image and index to run checks
        GameObject slot = imageDisplay[inventoryIndex];
        Debug.Log("Slot is: " + slot);
        //hide at 0
        if (inventory[inventoryIndex] <= 0)
        {
            inventory[inventoryIndex] = 0;
            slot.SetActive(false);
            Debug.Log("returning");
            return;
        }
        

        
         Debug.Log("WHERES MY INVENTORY");
         slot.SetActive(true);
     

        //update child text to amount
        TMP_Text text = slot.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = inventory[inventoryIndex].ToString();
        }

        

        //[0] = scrap metal
        //[1] = wood          store the amount in each position
        //[2] = seed
        //[3] = eggs
        //[4] = Gold egg
        //[5] = fruit
        //[6] = fruitGold
        //[7] = chikin

        //inventory index is passed in, check if same index in Images is active in inspector 
        //if inactive, set to active
        //update display count text to amount 
    }


}
