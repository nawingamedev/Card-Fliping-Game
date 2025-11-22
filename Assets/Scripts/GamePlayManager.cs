using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI matchCountText,turnCountText;
    private int matchedCount
    {
        get
        {
            return _matchedCount;
        }
        set
        {
            _matchedCount = value;
            matchCountText.text = matchCount.ToString(); 
        }
    }
    private int _matchedCount;
    private int turnCount
    {
        get
        {
            return _turnCount;
        }
        set
        {
            _turnCount = value;
            turnCountText.text = turnCount.ToString(); 
        }
    }
    private int _turnCount;
    private GridLayoutGroup gridLayoutGroup;
    private List<GameObject> cards = new();
    private List<CardBehaviour> selectedCard = new();
    void Awake()
    {
        gridLayoutGroup = gridPanel.GetComponent<GridLayoutGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        matchedCount = 0;
        turnCount = 0;
        GenerateCards(3,4);
    }

    void OnEnable()
    {
        CardBehaviour.CardClicked += CardSelected;
    }
    void OnDisable()
    {
        CardBehaviour.CardClicked -= CardSelected;
    }
    void GenerateCards(int _rows,int _colmns)
    {
        int count = _rows * _colmns;
        if(count % 2 != 0){count -= 1;}
        gridLayoutGroup.constraintCount = _rows;
        List<int> cardSet = new();
        for(int i = 0; i < count/2; i++)
        {
            cardSet.Add(i);
            cardSet.Add(i);
        }
        for(int i = 0; i < cardSet.Count; i++)
        {
            int random = Random.Range(i,cardSet.Count);
            int temp = cardSet[i];
            cardSet[i] = cardSet[random];
            cardSet[random] = temp;
        }
        for(int i = 0; i < count; i++)
        {
            GameObject _cardObj = Instantiate(cardPrefab,gridPanel.transform);
            CardBehaviour cb = _cardObj.GetComponent<CardBehaviour>();
            Debug.Log(faceColors[cardSet[i]]);
            cb.Initialize(faceColors[cardSet[i]],cardSet[i]);
            cards.Add(_cardObj);
            StartCoroutine(ShowHideCards());
        }
    }
    IEnumerator ShowHideCards()
    {
        foreach(var card in cards)
        {
            card.GetComponent<CardBehaviour>().FlipCard(true);
        }
        yield return new WaitForSeconds(2.0f);
        foreach(var card in cards)
        {
            card.GetComponent<CardBehaviour>().FlipCard(false);
        }
    }
    public void RestartGame()
    {
        foreach(GameObject card in cards){Destroy(card);}
        GenerateCards(3,4);
    }
    void CardSelected(CardBehaviour card)
    {
        if (card.State == CardStates.FaceUp || card.State == CardStates.Flipping || card.State == CardStates.Matched) return;

        card.FlipCard(true);
        selectedCard.Add(card);
        if (selectedCard.Count >= matchCount)
        {
            if (selectedCard[0].matchIndex == selectedCard[1].matchIndex)
            {
                matchedCount++;
                StartCoroutine(MatchedCards(selectedCard[0],selectedCard[1]));
            }
            else
            {
                StartCoroutine(NotMatchedCards(selectedCard[0],selectedCard[1]));
            }
            turnCount++;
            selectedCard.Clear();
        }
    }
    IEnumerator MatchedCards(CardBehaviour a, CardBehaviour b)
    {
        yield return new WaitForSeconds(1.0f);
        a.MatchedCard();
        b.MatchedCard();
    }
    IEnumerator NotMatchedCards(CardBehaviour a, CardBehaviour b)
    {
        yield return new WaitForSeconds(1.0f);
        a.FlipCard(false);
        b.FlipCard(false);
    }
}
