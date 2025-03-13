using System;
using System.Collections.Generic;

class Deck
{
    private Stack<Card> cards;

    public Deck()
    {
        cards = new Stack<Card>();
        InitializeDeck();
        Shuffle();
    }

    private void InitializeDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A", "2" };
        Dictionary<string, int> values = new Dictionary<string, int>
        {
            { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 }, { "10", 10 },
            { "J", 10 }, { "Q", 10 }, { "K", 10 }, { "A", 15 }, { "2", 20 }
        };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                cards.Push(new Card(suit, rank, values[rank]));
            }
        }

        // Add two Jokers
        cards.Push(new Card("Joker", "Joker", 25));
        cards.Push(new Card("Joker", "Joker", 25));
    }

    public void Shuffle()
    {
        Random rng = new Random();
        List<Card> tempList = new List<Card>(cards);
        int n = tempList.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (tempList[n], tempList[k]) = (tempList[k], tempList[n]);
        }

        cards = new Stack<Card>(tempList);
    }

    public Card DrawCard()
    {
        return cards.Count > 0 ? cards.Pop() : null;
    }

    public bool HasCards()
    {
        return cards.Count > 0;
    }
}
