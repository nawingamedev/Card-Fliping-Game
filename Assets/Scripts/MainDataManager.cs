using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class MainDataManager : MonoBehaviour
{
    public static MainDataManager instance;
    public MainData playerData;
    [SerializeField] private string fileName = "GameData.json";
    [SerializeField] private string folderName = "CardGame";
    [SerializeField] private LevelData levelData;
    private string filePath;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, folderName))){
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath,folderName));
        }
        filePath = Path.Combine(Application.persistentDataPath,folderName,fileName);
    }
    void Start()
    {
        LoadData();
    }
    void LoadData()
    {
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<MainData>(data);
        }
        else
        {
            SaveData(GenerateData());
        }
    }
    public void SaveData(MainData mainData)
    {
        playerData = mainData;
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath,data);
    }
    public void AddScore(int _score)
    {
        playerData.score += _score;
        SaveData(playerData);
    }
    MainData GenerateData()
    {
        List<LevelStats> levelStats = new();
        LevelStats stats;
        for (int i=0;i<levelData.levelsLists.Count;i++)
        {
            stats = new()
            {
                levelIndex = i,
                isCleared = i == 0
            };
            levelStats.Add(stats);
        }
        MainData mainData = new()
        {
            score = 0,
            levelInfo = levelStats  
        };
        return mainData;
    }
}
[System.Serializable]
public struct MainData
{
    public int score;
    public List<LevelStats> levelInfo;

}
[System.Serializable]
public struct LevelStats
{
    public int levelIndex;
    public bool isCleared;
}
