using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Card : MonoBehaviour {

    private string suit;
    private string value;
    private int cardValue;
    private int numberValue;
    private int numberSuit;

    private int index;

    private bool isBack;
    private CardState state;
    private Vector3 originalPosition;
    private bool isFirstTimeClick;

    private Vector3 screenPoint;
    private Vector3 offset;

    private bool isDrag;
    private float dragTime;
    private bool isSavedPosition;

	// Use this for initialization
	void Awake () {

        isBack = false;
        suit = "Spade";
        value = "1";
        state = CardState.Idle;

        // For click onhand
        originalPosition = new Vector3(0, 0);
        isFirstTimeClick = false;

        // For drag card
        isDrag = false;
        dragTime = Constants.DELAY_TIME_TO_DRAG_CARD;
        isSavedPosition = false;
	}
	
	// Update is called once per frame
	void Update () {
        switch(state)
        {
            case CardState.Idle:
                {
                    break;
                }
            case CardState.OnHand:
                {
                    break;
                }
            case CardState.Putted:
                {
                    break;
                }
            case CardState.Fold:
                {
                    break;
                }
            case CardState.Destroy:
                {
                    Destroy(gameObject);
                    break;
                }
            default:
                {
                    break;
                }
        }
	
	}

    void OnMouseUp()
    {

        MouseUpHandler();
    }

    public void MouseUpHandler()
    {
        if (!isDrag)
        {
            if (state == CardState.Idle)
            {
                if (!isFirstTimeClick)
                {
                    originalPosition = gameObject.transform.position;
                    isFirstTimeClick = true;
                }
                state = CardState.OnHand;
                Move(gameObject.transform.position.x, gameObject.transform.position.y + Constants.CARD_MOVE_UP_DISTANCE, 0.03f);

            }
            else if (state == CardState.OnHand)
            {
                state = CardState.Idle;
                //Move(gameObject.transform.position.x, gameObject.transform.position.y - Constants.CARD_MOVE_UP_DISTANCE, 0.2f);
                Move(gameObject.transform.position.x, originalPosition.y, 0.03f);

            }
        }
        else
        {
            SFSGameRoom.reArrangeCardValue = this.cardValue;
            dragTime = Constants.DELAY_TIME_TO_DRAG_CARD;
            isSavedPosition = false;
            isDrag = false;
        }
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if(dragTime < 0f)
        {
            isDrag = true;
            if(!isSavedPosition)
            {
                GameManager.reArrangeCardPosition = this.transform.position;
                isSavedPosition = true;
            }
            gameObject.renderer.sortingOrder = 20;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, gameObject.transform.position.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition = new Vector3(curPosition.x, gameObject.transform.position.y, curPosition.z);
            transform.position = curPosition;
        }
        else
        {
            dragTime -= Time.deltaTime;
        }
    }

    public void Move(float x, float y, float time){
        iTween.MoveTo(gameObject, iTween.Hash("x", x, "y", y,"time", time));
    }
    public void Move(float x, float y)
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", x, "y", y));
    }

    // Get & Set
    public string Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
    public string Suit
    {
        get { return suit; }
        set { suit = value; }
    }
    public bool IsBack
    {
        get { return isBack; }
        set { isBack = value; }
    }
    public CardState State
    {
        get { return state; }
        set { state = value; }
    }
    public int CardValue
    {
        get { return cardValue; }
        set { cardValue = value; }
    }
    public int NumberValue
    {
        get { return numberValue; }
        set { numberValue = value; }
    }
    public int NumberSuit
    {
        get { return numberSuit; }
        set { numberSuit = value; }
    }
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
}
