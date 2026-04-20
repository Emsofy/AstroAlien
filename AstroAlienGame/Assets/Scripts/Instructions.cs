using TMPro;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public TextMeshProUGUI instructions;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructions.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructions.enabled = false;
        }
    }
}
