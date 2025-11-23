using System.Collections.Generic;
using UnityEngine;

public class UIStateManager : MonoBehaviour
{
    public static UIStateManager instance;
    [SerializeField] private List<UIStateList> uiStates = new();
    private UIBaseStates currentState;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        ChangeState(uiStates[0].uiState);
    }
    public void ChangeState(UIBaseStates targetState)
    {
        currentState?.ExitState();
        currentState = targetState;
        currentState.EnterState();
    }
    public void ChangeState(UIStateEnum _newStateEnum)
    {
        foreach (var state in uiStates)
        {
            if (state.stateName == _newStateEnum)
            {
                ChangeState(state.uiState);
            }
        }
    }
}
[System.Serializable]
public struct UIStateList
{
    public UIStateEnum stateName;
    public UIBaseStates uiState;
}
public abstract class UIBaseStates : MonoBehaviour
{
    public abstract UIStateEnum stateName{ get; }
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