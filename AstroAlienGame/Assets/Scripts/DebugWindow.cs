using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class DebugWindow : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("=== DEBUG STATE ===");
        sb.AppendLine("Player Position: " + gm.playerPosition);
        sb.AppendLine("Offline Seconds: " + gm.lastComputedOfflineSeconds);


        sb.AppendLine("\nDialogues:");
        foreach (var d in gm.completedDialogues)
        {
            sb.AppendLine("- " + d);
        }

        sb.AppendLine("\nUnsaved Changes: " + (gm.unsavedChanges ? "YES" : "NO"));

        debugText.text = sb.ToString();
    }
}