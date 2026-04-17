using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public int playerLevel = 1;
    public Vector3 playerPosition;


    [Header("Trees")]
    public GameObject treePrefab;
    public List<TreeGrow> activeTrees = new List<TreeGrow>();
    public TreeCollect treeScript;

    [Header("Chickens")]
    public GameObject chickenPrefab;
    public List<ChickenMov> activeChicks = new List<ChickenMov>();

    [Header("Dialogue")]
    public HashSet<string> completedDialogues = new HashSet<string>();

    [Header("Time (debug view)")]
    public int lastComputedOfflineSeconds;


    [Header("Dirty Flag")]
    public bool unsavedChanges = false;

    [Header("Chicken")]
    public ChickenCollect chickenScript;
    public GameObject Player;

    [Header("Inventory")]
    public int chickenCount;
    public int seedCount;




    public int[] inventory = new int[8];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGame();
        
    }
    public void Start()
    {
        chickenScript = Player.GetComponent<ChickenCollect>();
        treeScript = Player.GetComponent<TreeCollect>();

        //inventory = 
    }

    private void Update()
    {
       chickenCount = chickenScript.PickUpchicken(chickenCount);
       seedCount = treeScript.PlaceTree(seedCount);
        seedCount = treeScript.ChopTree(seedCount);
    }

   

    public void SaveGame()
    {
        SaveSystem.SaveGame();
        unsavedChanges = false; //Or MarkClean();
    }

    public void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();
        if (data == null) return;

        playerLevel = data.playerLevel;
        playerPosition = data.playerPosition;

        completedDialogues = new HashSet<string>(data.completedDialogues ?? new List<string>());

        HandleOfflineTime(data.lastLoginTime);

        // Apply data to scene objects (player, dialogue flags)
        ApplyToScene();
        LoadTrees(data);
        LoadChickens(data);
        unsavedChanges = false; // loaded state is clean
    }

    // New method
    private void ApplyToScene()
    {
        // Player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.transform.position = playerPosition;

        // DialogueManager
        if (DialogueManager.Instance != null)
        {
            foreach (var id in completedDialogues)
            {
                DialogueManager.Instance.MarkSeen(id); // updates scene dialogue objects
            }
        }
    }
    public void UpdatePlayerPosition(Vector3 newPos)
    {
        if (Vector3.Distance(playerPosition, newPos) > 0.000001f)
        {
            playerPosition = newPos;
            unsavedChanges = true;
        }
    }
    public void HandleOfflineTime(long ticks)
    {
        DateTime lastLogin = new DateTime(ticks);
        TimeSpan timeAway = DateTime.UtcNow - lastLogin;

        int seconds = Mathf.Min((int)timeAway.TotalSeconds, 86400);
        lastComputedOfflineSeconds = seconds;

        Debug.Log("Offline seconds: " + seconds);
        unsavedChanges = true; //or MarkDirty();
    }

    public void HandleDebugTime(int seconds)
    {
        DateTime fakePast = DateTime.UtcNow.AddSeconds(-seconds);
        HandleOfflineTime(fakePast.Ticks);
    }

    // Dialogue helpers
    public void MarkDialogue(string id)
    {
        completedDialogues.Add(id);
        unsavedChanges = true; //or MarkDirty();
    }

    public void ResetDialogue(string id)
    {
        completedDialogues.Remove(id);
        unsavedChanges = true; //or MarkDirty();
    }

    public bool HasSeenDialogue(string id) => completedDialogues.Contains(id);
    public void ResetMemory()
    {
        playerPosition = Vector3.zero;
        completedDialogues.Clear();
        lastComputedOfflineSeconds = 0;
        unsavedChanges = true;
        Debug.Log("Memory Reset");
    }
    public void ResetSaveFile()
    {
        SaveSystem.DeleteSave();
        ResetMemory();
        Debug.Log("Disk Save Reset");
    }
    private void MarkDirty()
    {
        unsavedChanges = true;
    }
    private void MarkClean()
    {
        unsavedChanges = false;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyToScene();
    }

    public void LoadTrees(SaveData data)
    {
    
        foreach (var tree in activeTrees)
        {
            if(tree != null)
            {
                Destroy(tree.gameObject);
            }
        }
        activeTrees.Clear();
        if (data.trees == null) return;

        foreach (var treeData in data.trees)
        {
            GameObject obj = Instantiate(treePrefab, treeData.position, treePrefab.transform.rotation);

            TreeGrow tree = obj.GetComponent<TreeGrow>();
            tree.Init(treeData);
            obj.tag = tree.Appletag;
            activeTrees.Add(tree);
           
        }
        Debug.Log("Loaded " + activeTrees.Count + "trees");
    }
    public void PlantTree(Vector3 position)
    {
        GameObject obj = Instantiate(treePrefab, position, treePrefab.transform.rotation);

        TreeGrow treegrowS = obj.GetComponent<TreeGrow>();
        treegrowS.StartNew(position);

        activeTrees.Add(treegrowS);

        SaveSystem.SaveGame();
    }
    
    public List<TreeSaveData> GetTreeSaveData()
    {
        List<TreeSaveData> treeDataList = new List<TreeSaveData>();
        foreach (var tree in activeTrees)
        {
            if(tree != null)
            {
                treeDataList.Add(tree.GetSaveData());

            }
        }
        return treeDataList;
    }
    public void LoadChickens(SaveData data)
    {
        foreach (var chicken in activeChicks)
        {
            if (chicken != null)
            {
                Destroy(chicken.gameObject);

            }
        }
            activeChicks.Clear();
            if (data.chickens == null) return;
            foreach(var chickenData in data.chickens)
            {
                GameObject obj = Instantiate(chickenPrefab, chickenData.position,chickenPrefab.transform.rotation);
                ChickenMov chickenNew = obj.GetComponent<ChickenMov>();
                chickenNew.Init(chickenData);
                activeChicks.Add(chickenNew);
            }
        
        Debug.Log("Loaded " + activeChicks.Count + "chickens");
    }
    public void PlaceChicken(Vector3 position)
    {

        GameObject obj = Instantiate(chickenPrefab, position, chickenPrefab.transform.rotation);
        ChickenMov chickenNew = obj.GetComponent<ChickenMov>();
        chickenNew.StartNew(position);
        activeChicks.Add(chickenNew);
        SaveSystem.SaveGame();
      
    }
    public List<ChickenSaveData> GetChickenSaveData()
    {
        List<ChickenSaveData> chickenDataList = new List<ChickenSaveData>();
        foreach (var chickenNew in activeChicks)
        {
            if (chickenNew != null)
            {
                chickenDataList.Add(chickenNew.GetSaveData());

            }
        }
        return chickenDataList;
    }


}