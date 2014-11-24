using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public abstract class Player : MonoBehaviour
{

    public Deck2 deckPrefab;
    public Deck2 deck;
    //private GameManager gameManager;
    private SFSGameRoom gameManager;


    private int name;
    private PlayerState state;

    private bool isActive;

    private bool isHasSuperCut;

    private bool isNewRoundPlayer;

    private string userName;

    

    // Use this for initialization
    void Awake()
    {
        isActive = false;
        state = PlayerState.Idle;
        isHasSuperCut = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual Hand GetMove()
    {
        return null;
    }

    // Put card onhand ==> table
    public void PutCard(Hand hand)
    {
        GameManager.CurrentPlaceToPutCard++;
        int placeToPut = GameManager.CurrentPlaceToPutCard % Constants.NUMBER_POSITION_TO_PUT_CARD;

        for (int i = 0; i < gameManager.PreviousCards.Count; i++)
        {
            if (gameManager.PreviousCards[i] != null)
            {
                gameManager.PreviousCards[i].gameObject.SetActive(false);
            }
        }
        gameManager.PreviousCards.Clear();
        int tempCount1 = gameManager.CurrentCards.Count;
        for (int i = 0; i < tempCount1; i++)
        {
            if (gameManager.CurrentCards[i] != null)
            {
                gameManager.CurrentCards[i].Move(gameManager.sceneData.PreviousCardPosition.x + (i - tempCount1/2) * 0.35f,
                    gameManager.sceneData.PreviousCardPosition.y, 0.1f);
                gameManager.CurrentCards[i].transform.localScale = new Vector3(0.35f, 0.35f, 0);
                gameManager.PreviousCards.Add(gameManager.CurrentCards[i]);
            }
        }
        gameManager.CurrentCards.Clear();
        int tempCount2 = hand.CardCount();
        for (int i = 0; i < hand.CardCount(); i++)
        {
            if (hand.Cards[i] != null)
            {
                hand.Cards[i].gameObject.SetActive(true);
                //hand.Cards[i].Move(gameManager.sceneData.PutCardPosition[placeToPut].x + i * 0.5f,
                //    gameManager.sceneData.PutCardPosition[placeToPut].y, 0.1f);
                hand.Cards[i].Move(gameManager.sceneData.CurrentCardPosition.x + (i - tempCount2 / 2) * 0.5f,
                    gameManager.sceneData.CurrentCardPosition.y, 0.1f);
                hand.Cards[i].transform.localScale = new Vector3(0.5f, 0.5f, 0);
                hand.Cards[i].renderer.sortingOrder = gameManager.CurrentSortLayer++;
                gameManager.CurrentHand = hand.CloneHand();
                this.Deck.Cards.Remove(hand.Cards[i]);
                gameManager.CurrentCards.Add(hand.Cards[i]);
                gameManager.CardOnTable.Add(hand.Cards[i]);
            }
        }
    }
    public void SpreadCard()
    {

    }

    public bool IsHandContainsOther(List<Hand> listHand, Hand hand)
    {
        bool isContains = false;
        for (int i = 0; i < listHand.Count; i++)
        {
            if (IsHandEqualOther(listHand[i], hand))
            {
                isContains = true;
                hand.Type = listHand[i].Type;
                break;
            }
        }
        return isContains;
    }

    public bool IsHandEqualOther(Hand firstHand, Hand secondHand)
    {
        bool isEqual = true;
        if (firstHand.CardCount() == secondHand.CardCount())
        {
            for (int i = 0; i < firstHand.CardCount(); i++)
            {
                if (firstHand.Cards[i].NumberValue != secondHand.Cards[i].NumberValue)
                {
                    isEqual = false;
                }
            }
        }
        else
        {
            isEqual = false;
        }
        return isEqual;
    }

    public List<Hand> LegalMoves()
    {
        List<Hand> hands = new List<Hand>();
        if (deck.Cards.Count > 0)
        {
            hands = deck.GetAllPossibleHands(deck.Cards);
            for (int i = 0; i < hands.Count; i++)
            {
                if (gameManager.CurrentHand.Type == "Cut"
                    || gameManager.CurrentHand.Type == "SuperCut"
                    || gameManager.CurrentHand.Type == "FourOfAKind"
                    || (gameManager.CurrentHand.Type == "Single" && gameManager.CurrentHand.Cards[0].Value.Equals("2")))
                {
                    if (!CanPlayForCut(hands[i], gameManager.CurrentHand))
                    {
                        hands.Remove(hands[i]);
                        i--;
                    }
                }
                else if (gameManager.CurrentHand.Type == "Pair" && gameManager.CurrentHand.Cards[0].Value.Equals("2"))
                {
                    if (!CanPlayForCut2(hands[i], gameManager.CurrentHand))
                    {
                        hands.Remove(hands[i]);
                        i--;
                    }
                }
                else
                {
                    if (!CanPlay(hands[i], gameManager.CurrentHand))
                    {
                        hands.Remove(hands[i]);
                        i--;
                    }
                }
            }
        }
        return hands;
    }

    public bool IsCurrentPlayer()
    {
        return (this.Name.Equals(gameManager.CurrentPlayer.Name));
    }

    public bool CanPlay(Hand hand, Hand currentHand)
    {
        return ((currentHand.IsEmpty() && !hand.IsEmpty()) ||
            (currentHand.Type.Equals(hand.Type) && hand.EvaluateHand() > currentHand.EvaluateHand()));
    }

    public bool CanPlayForCut(Hand hand, Hand currentHand)
    {
        bool canPlay = false;
        if (currentHand.IsEmpty() && !hand.IsEmpty())
        {
            canPlay = true;
        }
        else if (hand.Type == "Cut" || hand.Type == "SuperCut" || hand.Type == "FourOfAKind" || (hand.Type == "Single" && hand.Cards[0].Value.Equals("2")))
            {
                canPlay = hand.EvaluateHand() > currentHand.EvaluateHand();
            }
            else
            {
                canPlay = false;
            }
       return canPlay;
    }

    public bool CanPlayForCut2(Hand hand, Hand currentHand)
    {
        bool canPlay = false;
        if (currentHand.IsEmpty() && !hand.IsEmpty())
        {
            canPlay = true;
        }
        else if (hand.Type == "SuperCut" || hand.Type == "FourOfAKind" || (hand.Type == "Pair" && hand.Cards[0].Value.Equals("2")))
        {
            canPlay = hand.EvaluateHand() > currentHand.EvaluateHand();
        }
        else
        {
            canPlay = false;
        }
        return canPlay;
    }

    public bool CheckSuperWinHelper(List<Hand> hands)
    {
        bool isSuper = false;
        return isSuper;
        List<Hand>[] handByValues = new List<Hand>[16];
        int pairNumber = 0;
        for (int i = 0; i < 16; i++ )
        {
            handByValues[i] = new List<Hand>();
        }

        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].Type.Equals("Pair"))
            {
                handByValues[hands[i].Cards[0].NumberValue].Add(hands[i]);
            }
        }
   
        for(int i = 0; i < handByValues.Length; i++)
        {
            if((handByValues[i].Count == 1) || (handByValues[i].Count == 3))
            {
                pairNumber++;
            }
            if(handByValues[i].Count == 6)
            {
                pairNumber += 2;
            }
        }

        if(pairNumber == 6)
        {
            return true;
        }
        return false;
    }

    public bool CheckSuperWin()
    {
        bool isSuper = false;
        List<Hand> hands = LegalMoves();
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].Type.Equals("Straight12"))
            {
                isSuper = true;
            }
            if (hands[i].Type.Equals("FourOfAKind") && (hands[i].Cards[0].Value.Equals("2"))) // ((hands[i].Cards[0].Value.Equals("3"))
            {
                isSuper = true;
            }
        }
        if(CheckSuperWinHelper(hands))
        {
            isSuper = true;
        }
        return isSuper;
    }

    public bool CheckSuperCut()
    {
        List<Hand> hands = LegalMoves();
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].Type.Equals("SuperCut"))
            {
                return true;
            }
        }
        return false;
    }

    public Hand QuickFindHand()
    {
        if (GameManager.CurrentPlayer.Name.Equals(this.Name))
        {
            List<Hand> hands = LegalMoves();
            Hand selectedHand = null;
            if(hands.Count > 0)
            {
                selectedHand = hands[0];
            }
            if(selectedHand != null)
            {
                return selectedHand;
            }
            
        }
        return null;
    }


    // Get & Set
    public Deck2 Deck
    {
        get { return deck; }
        set { deck = value; }
    }
    public SFSGameRoom GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }

    public int Name
    {
        get { return name; }
        set { name = value; }
    }
    public PlayerState State
    {
        get { return state; }
        set { state = value; }
    }
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public bool IsHasSuperCut
    {
        get { return isHasSuperCut; }
        set { isHasSuperCut = value; }
    }

    public bool IsNewRoundPlayer
    {
        get { return isNewRoundPlayer; }
        set { isNewRoundPlayer = value; }
    }
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }
}
