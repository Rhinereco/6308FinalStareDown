using System;
using System.Collections.Generic;

/*
    ========================== Game: Main Game Logic ==========================
    PURPOSE:
    This class controls the main game loop and player turns.
    Manages the deck, player actions, and win conditions.
    Alternates turns between the human player and the AI opponent.

    MAIN RESPONSIBILITIES:
    1. Game Setup:
       Initializes the deck and shuffles it.
       Creates player and AI instances and gives them starting cards.
    2. Game Loop:
       Alternates turns between the human player and the AI.
       Ensures valid moves and turn-based mechanics.
       Checks if a player has played all their cards (immediate win).
    3. Scoring & Win Conditions:
       Tracks player and AI scores.
       Ends the game when the deck is empty or a player has no cards.
       Determines the winner based on score or who has fewer remaining cards.
    ========================== Game: Main Game Logic ==========================
*/

class Game
{
    private Deck deck; // The deck of 54 playing cards
    private Player player;
    private Player aiPlayer; // The AI opponent (stored as Player type for polymorphism)
    private List<Card> playPile;// Stores the current pile of played cards
    private bool freePlay = false; // If last player drew a card, next player can play anything

    // Initializes the game
    public void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Stare Down!");

        deck = new Deck();// Create and shuffle the deck
        player = new Player("You"); // Human player
        aiPlayer = new AIPlayer("AI Bot"); // AI player stored as Player (polymorphism)
        playPile = new List<Card>();

        deck.Shuffle();

        player.DrawInitialCards(deck); // Deal starting hand to player
        aiPlayer.DrawInitialCards(deck); // Deal starting hand to AI

        PlayGame();// Begin the game loop
    }

    // Controls the main game loop
    private void PlayGame()
    {
        Player currentPlayer = player;
        Player opponent = aiPlayer; // Store both as Player for easy swapping

        while (deck.HasCards() || !currentPlayer.HasEmptyHand())
        {
            ShowGameStatus();

            bool drewCard;
            if (currentPlayer is AIPlayer ai)
            {
                drewCard = ai.PlayTurn(playPile, deck, freePlay); // AI makes a move
            }
            else
            {
                drewCard = currentPlayer.PlayTurn(playPile, deck, freePlay); // Human makes a move
            }

            if (currentPlayer.HasEmptyHand())
            {
                Console.WriteLine($"\n {currentPlayer.Name} played all their cards and wins immediately! ");
                return;
            }

            freePlay = drewCard; // If a player draws, the next player can play anything

            // Swap turns correctly
            (currentPlayer, opponent) = (opponent, currentPlayer);
        }

        Console.WriteLine("\nGame over! The deck is empty. Calculating final scores...");
        DetermineWinner();
    }

    private void ShowGameStatus()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("          STARE DOWN          ");
        Console.WriteLine("=====================================");
        Console.WriteLine($"Player Score: {player.Score} points  |  AI Score: {aiPlayer.Score} points");
        Console.WriteLine("=====================================");
    }

    private void DetermineWinner()
    {
        Console.WriteLine($"Final Scores - You: {player.Score} points | AI: {aiPlayer.Score} points");

        if (player.Score > aiPlayer.Score) Console.WriteLine("\n You win! ");
        else if (aiPlayer.Score > player.Score) Console.WriteLine("\n AI wins! Better luck next time. ");
        else Console.WriteLine("\n It's a tie! Tiebreaker: The player with more remaining cards loses.");
    }
}
