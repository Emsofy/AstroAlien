using UnityEngine;

public class DestroyDebugger : MonoBehaviour
{
    // This runs automatically when the object is destroyed
    void OnDestroy()
    {
        // This will print a clickable link in your Console
        // describing exactly what called Destroy()
        Debug.Log($"{name} is being destroyed! Check the stack trace below:", this);
    }
}
