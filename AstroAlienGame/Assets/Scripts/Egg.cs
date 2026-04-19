using System;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public string id;
    public string Eggtag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(EggSaveData data)
    {
        id = data.id;
        transform.position = data.position;
        Eggtag = data.Eggtag;
        //SetEgg();
    }
    public void StartNew(Vector3 position)
    {
        id =  Guid.NewGuid().ToString();
        transform.position = position;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EggProb();
        SetEgg();
    }
    public void EggProb()
    {
        if(gameObject.tag == "Untagged")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            int eggProb = UnityEngine.Random.Range(1, 11);
            Debug.Log(eggProb);
            if (eggProb > 2)
            {
                // Vector3 spawnPos = transform.position + transform.right * UnityEngine.Random.Range(-0.5f, 0.5f);
                //Instantiate(eggPrefab, spawnPos, Quaternion.identity);
                //GameManager.Instance.SpawnEgg(spawnPos);
                gameObject.tag = "Egg";
                transform.GetChild(0).gameObject.SetActive(true);
                Debug.Log("Egg laid");

            }
            else
            {
                //Vector3 spawnPos = transform.position + transform.right * UnityEngine.Random.Range(-0.5f, 0.5f);
                //Instantiate(goldEggPrefab, spawnPos, Quaternion.identity);
                //GameManager.Instance.SpawnEgg(spawnPos);
                gameObject.tag = "GoldenEgg";
                transform.GetChild(1).gameObject.SetActive(true);
                Debug.Log("Golden egg laid");
            }
            SaveSystem.SaveGame();
        }
        else
        {
           // Debug.Log("Egg is already tagged");
        }
       
        
    }
    public void SetEgg()
    {
       // Debug.Log("ran set egg");
        if(gameObject.tag == "Egg")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            //Debug.Log("set egg 1 active");
        }
        if(gameObject.tag == "GoldenEgg")
        {
            transform.GetChild(1).gameObject.SetActive(true);
            //Debug.Log("set egg 2 active");
        }
        else
        {
           // Debug.Log("fuck you");
        }
    }
    public EggSaveData GetSaveData()
    {
        return new EggSaveData
        {
            id = id,
            position = transform.position,
            Eggtag = gameObject.tag


        };
    }
}
