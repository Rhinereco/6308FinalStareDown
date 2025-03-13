using System;
using System.Collections.Generic;
using System.Linq;

/*
    ========================== Hand: Manages Player's Cards ==========================
    PURPOSE:
    This class represents a player's hand, managing cards they hold.
    It provides functions to add/remove cards, check valid moves, and organize play.

    MAIN RESPONSIBILITIES:
    1. Card Management:
       Adding new cards when drawn from the deck.
       Removing cards when played.
       Showing the current hand.
    2. Checking for Valid Moves:
       Determines if a card can be played based on game rules.
       dentifies special combinations like pairs and bombs.
    3. Helping AI and Player Decisions:
       Finds the best playable card.
       Checks if the player has a valid pair or bomb.
    ========================== Hand: Manages Player's Cards ==========================
*/
class Hand
{
    private List<Card> cards;// Stores the cards in the player's hand

    // Constructor - Initializes an empty hand
    public Hand()
    {
        cards = new List<Card>();
    }

    // Adds a new card to the player's hand
    public void AddCard(Card card)
    {
        if (card != null) cards.Add(card);
    }

    // Displays the player's current hand in the console
    public void ShowHand()
    {
        Console.WriteLine("\nYour hand:");
        for (int i = 0; i < cards.Count; i++)
        {
            Console.WriteLine($"{i}: {cards[i]} (Value: {cards[i].GetPointValue()} points)");
        }
    }

    // Checks if the selected indexes exist in the hand
    public bool AreValidIndexes(List<int> indexes)
    {
        return indexes.All(index => index >= 0 && index < cards.Count);
    }

    // Retrieves specific cards based on user input
    public List<Card> GetCardsByIndexes(List<int> indexes)
    {
        return indexes.Select(index => cards[index]).ToList();
    }
    
    // Finds all playable cards based on game rules
    public List<Card> GetValidMoves(List<Card> playPile, bool freePlay)
    {
        return freePlay ? cards : cards.Where(c => Rules.IsValidMove(c, playPile)).ToList();
    }

    // Checks if the hand contains a valid pair
    public bool HasPair(Card card)
    {
        return cards.Count(c => c.Rank == card.Rank) >= 2;
    }

    // Retrieves a playable pair from the hand
    public List<Card> GetPair(Card card)
    {
        return cards.Where(c => c.Rank == card.Rank).Take(2).ToList();
    }

    // Checks if the hand contains a four-of-a-kind bomb
    public bool HasBomb(Card card)
    {
        return cards.Count(c => c.Rank == card.Rank) == 4;
    }

    // Retrieves all four matching cards if a bomb exists
    public List<Card> GetBomb(Card card)
    {
        return cards.Where(c => c.Rank == card.Rank).ToList();
    }

    // Removes a single card from the player's hand
    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    // Calculates the total score of all cards in hand
    public int CalculateHandValue()
    {
        return cards.Sum(card => card.GetPointValue());
    }

    // Checks if the player's hand is empty
    public bool IsEmpty()
    {
        return cards.Count == 0;
    }
}


