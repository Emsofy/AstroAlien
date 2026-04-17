using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class SaveSystem
{
    // Save file path
    private static string path => Application.persistentDataPath + "/save.json";


    public static void SaveGame()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        //update SaveData structure with values from runtime
        SaveData data = new SaveData
        {
            playerLevel = gm.playerLevel,
            playerPosition = gm.playerPosition,
            lastLoginTime = DateTime.UtcNow.Ticks,
            lastOfflineSeconds = gm.lastComputedOfflineSeconds,
            completedDialogues = new List<string>(gm.completedDialogues),
            trees = new List<TreeSaveData>(),
            chickens = new List<ChickenSaveData>(),
            //inventorySaveData = gm.inventory
        };
        foreach (var tree in gm.activeTrees)
        {
            data.trees.Add(tree.GetSaveData());
          
        }
        foreach(var chickenNew in gm.activeChicks)
        {
            data.chickens.Add(chickenNew.GetSaveData());
        }

        for (int i = 0; i <= gm.inventory.Length; i++)
        {
            if (i == 0)
            {
                //scrapmetal= gm.scrapmetal[i]??
            }
            if (i == 1)
            {
            }
            if (i == 2)
            {
            }
            if (i == 3)
            {
            }
            if (i == 4)
            {
            }
            if (i == 5)
            {
            }
            if (i == 6)
            {
            }
            if (i == 7)
            {
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Saved:\n" + json);
    }

    public static SaveData LoadGame()
    {
        if (!File.Exists(path)) return null; 	//catches first time game is played, where no save file or path exists yet

        string json = File.ReadAllText(path);
        Debug.Log("Loaded:\n" + json);

        return JsonUtility.FromJson<SaveData>(json);
        //we are simply reading the JSON file, but nothing has been updated in the game yet.
        //the actual values are updated in GameManager after this is loaded…
    }


    // Delete / Reset Save File

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }


    // Reset Everything (optional helper)
 
    public static void ResetAll()
    {
        DeleteSave();
        var gm = GameManager.Instance;
        if (gm != null)
        {
            gm.ResetMemory(); // clear runtime state
        }
        Debug.Log("All game data reset");
    }
}