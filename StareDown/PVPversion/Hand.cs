using System;
using System.Collections.Generic;
using System.Linq;

class Hand
{
    private List<Card> cards;

    public Hand()
    {
        cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        if (card != null) cards.Add(card);
    }

    public void ShowHand()
    {
        Console.WriteLine("\nYour hand:");
        for (int i = 0; i < cards.Count; i++)
        {
            Console.WriteLine($"{i}: {cards[i]} (Value: {cards[i].GetPointValue()} points)");
        }
    }

    public bool AreValidIndexes(List<int> indexes)
    {
        return indexes.All(index => index >= 0 && index < cards.Count);
    }

    public List<Card> GetCardsByIndexes(List<int> indexes)
    {
        return indexes.Select(index => cards[index]).ToList();
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    // ✅ Fix: Replace card.Value with card.GetPointValue()
    public int CalculateHandValue()
    {
        return cards.Sum(card => card.GetPointValue());
    }

    public bool IsEmpty()
    {
        return cards.Count == 0;
    }
}
