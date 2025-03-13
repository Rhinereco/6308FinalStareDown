using System;

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
        Stare Down is a 2-player strategic card game where players take turns playing single cards, 
        pairs, or bombs (four-of-a-kind). The goal is to score the highest points or win immediately 
        by playing all cards. Jokers and face cards (J, Q, K, A) have special effects, and suits 
        affect card values.

        WIN CONDITIONS:
        - A player wins immediately if they play all their cards.
        - If the deck is empty and no more moves can be made, the player with the highest score wins.
        - If scores are tied, the player with more remaining cards loses.

        ============================= Project File Structure =============================
        
        Program.cs (THIS FILE) - Entry point that starts the game.
        Game.cs - Controls the main game loop, turn order, and win conditions.
        Player.cs - Manages player actions, scoring, and special card effects.
        Card.cs - Represents a playing card, including rank, suit, and value.
        Deck.cs - Handles deck creation, shuffling, and drawing cards.
        Hand.cs - Manages each player's hand, adding/removing cards.
        Rules.cs - Defines valid moves (single card, pairs, bombs).
        
        =========================== How the Rules Are Implemented ===========================
        
        - **Turn System**: `Game.cs` alternates turns between Player 1 and Player 2.
        - **Valid Moves**: `Rules.cs` ensures players can only play valid single cards, pairs, or bombs.
        - **Scoring**:
          - Cards have base values (`Card.cs`).
          - Suit effects modify values (♠x2, ♥+2, ♦-1, ♣ no effect).
          - `Player.cs` updates the score after each turn.
        - **Special Cards**:
          - `Player.cs` applies special effects:
            - `J`: +5 points
            - `Q`: Opponent loses 5 points
            - `K`: +10 points but skips next turn
            - `A`: Opponent must draw a card
          - `Joker` can replace any card or remove the opponent’s highest-value card.

        =========================== Game Execution Starts Here ===========================
    */