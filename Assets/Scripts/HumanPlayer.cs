using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class HumanPlayer : Player {

	// Use this for initialization
    public GameObject imgframeAvatar;
    public GameObject imgCards;
    public GameObject imgGlow;
    public GameObject imgPass;
    public GameObject txtTime;
    public GameObject imgGlowCircle;
    public GameObject txtMoney;

    private bool isAnimationActive;
    private PlayerState preState;
    

	void Awake () {
        deck = Instantiate(deckPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Deck2;
        imgPass.SetActive(false);
        txtTime.SetActive(false);
        imgGlowCircle.SetActive(false);
        isAnimationActive = false;
        preState = PlayerState.Pre;
	}
	
	// Update is called once per frame
	void Update () {
        if(!IsActive)
        {
            gameObject.SetActive(false);
        }
        if(preState!=State)
        {
            isAnimationActive = false;
            preState = State;
        }
        switch(State)
        {
            case PlayerState.Idle:
                {
                    if (!isAnimationActive)
                    {
                        SetTimeToStart();
                        isAnimationActive = true;
                    }
                    break;
                }
            case PlayerState.Play:
                {
                    if (!isAnimationActive)
                    {
                        Rotate();
                        isAnimationActive = true;
                    }
                    break;
                }
            case PlayerState.Pass:
                {
                    if(!isAnimationActive)
                    {
                        SetTimeToStart();
                        imgPass.SetActive(true);
                        isAnimationActive = true;
                    }
                    break;
                }
            case PlayerState.Win:
                {
                    if (!isAnimationActive)
                    {
                        SetTimeToStart();
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
	
	}

    // Find moves suitable
    public override Hand GetMove()
    {
        if(GameManager.CurrentPlayer.Name.Equals(this.Name))
        {
            List<Hand> hands = LegalMoves();
            Hand selectedHand = new Hand(GameManager.FindCardOnHand());
            if (IsHandContainsOther(hands, selectedHand))
            {
                return selectedHand;
            }
        }
        return null;
    }

    public void SetTimeToStart()
    {
        imgPass.SetActive(false);
        imgGlowCircle.SetActive(false);
        txtTime.SetActive(false);
        imgGlowCircle.GetComponent<AutoRotate>().IsRotate = false;
    }

    public void Rotate()
    {
        imgGlowCircle.GetComponent<AutoRotate>().time = Constants.COUNT_DOWN_TIME_PER_TURN;
        imgGlowCircle.transform.eulerAngles = new Vector3(0, 0, 0);
        imgPass.SetActive(false);
        imgGlowCircle.SetActive(true);
        txtTime.SetActive(true);
        imgGlowCircle.GetComponent<AutoRotate>().IsRotate = true;
    }
    public bool IsAnimationActive
    {
        get { return isAnimationActive; }
        set { isAnimationActive = value; }
    }
}
