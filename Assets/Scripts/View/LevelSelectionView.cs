using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionView : UIBaseStates
{
    [SerializeField] LevelData levelData;
    [SerializeField] GameObject levelUIPrefab;
    [SerializeField] Transform levelPanel;
    private List<GameObject> levelUIList = new();
    public override UIStateEnum stateName => UIStateEnum.LevelSelectionState;

    public override void EnterState()
    {
        gameObject.SetActive(true);
        GenerateUI();
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
        ClearAll();
    }
    void GenerateUI()
    {
        List<LevelStats> savedData = MainDataManager.instance.playerData.levelInfo;
        for (int i=0;i<levelData.levelsLists.Count;i++)
        {
            GameObject level = Instantiate(levelUIPrefab,levelPanel);
            LevelButtonUI buttonUI = level.GetComponent<LevelButtonUI>();
            buttonUI.InitializeUI(savedData[i].isCleared,i,this);
            levelUIList.Add(level);
        }
    }
    void ClearAll()
    {
        foreach(var levelUI in levelUIList)
        {
            Destroy(levelUI);
        }
        levelUIList.Clear();
    }
    public void LevelSelected(int lvlIndex)
    {
        AudioManager.instance.Play2DClip("ButtonClick");
        levelData.selectedLevel = lvlIndex;
        UIStateManager.instance.ChangeState(UIStateEnum.GameplayState);
    }
    public void OnBackClick()
    {
        AudioManager.instance.Play2DClip("ButtonClick");
        UIStateManager.instance.ChangeState(UIStateEnum.DashboardState);
    }
}
