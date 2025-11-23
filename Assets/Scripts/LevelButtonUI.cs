using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public bool isLocked;
    public int levelCount;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI levelText;
    private LevelSelectionView levelManager;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }
    public void InitializeUI(bool isCleared,int levelIndex,LevelSelectionView _manager)
    {
        gameObject.SetActive(true);
        levelCount = levelIndex;
        levelManager = _manager;
        if (isCleared)
        {
            lockIcon.SetActive(false);
            button.onClick.AddListener(ButtonOnClick);
            levelText.gameObject.SetActive(true);
            levelText.text = (levelCount + 1).ToString();
        }
        else
        {
            lockIcon.SetActive(true);
            levelText.gameObject.SetActive(false);
        }
    }
    void ButtonOnClick()
    {
        levelManager.LevelSelected(levelCount);
    }

}
