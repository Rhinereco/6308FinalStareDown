using System;
using System.Collections.Generic;
using System.Linq;

class Game
{
    private Deck deck;
    private Player player1;
    private Player player2;
    private List<Card> playPile;
    private bool freePlay = false; // If last player drew a card, next player can play anything

    public void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Stare Down!");
        deck = new Deck();
        player1 = new Player("Player 1");
        player2 = new Player("Player 2");
        playPile = new List<Card>();

        deck.Shuffle();
        player1.DrawInitialCards(deck);
        player2.DrawInitialCards(deck);

        PlayGame();
    }

    private void PlayGame()
    {
        Player currentPlayer = player1;
        Player opponent = player2;

        while (deck.HasCards() || !currentPlayer.HasEmptyHand())
        {
            ShowGameStatus();

            bool drewCard = currentPlayer.PlayTurn(playPile, deck, freePlay);

            if (currentPlayer.HasEmptyHand())
            {
                Console.WriteLine($"\n🎉 {currentPlayer.Name} played all their cards and wins immediately! 🎉");
                return;
            }

            freePlay = drewCard; // If a player draws, the next player can play anything

            // If the player played a King, they must skip their next turn
            if (currentPlayer.SkipNextTurn)
            {
                Console.WriteLine($"{currentPlayer.Name} skips their turn due to playing a King.");
                currentPlayer.SkipNextTurn = false; // Reset the skip flag
            }
            else
            {
                (currentPlayer, opponent) = (opponent, currentPlayer); // Swap turns
            }
        }

        Console.WriteLine("\nGame over! The deck is empty. Calculating final scores...");
        DetermineWinner();
    }

    private void ShowGameStatus()
    {
        Console.Clear();
        Console.WriteLine("=====================================");
        Console.WriteLine("          STARE DOWN GAME  ");
        Console.WriteLine("=====================================");
        Console.WriteLine($"Player Scores:\nPlayer 1: {player1.Score} points  |  Player 2: {player2.Score} points");
        Console.WriteLine("=====================================");
    }

    private void DetermineWinner()
    {
        Console.WriteLine($"Final Scores - Player 1: {player1.Score} points | Player 2: {player2.Score} points");

        if (player1.Score > player2.Score) Console.WriteLine("\n🎉 Player 1 wins! 🎉");
        else if (player2.Score > player1.Score) Console.WriteLine("\n🎉 Player 2 wins! 🎉");
        else Console.WriteLine("\n🤝 It's a tie! The player with more remaining cards loses.");
    }
}
