using UnityEditor;
using UnityEngine;

public class Reveal : MonoBehaviour
{
    [MenuItem("Tools/Reveal All Objects")]
    static void ReReveal()
    {
        foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            go.hideFlags = HideFlags.None;
        }
    }
}
