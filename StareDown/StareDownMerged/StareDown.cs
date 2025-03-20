using System;
using System.Collections.Generic;
using System.Linq;

/* ========================== Card Class: Represents a Playing Card ========================== */
class Card
{
    public string Suit { get; private set; }
    public string Rank { get; private set; }
    public int BaseValue { get; private set; }

    public Card(string suit, string rank, int baseValue)
    {
        Suit = suit;
        Rank = rank;
        BaseValue = baseValue;
    }

    public int GetPointValue()
    {
        int finalValue = BaseValue;
        switch (Suit)
        {
            case "Spades": finalValue *= 2; break;
            case "Hearts": finalValue += 2; break;
            case "Diamonds": finalValue -= 1; break;
        }
        return Math.Max(finalValue, 0);
    }

    public override string ToString() => $"{Rank} of {Suit}";
}

/* ========================== Deck Class: Manages the Card Deck ========================== */
class Deck
{
    private Stack<Card> cards = new Stack<Card>();

    public Deck()
    {
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

        foreach (var suit in suits)
            foreach (var rank in ranks)
                cards.Push(new Card(suit, rank, values[rank]));

        cards.Push(new Card("Joker", "Joker", 25));
        cards.Push(new Card("Joker", "Joker", 25));
    }

    public void Shuffle() => cards = new Stack<Card>(new List<Card>(cards).OrderBy(_ => Guid.NewGuid()));

    public Card DrawCard() => cards.Count > 0 ? cards.Pop() : null;

    public bool HasCards() => cards.Count > 0;
}

/* ========================== Hand Class: Manages Player's Cards ========================== */
class Hand
{
    private List<Card> cards = new List<Card>();

    public void AddCard(Card card) { if (card != null) cards.Add(card); }

    public void ShowHand()
    {
        Console.WriteLine("\nYour hand:");
        for (int i = 0; i < cards.Count; i++)
            Console.WriteLine($"{i}: {cards[i]} (Value: {cards[i].GetPointValue()} points)");
    }

    public List<Card> GetValidMoves(List<Card> playPile, bool freePlay) =>
        freePlay ? cards : cards.Where(c => Rules.IsValidMove(c, playPile)).ToList();

    public bool IsEmpty() => cards.Count == 0;
}

/* ========================== Rules Class: Defines Game Rules ========================== */
class Rules
{
    public static bool IsValidMove(Card playedCard, List<Card> playPile) =>
        playPile.Count == 0 || playedCard.GetPointValue() >= playPile.Last().GetPointValue();
}

/* ========================== Player & AIPlayer Classes ========================== */
class Player
{
    public string Name { get; private set; }
    public int Score { get; private set; } = 0;
    protected Hand hand = new Hand();
    public bool SkipNextTurn { get; set; } = false;

    public Player(string name) => Name = name;

    public void DrawInitialCards(Deck deck) { for (int i = 0; i < 5; i++) hand.AddCard(deck.DrawCard()); }

    public bool PlayTurn(List<Card> playPile, Deck deck, bool freePlay)
    {
        hand.ShowHand();
        Console.WriteLine($"{Name}, enter card index to play, or 'draw' to pick a card:");
        string input = Console.ReadLine();
        if (input.ToLower() == "draw")
        {
            hand.AddCard(deck.DrawCard());
            Console.WriteLine($"{Name} drew a card.\n");
            return true;
        }

        return false;
    }

    public bool HasEmptyHand() => hand.IsEmpty();
}

class AIPlayer : Player
{
    public AIPlayer(string name) : base(name) { }

    public bool PlayTurn(List<Card> playPile, Deck deck, bool freePlay)
    {
        Console.WriteLine($"\n{this.Name} is thinking...");
        if (hand.GetValidMoves(playPile, freePlay).Count == 0)
        {
            hand.AddCard(deck.DrawCard());
            Console.WriteLine($"{this.Name} had no valid move and drew a card.");
            return true;
        }
        return false;
    }
}

/* ========================== Game Class: Manages Main Game Loop ========================== */
class Game
{
    private Deck deck;
    private Player player;
    private AIPlayer aiPlayer;
    private List<Card> playPile = new List<Card>();
    private bool freePlay = false;

    public void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Stare Down! (PVE Mode)");
        deck = new Deck();
        player = new Player("You");
        aiPlayer = new AIPlayer("AI Bot");
        player.DrawInitialCards(deck);
        aiPlayer.DrawInitialCards(deck);
        PlayGame();
    }

    private void PlayGame()
    {
        Player currentPlayer = player;
        Player opponent = aiPlayer;

        while (deck.HasCards() || !currentPlayer.HasEmptyHand())
        {
            if (currentPlayer.PlayTurn(playPile, deck, freePlay))
                freePlay = true;
            else
                freePlay = false;

            if (currentPlayer.HasEmptyHand())
            {
                Console.WriteLine($"\n {currentPlayer.Name} played all their cards and wins immediately! ");
                return;
            }

            (currentPlayer, opponent) = (opponent, currentPlayer);
        }
    }
}

/* ========================== Program Class: Entry Point ========================== */
class Program
{
    static void Main()
    {
        new Game().Start();
    }
}

/*
        ========================== Stare Down: Console Card Game ==========================
        
        GAME OVERVIEW:
        Stare Down is a strategic card game where the player competes against an AI opponent. 
        Players take turns playing single cards, pairs, or bombs (four-of-a-kind). The goal is to 
        score the highest points or win immediately by playing all cards. Jokers and face cards 
        (J, Q, K, A) have special effects, and suits influence card values.

        WIN CONDITIONS:
        - The player or AI wins immediately if they play all their cards.
        - If the deck is empty and no more moves can be made, the player with the highest score 
        wins.
        - If scores are tied, the player with more remaining cards loses.

        ============================= Project File Structure =============================
        
        Program.cs (THIS FILE) - Entry point that starts the game.
        Game.cs - Controls the main game loop, manages turns, and win conditions.

        Player.cs - Manages player actions, scoring, and special card effects.
        AIPlayer.cs - AI logic for automated decision-making and gameplay.
        
        Card.cs - Represents a playing card, including rank, suit, and value.
        Deck.cs - Handles deck creation, shuffling, and drawing cards.
        Hand.cs - Manages each player's hand, adding/removing cards.
        Rules.cs - Defines valid moves (single card, pairs, bombs).
        
        =========================== How the Rules Are Implemented ===========================
        
        - Turn System: `Game.cs` alternates turns between the Player (User) and the AI.
        - AI Player Logic:
          - The AI plays the highest valid card whenever possible.
          - If no valid move is available, the AI draws a card.
          - AI can play single cards, pairs, or bombs when applicable.
        - Valid Moves: `Rules.cs` ensures that both the Player and AI can only play legal moves.
        - Scoring:
          - Cards have base values (`Card.cs`).
          - Suit effects modify values (♠x2, ♥+2, ♦-1, ♣ no effect).
          - `Player.cs` and `AIPlayer.cs` update scores after each turn.
        - Special Cards:
          - `Player.cs` and `AIPlayer.cs` apply special effects:
            - `J`: +5 points
            - `Q`: Opponent loses 5 points
            - `K`: +10 points but skips the next turn
            - `A`: Opponent must draw a card
          - `Joker` can replace any card or remove the opponent’s highest-value card.
    */

    /*
        The second human player was replaced with an AI opponent, changing the game from player 
        vs. player to player vs. AI. In Game.cs, Player player2 was replaced with AIPlayer aiPlayer, 
        ensuring AI inherits from Player to reuse existing gameplay logic.

        A new AIPlayer.cs file was created to handle AI decision-making. The PlayTurn() method was 
        implemented to make AI choose the best possible move, prioritizing bombs first, then pairs, 
        then single cards. If AI has no valid move, it automatically draws a card.

        Game.cs was modified to support AI turns by checking whether currentPlayer is an instance of 
        AIPlayer and calling aiPlayer.PlayTurn(). This allowed AI to follow the same gameplay flow 
        as a human player.

        Player.cs was adjusted to allow AI to use existing mechanics by changing PlayCards() and 
        ApplyCardEffects() from private to protected. AI logic was also updated to correctly apply 
        special effects for J, Q, K, A, and Joker cards.

        Hand.cs was updated to support AI decision-making by adding HasPair() and HasBomb() methods, 
        enabling AI to check if it can play a pair or a bomb. The GetValidMoves() method was 
        improved to let AI filter playable cards and choose the optimal move.

        Deck.cs remained largely unchanged, but its Shuffle() function continues to randomize the 
        deck, and DrawCard() ensures AI can also draw cards when necessary.

        Rules.cs was kept unchanged to maintain the same gameplay logic as the original PVP version.
    */