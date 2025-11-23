using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayView : UIBaseStates
{
    public override UIStateEnum stateName => UIStateEnum.GameplayState;
    [SerializeField] private LevelData levelData;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private GamePlayManager gamePlayManager;
    [SerializeField] private TextMeshProUGUI matchCountText, turnCountText, scoreCountText,comboCountText;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private float comboBaseY,comboTargetY,comboAnimTime;
    [SerializeField] private RectTransform comboRect;

    private int matchedCount
    {
        get => _matchedCount;
        set { _matchedCount = value; matchCountText.text = value.ToString(); }
    }
    private int _matchedCount;

    private int turnCount
    {
        get => _turnCount;
        set { _turnCount = value; turnCountText.text = value.ToString(); }
    }
    private int _turnCount;
    private int scoreCount
    {
        get => _scoreCount;
        set { _scoreCount = value; scoreCountText.text = value.ToString(); }
    }
    private int _scoreCount;
    private Coroutine coroutine;

    public override void EnterState()
    {
        gameObject.SetActive(true);
        gamePlayManager.gameObject.SetActive(true);
        InitializeGameplay();
    }

    public override void ExitState()
    {
        levelCompletePanel.SetActive(false);
        gamePlayManager.ResetGame();
        gamePlayManager.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    void OnEnable()
    {
        GamePlayManager.UpdateUI += UpdateUI;
        GamePlayManager.GetCombo += ComboAnimation;
        GamePlayManager.LevelCleared += LevelCleared;
    }
    void OnDisable()
    {
        GamePlayManager.UpdateUI -= UpdateUI;
        GamePlayManager.GetCombo -= ComboAnimation;
        GamePlayManager.LevelCleared -= LevelCleared;
    }
    void InitializeGameplay()
    {
        int selected = levelData.selectedLevel;
        int _rows = levelData.levelsLists[selected].rowCount;
        int _clmns = levelData.levelsLists[selected].columnCout;
        int _matches = levelData.levelsLists[selected].matchCount;
        int _scoreMulti = levelData.scoreMultiplier;
        gamePlayManager.InitializeLevel(_rows,_clmns,_matches,_scoreMulti);
        levelTxt.text = "LEVEL " + (levelData.selectedLevel+1);
    }
    void UpdateUI(int _match,int _turn,int _score)
    {
        matchedCount = _match;
        turnCount = _turn;
        scoreCount = _score;
    }
    void LevelCleared()
    {
        AudioManager.instance.Play2DClip("GameWin");
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            comboRect.anchoredPosition = comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,comboBaseY);
        }
        MainData mainData = MainDataManager.instance.playerData;
        mainData.levelInfo[levelData.selectedLevel + 1] = new()
        {
            isCleared = true,
            levelIndex = levelData.selectedLevel + 1
        };
        mainData.score += 100;
        MainDataManager.instance.SaveData(mainData);
        if(levelData.levelsLists.Count - 1 > levelData.selectedLevel){
            levelData.selectedLevel++;
        }
        levelCompletePanel.SetActive(true);
    }
    void ComboAnimation(int _comboCount)
    {
        comboCountText.text = "COMBO " + _comboCount + "X";
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            comboRect.anchoredPosition = comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,comboBaseY);
        }
        coroutine = StartCoroutine(ComboAnimation());
        
    }
    IEnumerator ComboAnimation()
    {
        float elapse = 0;
        while(elapse < comboAnimTime)
        {
            float tempY = Mathf.Lerp(comboBaseY,comboTargetY,elapse/comboAnimTime);
            comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,tempY);
            elapse += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        elapse = 0;
        while(elapse < comboAnimTime)
        {
            float tempY = Mathf.Lerp(comboTargetY,comboBaseY,elapse/comboAnimTime);
            comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,tempY);
            elapse += Time.deltaTime;
            yield return null;
        }
    }
    public void RestartGame()
    {
        AudioManager.instance.Play2DClip("ButtonClick");
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            comboRect.anchoredPosition = comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,comboBaseY);
        }
        UpdateUI(0,0,0);
        levelCompletePanel.SetActive(false);
        gamePlayManager.ResetGame();
        InitializeGameplay();
    }
    public void Home()
    {
        AudioManager.instance.Play2DClip("ButtonClick");
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            comboRect.anchoredPosition = comboRect.anchoredPosition = new Vector2(comboRect.anchoredPosition.x,comboBaseY);
        }
        UpdateUI(0,0,0);
        levelCompletePanel.SetActive(false);
        UIStateManager.instance.ChangeState(UIStateEnum.DashboardState);
    }
}
