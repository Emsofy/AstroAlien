using System.Security.Cryptography;
using UnityEngine;

public class FruitPickUp : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other)
    {
       if(Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
            GameManager.Instance.appleCount++;
        }
    }
}
