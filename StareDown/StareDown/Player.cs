using System;
using System.Collections.Generic;
using System.Linq;

/*
    ========================== Player: Manages Player Actions & Scoring ==========================
    PURPOSE:
    This class represents a player (human or AI) in the game.
    It manages the player's hand, turn-based actions, scoring, and special card effects.

    MAIN RESPONSIBILITIES:
    1. Card Management:
       Handles drawing cards at the start of the game and during turns.
       Manages played cards, removing them from the hand.
    2. Turn Execution:
       Allows the player to choose and play a card (single, pair, or bomb).
       Ensures valid moves based on the game rules.
       Allows drawing a card if no valid move is available.
    3. Scoring System:
       Updates the player's score based on the played card's value.
       Applies suit-based scoring effects (Spades x2, Hearts +2, Diamonds -1).
       Handles special scoring effects from face cards and Jokers.
    4. AI Compatibility:
       `PlayCards()` and `ApplyCardEffects()` are marked protected so AI can use them.
       AI logic (in `AIPlayer.cs`) can override `PlayTurn()` for automated decision-making

    ========================== Player: Manages Player Actions & Scoring ==========================

*/

class Player
{
    public string Name { get; private set; }
    public int Score { get; private set; } = 0;// Player's current score
    protected Hand hand; // Allows AI to access player's hand
    public bool SkipNextTurn { get; set; } = false;// Tracks whether the player skips a turn

    // Constructor - Initializes the player with an empty hand
    public Player(string name)
    {
        Name = name;
        hand = new Hand();
    }

    // Draws 5 starting cards for the player
    public void DrawInitialCards(Deck deck)
    {
        for (int i = 0; i < 5; i++)
            hand.AddCard(deck.DrawCard());
    }
    
    // Handles a player's turn, including playing or drawing cards
    public bool PlayTurn(List<Card> playPile, Deck deck, bool freePlay)
    {
        hand.ShowHand();
        Console.WriteLine($"{Name}, choose your cards to play (enter indexes separated by spaces, or type 'draw' to pick a card):");

        string input = Console.ReadLine();
        if (input.ToLower() == "draw")
        {
            hand.AddCard(deck.DrawCard());
            Console.WriteLine($"{Name} drew a card.\n");
            return true;
        }

        List<int> indexes = input.Split(' ').Select(int.Parse).Distinct().ToList();
        if (!hand.AreValidIndexes(indexes))
        {
            Console.WriteLine("Invalid input. Try again!");
            return false;
        }

        List<Card> selectedCards = hand.GetCardsByIndexes(indexes);

        bool validMove = freePlay ||
                         (selectedCards.Count == 1 && Rules.IsValidMove(selectedCards[0], playPile)) ||
                         (selectedCards.Count == 2 && Rules.IsValidPair(selectedCards, playPile)) ||
                         (selectedCards.Count == 4 && Rules.IsValidBomb(selectedCards, playPile));

        if (validMove)
        {
            // Make sure AI player can call these 2 methods
            PlayCards(selectedCards, playPile);  
            ApplyCardEffects(selectedCards);  
            return false;
        }
        else
        {
            Console.WriteLine("Invalid move! Try again.\n");
            return false;
        }
    }

    // Changed from private to protected to allow AI to use this method
    /*
        private: Only code declared in the same class or struct can access this member. 
        protected: Only code in the same class or in a derived class can access this 
        type or member.
    */
    // Handles the logic for playing a card (or set of cards)
    protected void PlayCards(List<Card> selectedCards, List<Card> playPile)
    {
        foreach (Card card in selectedCards)
        {
            playPile.Add(card);
            hand.RemoveCard(card);
            Score += card.GetPointValue();
            Console.WriteLine($"{Name} played {card}");
        }
    }

    // Changed from private to protected to allow AI to use this method
    // Applies special effects for face cards and Jokers
    protected void ApplyCardEffects(List<Card> playedCards)
    {
        foreach (var card in playedCards)
        {
            switch (card.Rank)
            {
                case "J":
                    Score += 5;
                    break;
                case "Q":
                    Score -= 5;
                    break;
                case "K":
                    Score += 10;
                    SkipNextTurn = true;
                    break;
                case "A":
                    Console.WriteLine("Opponent must draw a card before their turn!");
                    break;
                case "Joker":
                    Console.WriteLine("Joker removes the opponent’s highest-value card!");
                    break;
            }
        }
    }

    public bool HasEmptyHand() => hand.IsEmpty();// Checks if the player has played all their cards
}
