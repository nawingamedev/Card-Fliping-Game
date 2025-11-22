using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour,IPointerClickHandler
{
    public int matchIndex;
    public CardStates State = CardStates.FaceDown;
    [SerializeField] private GameObject Backface;
    [SerializeField] private Image frontFace;
    public delegate void ClickCard(CardBehaviour card);
    public static ClickCard CardClicked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(Sprite faceImg,int matchID)
    {
        frontFace.sprite = faceImg;
        matchIndex = matchID;
    }
    public void Initialize(Color faceImg,int matchID)
    {
        frontFace.color = faceImg;
        matchIndex = matchID;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardClicked?.Invoke(this);
    }
    public void FlipCard(bool status)
    {
        Backface.SetActive(!status);
        if(status){State = CardStates.FaceUp;}
        else{State = CardStates.FaceDown;}
    }
    public void MatchedCard()
    {
        frontFace.color = new Color(0,0,0,0);
        State = CardStates.Matched;
    }
}
public enum CardStates
{
    FaceDown,
    Flipping,
    FaceUp,
    Matched,
}
