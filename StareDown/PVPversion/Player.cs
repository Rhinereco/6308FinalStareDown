using System;
using System.Collections.Generic;
using System.Linq;

class Player
{
    public string Name { get; private set; }
    public int Score { get; private set; } = 0;
    private Hand hand;
    public bool SkipNextTurn { get; set; } = false;

    public Player(string name)
    {
        Name = name;
        hand = new Hand();
    }

    public void DrawInitialCards(Deck deck)
    {
        for (int i = 0; i < 5; i++)
            hand.AddCard(deck.DrawCard());
    }

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

        // ✅ Fix: Check the number of selected cards and use the correct rule function
        bool validMove = freePlay ||
                         (selectedCards.Count == 1 && Rules.IsValidMove(selectedCards[0], playPile)) || // Single card move
                         (selectedCards.Count == 2 && Rules.IsValidPair(selectedCards, playPile)) ||   // Pair move
                         (selectedCards.Count == 4 && Rules.IsValidBomb(selectedCards, playPile));     // Bomb move

        if (validMove)
        {
            PlayCards(selectedCards, playPile);
            ApplyCardEffects(selectedCards);
            return false;
        }
        else
        {
            Console.WriteLine("⚠️ Invalid move! Try again.\n");
            return false;
        }
    }

    private void PlayCards(List<Card> selectedCards, List<Card> playPile)
    {
        foreach (Card card in selectedCards)
        {
            playPile.Add(card);
            hand.RemoveCard(card);
            Score += card.GetPointValue();
            Console.WriteLine($"{Name} played {card}");
        }
    }

    private void ApplyCardEffects(List<Card> playedCards)
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

    public bool HasEmptyHand() => hand.IsEmpty();
}
