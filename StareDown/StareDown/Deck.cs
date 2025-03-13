using System;
using System.Collections.Generic;

/*
    ========================== Deck: Manages the Game Deck ==========================
    PURPOSE:
    This class is responsible for creating, shuffling, and managing the deck of cards.
    The deck contains 52 standard cards + 2 Jokers (54 total).
    Implements random shuffling and drawing mechanics.

    MAIN RESPONSIBILITIES:
    1. Deck Initialization:
       Generates a full deck of cards (4 suits, ranks from 3 to A, plus 2).
       Assigns each card a base value using a dictionary.
       Includes 2 Jokers with a fixed value of 25 points.
    2. Shuffling the Deck:
       Uses the Fisher-Yates shuffle algorithm to randomize the card order.
    3. Drawing Cards:
       Provides a method to draw a card from the top of the deck.
       Ensures the game can check if the deck is empty.
    ========================== Deck: Manages the Game Deck ==========================

*/
class Deck
{
    private Stack<Card> cards;// Stack structure to efficiently manage drawing

    // Constructor that initializes the deck
    public Deck()
    {
        cards = new Stack<Card>();
        InitializeDeck();
        Shuffle();
    }

    // Creates the full deck of 54 cards
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
                cards.Push(new Card(suit, rank, values[rank]));// Adds each card to the deck
            }
        }

        // Add two Jokers
        cards.Push(new Card("Joker", "Joker", 25));
        cards.Push(new Card("Joker", "Joker", 25));
    }

    // Randomizes the order of cards in the deck
    public void Shuffle()
    {
        Random rng = new Random();
        List<Card> tempList = new List<Card>(cards);
        int n = tempList.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            (tempList[n], tempList[k]) = (tempList[k], tempList[n]);// Swap elements to shuffle

        }

        cards = new Stack<Card>(tempList);// Convert shuffled list back to a stack
    }

    // Removes and returns the top card from the deck
    public Card DrawCard()
    {
        return cards.Count > 0 ? cards.Pop() : null;
    }

    // Checks if there are cards remaining in the deck
    public bool HasCards()
    {
        return cards.Count > 0;
    }
}
