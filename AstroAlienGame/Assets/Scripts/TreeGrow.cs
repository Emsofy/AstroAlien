using UnityEngine;
using System;

public class TreeGrow : MonoBehaviour
{
    public string id;
    private DateTime endTime;
    private TimeSpan totalDuration;

    private int currentStage = 0;

    public bool applegiven = false;
    public string Appletag;

    private bool fullyGrownTriggered = false;

    //called when loading game 
    public void Init (TreeSaveData data)
    {
        id = data.id;
        transform.position = data.position;
        endTime = new DateTime(data.endTimeTicks);
        Appletag = data.Appletag; 
        applegiven = data.applegiven;
        totalDuration = TimeSpan.FromMinutes(4);
        // Restore stage visually
        currentStage = data.stage;
        SetStage(currentStage, true); // force set instantly
    }
    // when the tree gets planted 
    public void StartNew(Vector3 position)
    {
        id = Guid.NewGuid().ToString();
        transform.position = position;

        totalDuration = TimeSpan.FromMinutes(4);
        endTime = DateTime.UtcNow.Add(totalDuration); //change to hours later

        SetStage(0, true); //starts as sprout
    }

    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        TimeSpan remaining = endTime - DateTime.UtcNow;

        //Clamping so no neg times
        if(remaining < TimeSpan.Zero)
            remaining = TimeSpan.Zero;
        //progress goes from 0 to 1
        float progress = 1f - (float)(remaining.TotalSeconds / totalDuration.TotalSeconds);
        UpdateGrowthStage(progress);
        if (progress >= 1f && !fullyGrownTriggered)
        {
            FullyGrown();
            //applegiven = true;
            fullyGrownTriggered |= true;
        }
       
    }
    void UpdateGrowthStage(float progress)
    {
        if(progress >= 1f && currentStage < 3)
        { 
            SetStage(3);
        }
        else if (progress >= 0.5f && currentStage < 2)
        {
            SetStage(2);
        }
        else if(progress >= 0.25f && currentStage <1)
        {
            SetStage(1);
        }
    }
    void SetStage(int stage, bool force = false)
    {
        if (!force && stage == currentStage) return;
        //turn off everything
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //Turn on right stage
        transform.GetChild(stage).gameObject.SetActive(true);
        currentStage = stage;

        SaveSystem.SaveGame();
    }
    void FullyGrown()
    {
        //Debug.Log("Tree has grown");
        if(gameObject.tag == "Untagged")
        {
            int appleProb = UnityEngine.Random.Range(1, 11);
            Debug.Log(appleProb);
            if (appleProb > 2)
            {
                gameObject.tag = "AppleTree";
                Debug.Log("changed tag to " + gameObject.tag);

            }
            else
            {
                gameObject.tag = "GoldenTree";
                SetStage(4);
                Debug.Log("changed tag to " + gameObject.tag);
            }
            applegiven = true;
            SaveSystem.SaveGame();

        }
        else
        {
            Debug.Log("trees already tagged");
        }
        
        
    }
    //saves the time left and position 
    public TreeSaveData GetSaveData()
    {
        return new TreeSaveData
        {
            id = id,
            position = transform.position,
            endTimeTicks = endTime.Ticks,
            Appletag = gameObject.tag,
            applegiven= applegiven,
            stage = currentStage 
        };
    }
    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}
    public void SetRemainingSeconds(int seconds)
    {
        endTime = DateTime.UtcNow.AddSeconds(seconds);
    }

    public int GetRemainingSeconds()
    {
        return (int)(endTime - DateTime.UtcNow).TotalSeconds;
    }

    public void ForceGrow()
    {
        endTime = DateTime.UtcNow;
    }
}
