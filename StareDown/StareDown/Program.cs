﻿using System;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
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