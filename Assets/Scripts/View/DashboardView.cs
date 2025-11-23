using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashboardView : UIBaseStates
{
    public override UIStateEnum stateName => UIStateEnum.DashboardState;

    public override void EnterState()
    {
        gameObject.SetActive(true);
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
