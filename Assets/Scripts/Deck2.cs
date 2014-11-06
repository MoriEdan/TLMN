using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck2 : MonoBehaviour {

    private List<Card> cards;
    private List<Hand> hands;
    
    // Use this for initialization
    void Awake()
    {
        //cards = new Card[Constants.CARD_AMOUNT];
        cards = new List<Card>();
        hands = new List<Hand>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Hand> GetAllPossibleHands(List<Card> cards)
    {
        List<Hand> hands = new List<Hand>();
        AddAll(hands, GetPossibleSingles(cards));
        AddAll(hands, GetPossiblePairs(cards));
        AddAll(hands, GetPossibleThrees(cards));
        AddAll(hands, GetPossibleFours(cards));
        AddAll(hands, GetPossibleStraights(cards));
        AddAll(hands, GetPossibleCuts(cards));
        return hands;
    }

    public List<Hand> GetPossibleSingles(List<Card> cards)
    {
        List<Hand> tempHands = new List<Hand>();
        for(int i =0; i< cards.Count; i++)
        {
            Hand tempHand = new Hand(cards[i]);
            tempHand.Type = "Single";
            tempHands.Add(tempHand);
            //tempHands.Add(new Hand(cards[i]));
        }
        return tempHands;
    }

    public List<Hand> GetPossiblePairs(List<Card> cards)
    {
        List<Hand> tempHands = new List<Hand>();
        List<Card>[] tempPairs = new List<Card>[16];
        for(int i = 0; i < 16; i++)
        {
            tempPairs[i] = new List<Card>();
        }
        for(int i = 0; i < cards.Count; i++)
        {
            tempPairs[cards[i].NumberValue].Add(cards[i]);
        }
        for(int i = 0; i< tempPairs.Length; i++)
        {
            if(tempPairs[i].Count >= 2)
            {
                for(int j = 0; j < tempPairs[i].Count; j++)
                {
                    for(int k = j + 1; k < tempPairs[i].Count; k++)
                    {
                        //List<Card> tempHand = new List<Card>();
                        Hand tempHand = new Hand(new List<Card>());
                        if(j!=k)
                        {
                            tempHand.Cards.Add(tempPairs[i][j]);
                            tempHand.Cards.Add(tempPairs[i][k]);
                            if(tempHand.CardCount() > 0)
                            {
                                tempHand.Type = "Pair";
                                tempHands.Add(tempHand);
                            }
                        }
                    }
                }
            }
        }
        return tempHands;
    }

    public List<Hand> GetPossibleThrees(List<Card> cards)
    {
        List<Hand> tempHands = new List<Hand>();
        List<Card>[] tempThrees = new List<Card>[16];
        for (int i = 0; i < 16; i++)
        {
            tempThrees[i] = new List<Card>();
        }
        for (int i = 0; i < cards.Count; i++)
        {
            tempThrees[cards[i].NumberValue].Add(cards[i]);
        }
        for (int i = 0; i < tempThrees.Length; i++)
        {
            if(tempThrees[i].Count >= 3)
            {
                for(int j = 0; j < tempThrees[i].Count; j++)
                {
                    for(int k = j + 1; k < tempThrees[i].Count; k++)
                    {
                        for(int l = k + 1; l <tempThrees[i].Count; l++)
                        {
                            //List<Card> tempHand = new List<Card>();
                            Hand tempHand = new Hand(new List<Card>());
                            tempHand.Cards.Add(tempThrees[i][j]);
                            tempHand.Cards.Add(tempThrees[i][k]);
                            tempHand.Cards.Add(tempThrees[i][l]);
                            tempHand.Type = "ThreeOfAKind";
                            tempHands.Add(tempHand);
                        }
                    }
                }
            }
        }
        return tempHands;
    }

    public List<Hand> GetPossibleFours(List<Card> cards)
    {
        List<Hand> tempHands = new List<Hand>();
        List<Card> tempCards = new List<Card>();
        Utility.BubbleSort(cards);
        int firstValue = cards[0].NumberValue;
        tempCards.Add(cards[0]);
        for (int i = 1; i < cards.Count; i++ )
        {
            if(cards[i].NumberValue == firstValue)
            {
                tempCards.Add(cards[i]);
                if (tempCards.Count == 4)
                {
                    Hand tempHand = new Hand(tempCards);
                    tempHand.Type = "FourOfAKind";
                    tempHands.Add(tempHand);
                }
            }
            else
            {
                tempCards.Clear();
                firstValue = cards[i].NumberValue;
                tempCards.Add(cards[i]);
            }
        }
        return tempHands;
    }

    public List<Hand> GetPossibleStraights(List<Card> cards)
    {
        bool isFirstHand = true;
        List<Hand> returnHands = new List<Hand>();
        List<Hand> hands = new List<Hand>();
        
        List<Card>[] tempStraights = new List<Card>[16];
        for (int i = 0; i < 16; i++)
        {
            tempStraights[i] = new List<Card>();
        }
        for (int i = 0; i < cards.Count; i++)
        {
            tempStraights[cards[i].NumberValue].Add(cards[i]);
        }
        // -1 cause 2 not in straights
        int last = -1;
        hands.Add(new Hand(new List<Card>()));
        for (int i = 0; i < tempStraights.Length - 1; i++ )
        {
            List<Hand> tempHands = new List<Hand>();
            if ((tempStraights[i].Count > 0) && ((i == last + 1) || (last == -1)))
            {
                for (int j = 1; j < tempStraights[i].Count; j++)
                {
                    for (int k = 0; k < hands.Count; k++)
                    {
                        tempHands.Add(hands[k].CloneHand());
                    }
                }
                hands = AddAll(hands, tempHands);

                for (int l = 0; l < hands.Count; l++)
                {
                    hands[l].Cards.Add(tempStraights[i][l % tempStraights[i].Count]);
                }

                if (!isFirstHand)
                {
                    for (int n = 0; n < tempStraights[i].Count; n++)
                    {
                        hands.Add(new Hand(tempStraights[i][n]));
                    }
                }

                isFirstHand = false;
                
                for (int m = 0; m < hands.Count; m++)
                {
                    if (hands[m].CardCount() >= 3)
                    {
                        Hand tempHand = hands[m].CloneHand();
                        tempHand.Type = GetStraightType(tempHand);
                        returnHands.Add(tempHand);
                    }
                }
            }
            else
            {
                hands.Clear();
                hands.Add(new Hand(new List<Card>()));
                isFirstHand = true;
            }
            last = i;
        }

        return returnHands;
    }

    public string GetStraightType(Hand hand)
    {
        string type = string.Empty;
        switch (hand.CardCount())
        {
            case 3: type = "Straight3"; break;
            case 4: type = "Straight4"; break;
            case 5: type = "Straight5"; break;
            case 6: type = "Straight6"; break;
            case 7: type = "Straight7"; break;
            case 8: type = "Straight8"; break;
            case 9: type = "Straight9"; break;
            case 10: type = "Straight10"; break;
            case 11: type = "Straight11"; break;
            case 12: type = "Straight12"; break;
            case 13: type = "Straight13"; break;
        }
        return type;
    }

    public List<Hand> GetPossibleCuts(List<Card> cards)
    {
        List<Hand> cuts = new List<Hand>();
        List<Card> tempCuts = new List<Card>();
        List<Hand> tempPairs = GetPossiblePairs(cards);
        int last = -1;
        for (int i = 0; i < tempPairs.Count; i++ )
        {
            if(tempPairs[i].Cards[0].NumberValue == last + 1 || last == -1)
            {
                for(int j = 0 ;j <tempPairs[i].CardCount(); j++)
                {
                    tempCuts.Add(tempPairs[i].Cards[j]);
                }
            }
            else
            {
                GetPossibleCutsHelper(cuts, tempCuts);
                tempCuts = new List<Card>();
            }
            last = tempPairs[i].Cards[0].NumberValue;
        }
        return cuts;
    }

    public void GetPossibleCutsHelper(List<Hand> cuts, List<Card> tempCuts)
    {
        Hand tempHand;
        for (int i = 0; i < tempCuts.Count; i += 2)
        {
            tempHand = new Hand(new List<Card>());
            for(int j = i ; j < tempCuts.Count; j +=2)
            {
                tempHand.Cards.Add(tempCuts[j]);
                tempHand.Cards.Add(tempCuts[j+1]);
                if(tempHand.CardCount() == 6)
                {
                    Hand h = tempHand.CloneHand();
                    h.Type = "Cut";
                    cuts.Add(h);
                }
                if(tempHand.CardCount() ==8)
                {
                    Hand h = tempHand.CloneHand();
                    h.Type = "SuperCut";
                    cuts.Add(h);
                }
            }
        }
            
    }
    public List<Hand> AddAll(List<Hand> list, List<Hand> listToAdd)
    {
        for (int i = 0; i < listToAdd.Count; i++ )
        {
            list.Add(listToAdd[i]);
        }
        return list;
    }

    public void DestroyAllCard()
    {
        for(int i = 0; i< cards.Count; i++)
        {
            Destroy(cards[i]);
        }
    }

    // Get & Set
    public List<Card> Cards
    {
        get { return cards; }
        set { cards = value; }
    }
    public List<Hand> Hands
    {
        get { return hands; }
        set { hands = value; }
    }
}
