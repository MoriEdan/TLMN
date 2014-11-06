using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public SceneData sceneData;

    public HumanPlayer humanPlayerPrefab;
    public AIPlayer aiPlayerPrefab;

    public List<Player> players;
    private int numberPlayer;

    private GameState gameState;
    private List<Card> allCards;

    public Card cardPrefab;

    private Player currentPlayer;
    private int currentPlayerIndex;
    private Hand currentHand;
    private bool isFirstRound;

    private int currentSortLayer;
    private int currentPlaceToPutCard;

    //Player who has currentHand in one round
    private Player roundPlayer;

    private int winPlayerNumber;

    private List<Card> cardOnTable;

    // Test for rearrange card, not the best method
    public static int reArrangeCardValue;
    public static Vector3 reArrangeCardPosition;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.Idle;
        numberPlayer = 1;
        players = new List<Player>();
        CreatePlayers(numberPlayer);
        currentPlayerIndex = 0;
        currentPlayer = players[currentPlayerIndex];
        
        allCards = new List<Card>();

        // Create cards and shuffle them
        CreateAllCards();
        this.allCards = Shuffle(this.allCards);

        // set is first round or not, for player who has 3-spade will go first
        isFirstRound = true;

        // Set first player, hand
        currentHand = new Hand(new List<Card>());

        // Set Info on table to put card
        currentSortLayer = 1;
        currentPlaceToPutCard = 0;

        winPlayerNumber = 0;
        cardOnTable = new List<Card>();

        // For drag cards
        reArrangeCardValue = -1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.Idle:
                {
                    
                    break;
                }
            case GameState.Deal:
                {
                    break;
                }
            case GameState.Play:
                {
                    if (reArrangeCardValue >= 0)
                    {
                        ReArrange(reArrangeCardValue);
                        reArrangeCardValue = -1;
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    // Create players
    public void CreatePlayers(int numberPlayer)
    {
        for(int i = 0; i < numberPlayer; i++)
        {
            HumanPlayer humanPlayerInstance = Instantiate(humanPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as HumanPlayer;
            //humanPlayerInstance.GameManager = this;
            humanPlayerInstance.Name = i;
            players.Add(humanPlayerInstance);
        }
        for(int i = 0 ; i < Constants.MAX_NUMBER_PLAYER - numberPlayer; i++)
        {
            AIPlayer aiPlayerInstance = Instantiate(aiPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity) as AIPlayer;
            //aiPlayerInstance.GameManager = this;
            aiPlayerInstance.Name = (i + 2);
            players.Add(aiPlayerInstance);
        }
    }


    // Create 52 cards
    public void CreateAllCards()
    {
        for (int i = 0; i < Constants.SUITS.Length; i++)
        {
            for (int j = 0; j < Constants.VALUES.Length; j++)
            {
                Card cardInstance = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Card;
                cardInstance.GetComponent<tk2dSprite>().SetSprite("card_" + (j + 1) + "_" + i);
                //cardInstance.GetComponent<tk2dSprite>().SetSprite("Card_Back");
                cardInstance.GetComponent<Card>().Suit = Constants.SUITS[i];
                cardInstance.GetComponent<Card>().Value = Constants.VALUES[j];
                cardInstance.NumberValue = Constants.NUMBER_VALUES[j];
                cardInstance.NumberSuit = Constants.NUMBER_SUITS[i];
                cardInstance.CardValue = Constants.NUMBER_VALUES[j] * 10 + Constants.NUMBER_SUITS[i];
                allCards.Add(cardInstance);
            }
        }
    }
    
    // Shuffle cards
    public List<Card> Shuffle(List<Card> cards)
    {
        //ArrayList tempCards = ArrayToList(cards);
        List<Card> shuffleCards = new List<Card>();
        int randomIndex = 0;
        while (cards.Count > 0)
        {
            randomIndex = Random.Range(0, cards.Count);
            shuffleCards.Add(cards[randomIndex]);
            cards.RemoveAt(randomIndex);
        }
        return shuffleCards;
    }

    // Delay for deal card
    IEnumerator WaitToDealCard()
    {
        yield return new WaitForSeconds(3f);
    }

    // Move cards to each player's position
    public IEnumerator DealCards(List<Card> cards)
    {
       
        for (int i = 0; i < Constants.CARD_AMOUNT; i++)
        {
            if(cards[i].Value == "3" && cards[i].Suit == "Spade" && isFirstRound)
            {
                switch(i%4)
                {
                    case 0:
                        {
                            currentPlayer = players[0];
                            break;
                        }
                    case 1:
                        {
                            currentPlayer = players[1];
                            break;
                        }
                    case 2:
                        {
                            currentPlayer = players[2];
                            break;
                        }
                    case 3:
                        {
                            currentPlayer = players[3];
                            break;
                        }
                    default:
                        break;
                }
                isFirstRound = false;
            }

            //if (i % 4 == 0)
            //{
            //    cards[i].Move(sceneData.PlayerCardPosition[temp1].x,
            //        sceneData.PlayerCardPosition[temp1].y, Constants.TIME_TO_DEAL_ONE_CARD);
            //    //cards[i].DealMove(i%13, this.sceneData);
            //    cards[i].renderer.sortingLayerName = "Card";
            //    cards[i].renderer.sortingOrder = temp1;
            //    players[0].Deck.Cards.Add(cards[i]);
            //    temp1++;
                
            //}
            //else if (i % 4 == 1)
            //{
            //    cards[i].Move(sceneData.Player2CardPosition[temp2].x,
            //        sceneData.Player2CardPosition[temp2].y, Constants.TIME_TO_DEAL_ONE_CARD);
            //    cards[i].renderer.sortingLayerName = "Card";
            //    cards[i].renderer.sortingOrder = 12 - (temp2);
            //    players[1].Deck.Cards.Add(cards[i]);
            //    temp2++;
                
            //}
            //else if (i % 4 == 2)
            //{
            //    cards[i].Move(sceneData.Player3CardPosition[temp3].x,
            //        sceneData.Player3CardPosition[temp3].y, Constants.TIME_TO_DEAL_ONE_CARD);
            //    cards[i].renderer.sortingLayerName = "Card";
            //    cards[i].renderer.sortingOrder = 12 - (temp3);
            //    players[2].Deck.Cards.Add(cards[i]);
            //    temp3++;
                
            //}
            //else if (i % 4 == 3)
            //{
            //    cards[i].Move(sceneData.Player4CardPosition[temp4].x,
            //        sceneData.Player4CardPosition[temp4].y, Constants.TIME_TO_DEAL_ONE_CARD);
            //    cards[i].renderer.sortingLayerName = "Card";
            //    cards[i].renderer.sortingOrder = temp4;
            //    players[3].Deck.Cards.Add(cards[i]);
            //    temp4++;
                
            //}
            yield return new WaitForSeconds(0.05f);
        }
        currentPlayerIndex = 0;
        currentPlayer = players[currentPlayerIndex];
        gameState = GameState.Play;

        // Test Sort
        Utility.BubbleSort(players[1].Deck.Cards);
        Utility.BubbleSort(players[2].Deck.Cards);
        Utility.BubbleSort(players[3].Deck.Cards);
    }

    // Sort card ( onhand)
    public void Sort(List<Card> cards)
    {
        Utility.BubbleSort(cards);
        for(int i = 0; i< cards.Count; i++)
        {
            //cards[i].Move(sceneData.Player1CardPosition[i].x, sceneData.Player1CardPosition[i].y, Constants.TIME_TO_SORT_ONE_CARD);
            cards[i].renderer.sortingOrder = i;
        }
    }

    // Sort without bubble sort :))
    public void SortWithoutBubble(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            //cards[i].Move(sceneData.Player1CardPosition[i].x, sceneData.Player1CardPosition[i].y, Constants.TIME_TO_SORT_ONE_CARD);
            cards[i].renderer.sortingOrder = i;
        }
    }

    // Drag card handle
    public void ReArrange(int cardValue)
    {
        int tempDragCardIndex = 0;
        Card tempCard = null;
        bool isMoveToLeft = false;
        for(int i = 0; i < players[0].Deck.Cards.Count; i++)
        {
            if (players[0].Deck.Cards[i].CardValue == cardValue)
            {
                tempDragCardIndex = i;
                tempCard = players[0].Deck.Cards[i];
                if(tempCard.transform.position.x >= reArrangeCardPosition.x)
                {
                    isMoveToLeft = false;
                }
                else
                {
                    isMoveToLeft = true;
                }
                break;
            }
        }
        for (int i = 0; i < players[0].Deck.Cards.Count; i++)
        {
            if (tempCard.transform.position.x < players[0].Deck.Cards[i].transform.position.x)
            {
                players[0].Deck.Cards.Insert(i, tempCard);
                if (isMoveToLeft)
                {
                    players[0].Deck.Cards.RemoveAt(tempDragCardIndex+1);
                }
                else
                {
                    players[0].Deck.Cards.RemoveAt(tempDragCardIndex);
                }
                
                break;
            }
        }
        SortWithoutBubble(players[0].Deck.Cards);
    }

    // Find card is onhand
    public List<Card> FindCardOnHand()
    {
        List<Card> tempCardList = new List<Card>();
        for (int i = 0; i < players[0].Deck.Cards.Count; i++)
        {
            if (players[0].Deck.Cards[i].State == CardState.OnHand)
            {
                tempCardList.Add(players[0].Deck.Cards[i]);
            }
        }
        return tempCardList;
    }    

    public void NewRound(Player player, GameManager gameManager)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].State != PlayerState.Win)
            {
                players[i].State = PlayerState.Idle;
            }
        }
        player.State = PlayerState.Play;
        gameManager.CurrentPlayer = player;
        gameManager.RoundPlayer = player;

        for(int i = 0; i <cardOnTable.Count; i++)
        {
            cardOnTable[i].GetComponent<tk2dSprite>().SetSprite("Card_Back");
        }
    }

    public void CheckWin(Player player)
    {
        if(player.Deck.Cards.Count == 0)
        {
            player.State = PlayerState.Win;
            winPlayerNumber++;
            if(winPlayerNumber == 3)
            {
                ResetGame();
            }
        }
    }

    public void ResetGame()
    {
        for(int i = 0; i <players.Count; i++)
        {
            players[i].deck.Cards.Clear();
            players[i].State = PlayerState.Idle;
        }
        cardOnTable.Clear();
        for (int i = 0; i < allCards.Count; i++ )
        {
            allCards[i].State = CardState.Destroy;
        }
        AllCards.Clear();
        CreateAllCards();
        this.allCards = Shuffle(this.allCards);
        currentHand = new Hand(new List<Card>());
        currentSortLayer = 1;
        currentPlaceToPutCard = 0;
        winPlayerNumber = 0;
        currentPlayer = players[0];

    }

    public void SetPositionToStart()
    {
        int temp = 0;
        for (int i = 0; i < Constants.SUITS.Length; i++)
        {
            for (int j = 0; j < Constants.VALUES.Length; j++)
            {
                allCards[temp].transform.position = new Vector3(0, 0, 0);
                allCards[temp].GetComponent<tk2dSprite>().SetSprite("card_" + (j + 1) + "_" + i);
                //cardInstance.GetComponent<tk2dSprite>().SetSprite("Card_Back");
                allCards[temp].GetComponent<Card>().Suit = Constants.SUITS[i];
                allCards[temp].GetComponent<Card>().Value = Constants.VALUES[j];
                allCards[temp].NumberValue = Constants.NUMBER_VALUES[j];
                allCards[temp].NumberSuit = Constants.NUMBER_SUITS[i];
                allCards[temp].CardValue = Constants.NUMBER_VALUES[j] * 10 + Constants.NUMBER_SUITS[i];
                allCards.Add(allCards[temp]);
            }
        }
    }

    // Set state for all players
    public void SetPass(Player player)
    {
        player.State = PlayerState.Pass;
    }
    public void SetIdle(Player player)
    {
        player.State = PlayerState.Idle;
    }

    public void SetPlay()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        currentPlayer = players[currentPlayerIndex];
        if(currentPlayer.State == PlayerState.Win && roundPlayer == currentPlayer)
        {
            NewRound(NextPlayer(), this);
            return;
        }
        if(currentPlayer.State == PlayerState.Pass || currentPlayer.State == PlayerState.Win)
        {
            SetPlay();
        }
        currentPlayer.State = PlayerState.Play;
    }

    public Player NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        currentPlayer = players[currentPlayerIndex];
        if(currentPlayer.State == PlayerState.Win)
        {
            NextPlayer();
        }
        return currentPlayer;
    }

    // Get & Set
    public GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }
    public List<Card> AllCards
    {
        get { return allCards; }
        set { allCards = value; }
    }
    public Hand CurrentHand
    {
        get { return currentHand; }
        set { currentHand = value; }
    }
    public Player CurrentPlayer
    {
        get { return currentPlayer; }
        set { currentPlayer = value; }
    }
    public int CurrentSortLayer
    {
        get { return currentSortLayer; }
        set { currentSortLayer = value; }
    }
    public int CurrentPlaceToPutCard
    {
        get { return currentPlaceToPutCard; }
        set { currentPlaceToPutCard = value; }
    }
    public Player RoundPlayer
    {
        get { return roundPlayer; }
        set { roundPlayer = value; }
    }
    public List<Card> CardOnTable
    {
        get { return cardOnTable; }
        set { cardOnTable = value; }
    }
}

