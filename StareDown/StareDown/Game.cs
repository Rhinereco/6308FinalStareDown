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
    private List<Card> lastAIMove = new List<Card>(); // take Ai player's last played card


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
        Player opponent = aiPlayer; // Store both players for easy swapping

        while (deck.HasCards() || !currentPlayer.HasEmptyHand()) // Keep playing while deck has cards or players have moves
        {
            ShowGameStatus();

            if (currentPlayer == player)
            {
                /*
                    In the current version, after the player plays a card, the AI immediately plays its move. However, because 
                    this happens too quickly, the player does not know what the AI played. To prevent the player from seeing 
                    the AI’s hand, no delay will be added to the AI’s actions. Instead, when the player plays a card, a message 
                    will be displayed informing them of the AI’s last move.
                */ 
                if (lastAIMove.Count > 0)
                {
                    Console.WriteLine($"AI previously played: {string.Join(", ", lastAIMove)}");
                }
            }

            bool drewCard;
            if (currentPlayer is AIPlayer ai)
            {
                // AI player play the card, and keep the played one
                // if AI cannot play, then draw a new card from the pile
                drewCard = ai.PlayTurn(playPile, deck, freePlay);
                lastAIMove = drewCard ? new List<Card>() : new List<Card>(playPile.GetRange(playPile.Count - 1, 1));
            }
            else
            {
                // Human player's turn, check if human player can play
                drewCard = currentPlayer.PlayTurn(playPile, deck, freePlay); 
            }

            // ver.2025.3.18 modify
            // When someone was forced to draw a new card, since he cannot play, the other player can play any hand they have
            // The other player have no need to follow the last played card
            if (drewCard)
            {
                freePlay = true; // Let the other player play any card they want
            }
            else
            {
                freePlay = false; // if the last played card was followed successively, then keep the order
            }

            // check if someone win
            if (currentPlayer.HasEmptyHand())
            {
                Console.WriteLine($"\n {currentPlayer.Name} played all their cards and wins immediately! ");
                return;
            }

            /*
             I do not use this now
            freePlay = drewCard; // If a player draws, the next player can play anything
            */

            // Swap turns correctly (Player <-> AI)
            (currentPlayer, opponent) = (opponent, currentPlayer);
        }

        Console.WriteLine("\nGame over! The deck is empty. Calculating final scores...");
        DetermineWinner(); // Determine the winner based on score
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
