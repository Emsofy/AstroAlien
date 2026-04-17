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
}
[Serializable]
public class InventorySaveData
{
    public int scrapMetal;
    public int wood;
    public int seeds;
    public int eggs;
    public int goldEggs;
    public int fruit;
    public int goldFruit;
    public int chickens;
}
public class SaveData
{
    public int playerLevel;
    public Vector3 playerPosition;
    public long lastLoginTime;
    public long lastOfflineSeconds;
    public List<string> completedDialogues;
    public List<TreeSaveData> trees;
    public List<ChickenSaveData> chickens;
    public InventorySaveData inventorySaveData;
}