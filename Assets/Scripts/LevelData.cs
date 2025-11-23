using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelScriptable/LevelData")]
public class LevelData : ScriptableObject
{
    public int selectedLevel;
    public int scoreMultiplier;
    public List<LevelsList> levelsLists;
}
[System.Serializable]
public struct LevelsList
{
    public int rowCount;
    public int columnCout;
    public int matchCount;
}
