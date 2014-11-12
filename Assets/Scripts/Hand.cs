using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand{

    private List<Card> cards;
    private string type;

    public Hand(Card card)
    {
        cards = new List<Card>();
        cards.Add(card);
        type = "INVALID";
    }
    public Hand(List<Card> paramCards)
    {
        cards = new List<Card>();
        for(int i = 0; i < paramCards.Count; i++)
        {
            cards.Add(paramCards[i]);
        }
        type = "INVALID";
    }

    public string getHandType()
    {
        type = "INVALID";
        if (IsSingle()) type = "Single";
        else if (IsPair()) type = "Pair";
        else if (IsThreeOfAKind()) type = "ThreeOfAKind";
        else if (IsFourOfAKind()) type = "FourOfAKind";
        else if (IsCut()) type = "Cut";
        else if (IsSuperCut()) type = "SuperCut";
        else if (IsStraight())
        {
            switch (this.CardCount())
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
        }
        return type;
    }

    public int EvaluateHand()
    {
        int val;
        if (this.CardCount() == 0)
        {
            val = 0;
        }
        else
        {
            if(this.type.Equals("Cut"))
            {
                val = 200 + GetHighestCard().CardValue;
            }
            else if(this.type.Equals("FourOfAKind"))
            {
                val = 500 + GetHighestCard().CardValue;
            }
            else if(this.type.Equals("SuperCut"))
            {
                val = 800 + GetHighestCard().CardValue;
            }
            else
            {
                val = GetHighestCard().CardValue;
            }
        }
        return val; // return the value of the highest card
    }


    public Card GetLowestCard()
    {
        Card lowest = this.cards[0];
        for (int i = 1; i < this.CardCount(); i++)
        {
            if (lowest.CardValue > this.cards[i].CardValue)
                lowest = this.cards[i];
        }
        return lowest;
    }
    public Card GetHighestCard()
    {
        Card highest = this.cards[0];
        for (int i = 1; i < this.CardCount(); i++)
        {
            if (highest.CardValue < this.cards[i].CardValue)
                highest = this.cards[i];
        }
        return highest;
    }

    public bool IsEmpty()
    {
        return CardCount() == 0;
    }

    public bool IsSingle()
    {
       if(this.CardCount() == 1)
       {
           return true;
       }
       return false;
    }

    public bool IsPair()
    {
        if (this.CardCount() == 2 && IsAllCardsEqualValue())
        {
            return true;
        }
        return false;
    }

    public bool IsThreeOfAKind()
    {
        if (this.CardCount() == 3 && IsAllCardsEqualValue())
        {
            return true;
        }
        return false;
    }

    public bool IsFourOfAKind()
    {
        if (this.CardCount() == 4 && IsAllCardsEqualValue())
        {
            return true;
        }
        return false;
    }

    public bool IsStraight()
    {
        if (this.CardCount() == 0)
        {
            return false;
        }
        bool isStraight = true;
        Utility.BubbleSort(this.Cards);
        int firstValue = this.Cards[0].NumberValue;
        for (int i = 1; i < this.CardCount(); i++)
        {
            firstValue++;
            isStraight = isStraight && (this.Cards[i].NumberValue == firstValue);
        }
        return isStraight;
    }

    public bool IsCut()
    {
        if (this.CardCount() != 6)
        {
            return false;
        }
        bool isCut = true;
        Utility.BubbleSort(this.Cards);
        int firstValue = this.Cards[0].NumberValue;
        for (int i = 1; i < this.CardCount(); i++)
        {
            if(i%2 == 0)
            {
                firstValue++;
            }
            isCut = isCut && (this.Cards[i].NumberValue == firstValue);
        }
        return isCut;
    }

    public bool IsSuperCut()
    {
        if (this.CardCount() != 8)
        {
            return false;
        }
        bool isSuperCut = true;
        Utility.BubbleSort(this.Cards);
        int firstValue = this.Cards[0].NumberValue;
        for (int i = 1; i < this.CardCount(); i++)
        {
            if (i % 2 == 0)
            {
                firstValue++;
            }
            isSuperCut = isSuperCut && (this.Cards[i].NumberValue == firstValue);
        }
        return isSuperCut;
    }

    public bool IsAllCardsEqualValue()
    {
        bool isEqual = true;
        string lastValue = "";
        if (this.CardCount() == 0)
        {
            return true;
        }
        else
        {
            lastValue = this.Cards[0].Value;
        }
        for (int i = 0; i < this.CardCount(); i++)
        {
            isEqual = isEqual && (this.Cards[i].Value == lastValue);
        }
        return isEqual;
    }    

    public Hand CloneHand()
    {
        Hand tempHand = new Hand(new List<Card>());
        for(int i = 0; i < cards.Count; i++)
        {
            tempHand.cards.Add(cards[i]);
        }
        tempHand.Type = this.Type;
        return tempHand;
    }

    public int CardCount()
    {
        return Cards.Count;
    }

    // Get & Set
    public List<Card> Cards
    {
        get { return cards; }
        set { cards = value; }
    }
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
}
