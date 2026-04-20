using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreeSaveData
{
    public string id;
    public Vector3 position;
    public long endTimeTicks;
    public string Appletag;
    public bool applegiven;
    public int stage;
}
[Serializable]
public class ChickenSaveData
{
    public string id;
    public Vector3 position;
    public long nextEggTicks;
   // public bool eggLayed;
}
[Serializable]
public class EggSaveData
{
    public string id;
    public Vector3 position;
    public string Eggtag;

}
//public class RepairSaveData
//{
//    public int currentRepairLevel;
//    public bool repairing;
//    public long endTimeTicks;
//}


//public class InventorySaveData
//{
//}
public class SaveData
{
    //public int playerLevel;
   // public Vector3 playerPosition;
    public long lastLoginTime;
    public long lastOfflineSeconds;
    public List<string> completedDialogues;
    public List<TreeSaveData> trees;
    public List<ChickenSaveData> chickens;
    public List <EggSaveData> eggs;
    // public InventorySaveData inventorySaveData;
    public int appleCount;
    public int chickenCount;
    public int woodCount;
    public int seedCount;
    public int goldenAppleCount;
    public int eggCount;
    public int goldenEggCount;
    public int scrapMetalCount;

    //repair data
    public int currentRepairLevel;
    public bool repairing;
    public long endTimeTicks;

}