using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move != Vector3.zero)
        {
            transform.position += move * Time.deltaTime * 5f;

            // Notify GameManager of the new position
            GameManager.Instance.UpdatePlayerPosition(transform.position);
        }

    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdatePlayerPosition(transform.position);
        }
    }
}