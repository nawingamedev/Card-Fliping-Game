using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashboardView : UIBaseStates
{
    public override UIStateEnum stateName => UIStateEnum.DashboardState;
    [SerializeField] TextMeshProUGUI scoreText;

    public override void EnterState()
    {
        gameObject.SetActive(true);
        scoreText.text = MainDataManager.instance.playerData.score.ToString();
    }

    public override void ExitState()
    {
        gameObject.SetActive(false);
    }
    public void OpenLevelPage()
    {
        UIStateManager.instance.ChangeState(UIStateEnum.LevelSelectionState);
    }
}
