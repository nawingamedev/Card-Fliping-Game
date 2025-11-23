using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class CardBehaviour : MonoBehaviour,IPointerClickHandler
{
    public int matchIndex;
    public CardStates State = CardStates.FaceDown;
    [SerializeField] private GameObject Backface;
    [SerializeField] private Image frontFace;
    [SerializeField] private float flipTime = 0.3f;

    private Coroutine coroutine;
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
        coroutine = StartCoroutine(FlipCardCoroutine(status));
    }
    public void MatchedCard()
    {
        frontFace.color = new Color(0,0,0,0);
        State = CardStates.Matched;
    }
    IEnumerator FlipCardCoroutine(bool status)
    {
        State = CardStates.flipping;

        float t = 0f;
        float half = flipTime / 2f;

        while (t < half)
        {
            t += Time.deltaTime;
            float p = t / half;
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, p), 1f, 1f);
            yield return null;
        }

        Backface.SetActive(!status);

        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float p = t / half;
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, p), 1f, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;

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
