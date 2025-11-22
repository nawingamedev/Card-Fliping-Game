using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System;

public class CardBehaviour : MonoBehaviour,IPointerClickHandler
{
    public int matchIndex;
    public CardStates State = CardStates.FaceDown;
    [SerializeField] private GameObject Backface;
    [SerializeField] private Image frontFace;
    [SerializeField] private float flipTime = 0.3f;
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
        if(status){State = CardStates.FaceUp;}
        else{State = CardStates.FaceDown;}
        StartCoroutine(FlipCardCoroutine(status));
    }
    public void MatchedCard()
    {
        frontFace.color = new Color(0,0,0,0);
        State = CardStates.Matched;
    }
    IEnumerator FlipCardCoroutine(bool status)
    {
        State = CardStates.flipping;
        float elapse = 0;
        float targetRotation = transform.eulerAngles.y + 90;
        float initialRotation = transform.eulerAngles.x;
        while(elapse < flipTime)
        {
            elapse += Time.deltaTime;
            float rot = Mathf.Lerp(initialRotation,targetRotation,elapse/flipTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,rot,transform.eulerAngles.z);
            yield return null;
        }
        Backface.SetActive(!status);
        targetRotation = transform.eulerAngles.y - 90;
        elapse = 0;
        while(elapse < flipTime)
        {
            elapse += Time.deltaTime;
            float rot = Mathf.Lerp(initialRotation,targetRotation,elapse/flipTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,rot,transform.eulerAngles.z);
            yield return null;
        }
        State = status ? CardStates.FaceUp : CardStates.FaceDown;
    }
}
public enum CardStates
{
    FaceDown,
    FaceUp,
    flipping,
    Matched,
}
