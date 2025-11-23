using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private GameObject gridPanel;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] faceSprites;
    [SerializeField] private Color[] faceColors;
    [SerializeField] private int matchCount; 
    [SerializeField] private int m_RowCount,m_ColumnCount;
    public delegate void UIUpdate(int machtCount,int turnCount,int score);
    public static UIUpdate UpdateUI;
    public delegate void ComboUI(int combo);
    public static ComboUI GetCombo;
    public delegate void GameComplete();
    public static GameComplete LevelCleared;

    private int comboCount = 1;
    private int scoreCounter;
    private int matchedCount;
    private int turnCount;
    private int scoreMultiplier;

    private GridLayoutGroup gridLayoutGroup;
    private List<GameObject> cards = new();
    private List<CardBehaviour> selectedCard = new();
    private HashSet<CardBehaviour> processingCards = new();


  //  private bool inputLocked = false;


    void Awake()
    {
        gridLayoutGroup = gridPanel.GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        
    }

    void OnEnable()
    {
        CardBehaviour.CardClicked += CardSelected;
    }
    void OnDisable()
    {
        CardBehaviour.CardClicked -= CardSelected;
    }
    public void InitializeLevel(int _row,int _columns,int matches,int _scoreMultiplier)
    {
        matchedCount = 0;
        turnCount = 0;
        scoreCounter = 0;
        scoreMultiplier = _scoreMultiplier;
        matchCount = matches;
        GenerateCards(_row, _columns);
    }

    void GenerateCards(int _rows, int _columns)
    {
        int count = _rows * _columns;

        count = count - (count % matchCount);

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = _columns;

        //Auto-size cards so they always fit the grid perfectly
        ResizeGridCells(_rows, _columns);


        List<int> cardSet = new();

        int totalGroups = count / matchCount;

        //Initializing no of cards and its copies
        for (int i = 0; i < totalGroups; i++)
        {
            for (int j = 0; j < matchCount; j++)
                cardSet.Add(i);
        }
        //Shuffling cards
        for (int i = 0; i < cardSet.Count; i++)
        {
            int random = Random.Range(i, cardSet.Count);
            (cardSet[i], cardSet[random]) = (cardSet[random], cardSet[i]);
        }

        //Creating Images and Card Buttons
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(cardPrefab, gridPanel.transform);
            CardBehaviour cb = obj.GetComponent<CardBehaviour>();

            cb.Initialize(faceSprites[cardSet[i]], cardSet[i]);
            cards.Add(obj);
        }

        StartCoroutine(ShowHideCards());
    }


    IEnumerator ShowHideCards()
    {
        //Show and Hide card before starting the game

        foreach (var card in cards)
            card.GetComponent<CardBehaviour>().FlipCard(true);

        yield return new WaitForSeconds(2.0f);

        foreach (var card in cards)
            card.GetComponent<CardBehaviour>().FlipCard(false);

    }
    private void ResizeGridCells(int rows, int columns)
    {
        RectTransform panelRect = gridPanel.GetComponent<RectTransform>();

        float panelW = panelRect.rect.width;
        float panelH = panelRect.rect.height;

        float spacingX = gridLayoutGroup.spacing.x;
        float spacingY = gridLayoutGroup.spacing.y;

        float totalSpacingX = spacingX * (columns - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellW = (panelW - totalSpacingX) / columns;
        float cellH = (panelH - totalSpacingY) / rows;

        // Perfect square cards 
        float finalSize = Mathf.Min(cellW, cellH);

        gridLayoutGroup.cellSize = new Vector2(finalSize, finalSize);
    }


    void CardSelected(CardBehaviour card)
    {
        if (processingCards.Contains(card)) return;   
        if (card.State != CardStates.FaceDown) return;
        if (selectedCard.Contains(card)) return;

        card.FlipCard(true);
        AudioManager.instance.Play2DClip("FlipCard");
        selectedCard.Add(card);

        
        if (selectedCard.Count == matchCount)
        {
            var snapshot = new List<CardBehaviour>(selectedCard);
            selectedCard.Clear();
            StartCoroutine(EvaluateSelectionContinuous(snapshot));
            selectedCard.Clear();
        }
    }


    IEnumerator EvaluateSelectionContinuous(List<CardBehaviour> _cardsInHold)
    {
        // Mark these cards as being processed
        foreach (var c in _cardsInHold)
            processingCards.Add(c);

        yield return new WaitUntil(() => _cardsInHold.All(c => c.State == CardStates.FaceUp));


        bool allMatch = true;
        int id = _cardsInHold[0].matchIndex;

        for (int i = 1; i < _cardsInHold.Count; i++)
        {
            if (_cardsInHold[i].matchIndex != id)
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            AudioManager.instance.Play2DClip("CardMatched");
            matchedCount++;
            scoreCounter += scoreMultiplier * comboCount;
            if (comboCount > 1){ GetCombo?.Invoke(comboCount);}
            comboCount++;

            foreach (var c in _cardsInHold)
                c.MatchedCard();

            if (matchedCount == cards.Count / matchCount)
            {
                yield return new WaitForSeconds(0.5f);
                LevelCleared?.Invoke();
            }
        }
        else
        {
            AudioManager.instance.Play2DClip("WrongCard");
            comboCount = 1;

            foreach (var c in _cardsInHold)
                c.FlipCard(false);
        }

        // remove from processing
        foreach (var c in _cardsInHold)
            processingCards.Remove(c);

        turnCount++;
        UpdateUI?.Invoke(matchedCount, turnCount, scoreCounter);
    }

    public void ResetGame()
    {
        foreach (GameObject card in cards)
            Destroy(card);

        cards.Clear();
        selectedCard.Clear();

        matchedCount = 0;
        turnCount = 0;
        scoreCounter = 0;
        comboCount = 0;
    }
}
