using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    private UIBaseStates currentState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeState(UIBaseStates targetState)
    {
        currentState?.ExitState();
        currentState = targetState;
        currentState.EnterState();
    }
}
public abstract class UIBaseStates : MonoBehaviour
{
    public UIStateEnum UIState;
    public abstract void EnterState();
    public abstract void ExitState();
}
public enum UIStateEnum
{
    DashboardState,
    LevelSelectionState,
    GameplayState,
    PauseState,
    GameEndState
}