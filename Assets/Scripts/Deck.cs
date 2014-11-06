using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

    private ArrayList cards;
    public Card cardPrefab;
    private ArrayList player1Cards;
    private ArrayList player2Cards;
    private ArrayList player3Cards;
    private ArrayList player4Cards;

	// Use this for initialization
	void Start () {
        //cards = new Card[Constants.CARD_AMOUNT];
        cards = new ArrayList(Constants.CARD_AMOUNT);
        player1Cards = new ArrayList(Constants.CARD_AMOUNT_FOR_EACH_PLAYER);
        player1Cards = new ArrayList(Constants.CARD_AMOUNT_FOR_EACH_PLAYER);
        player1Cards = new ArrayList(Constants.CARD_AMOUNT_FOR_EACH_PLAYER);
        player1Cards = new ArrayList(Constants.CARD_AMOUNT_FOR_EACH_PLAYER);
        for(int i = 0; i < Constants.SUITS.Length; i++)
        {
            for(int j = 0; j < Constants.VALUES.Length; j++)
            {
                Card cardInstance = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as Card;
                cardInstance.GetComponent<tk2dSprite>().SetSprite("card_" + (j + 1) + "_" + i);
                //cardInstance.GetComponent<tk2dSprite>().SetSprite("Card_Back");
                cardInstance.GetComponent<Card>().Suit = Constants.SUITS[i];
                cardInstance.GetComponent<Card>().Value = Constants.VALUES[j];
                cards.Add(cardInstance);
                //cardInstance.gameObject.SetActive(false);
            }
        }

        // Shuffle Cards
        cards = Shuffle(cards);
        // Deal for 4 player Array
        Deal(cards);
	}
	
	// Update is called once per frame
	void Update () {

	}

    // Shuffle Card
    public ArrayList Shuffle(ArrayList cards)
    {
        ArrayList tempCards = new ArrayList(cards.Count);
        int randomIndex = 0;
        while(cards.Count > 0)
        {
            randomIndex = Random.Range(0, cards.Count);
            tempCards.Add(cards[randomIndex]);
            cards.RemoveAt(randomIndex);
        }
        return tempCards;
    }

    // Deal Cards
    public void Deal(ArrayList cards)
    {
        for (int i = 0; i < Constants.CARD_AMOUNT; i++)
        {
            if (i % 4 == 0)
            {
                player1Cards.Add(cards[i]);
            }
            else if (i % 4 == 1)
            {
                player2Cards.Add(cards[i]);
            }
            else if (i % 4 == 2)
            {
                player2Cards.Add(cards[i]);
            }
            else if (i % 4 == 3)
            {
                player2Cards.Add(cards[i]);
            }
        }
    }

    // Get & Set
    public ArrayList Cards
    {
        get { return cards; }
        set { cards = value; }
    }
    public ArrayList Player1Cards
    {
        get { return player1Cards; }
        set { player1Cards = value; }
    }
    public ArrayList Player2Cards
    {
        get { return player2Cards; }
        set { player2Cards = value; }
    }
    public ArrayList Player3Cards
    {
        get { return player3Cards; }
        set { player3Cards = value; }
    }
    public ArrayList Player4Cards
    {
        get { return player4Cards; }
        set { player4Cards = value; }
    }
}

