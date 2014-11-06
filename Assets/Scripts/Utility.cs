using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility {

    public static void BubbleSort(List<Card> cards)
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            for (int j = i + 1; j < cards.Count; j++)
            {
                if (cards[j].CardValue < cards[i].CardValue)
                {
                    Swap(i, j, cards);
                }
            }
        }
    }

    public static void BubbleSortByIndex(List<Card> cards)
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            for (int j = i + 1; j < cards.Count; j++)
            {
                if (cards[j].Index < cards[i].Index)
                {
                    Swap(i, j, cards);
                }
            }
        }
    }

    // Selectionsort Algorithm
    public void SelectionSort(List<Card> cards)
    {
        int min = 0;
        for (int i = 0; i < cards.Count - 1; i++)
        {
            min = i;
            for (int j = i + 1; j < cards.Count; j++)
            {
                if (cards[j].CardValue < cards[min].CardValue)
                {
                    min = j;
                    Swap(i, min, cards);
                }
            }
        }
    }

    // Swap 2 card
    public static void Swap(int i, int j, List<Card> cards)
    {
        Card temp = cards[i];
        cards[i] = cards[j];
        cards[j] = temp;
    }
}
