using System;
using System.Collections.Generic;
using System.Linq;

/*
    ========================== AIPlayer: Automated Opponent ==========================
    PURPOSE:
    AIPlayer is a subclass of Player that implements automated decision-making.
    It plays cards strategically, choosing the highest valid move.
    If no valid move is available, the AI draws a card.

    AI LOGIC:
    1. Evaluate Possible Moves:
       AI retrieves all valid cards from its hand.
       If it has valid cards, it selects the best possible move.
    2. Choosing a Move:
       If it has a valid single card, it picks the highest-value one.
       If it has a pair, it plays the best pair.
       If it has a bomb (four-of-a-kind)**, it plays the bomb.
    3. If No Valid Moves Exist:
       The AI draws a card from the deck.
    4. Play the Selected Cards:
       AI plays its chosen move and applies any special effects.
    ========================== AIPlayer: Automated Opponent ==========================
*/

class AIPlayer : Player
{
    public AIPlayer(string name) : base(name) { }// Constructor - AIPlayer inherits from Player

    /*
        PARAMETERS:
        playPile: List of cards currently in play.
        deck: The deck of cards to draw from.
        freePlay: If true, the AI can play any card.

        RETURNS:
        true if the AI draws a card.
        false if the AI successfully plays a move.
    */
    public bool PlayTurn(List<Card> playPile, Deck deck, bool freePlay)
    {
        Console.WriteLine($"\n{this.Name} is thinking...");
        // Get all valid moves the AI can play
        List<Card> validCards = hand.GetValidMoves(playPile, freePlay);
        // Check if AI has valid moves
        if (validCards.Count > 0)
        {
            // Choose the highest-value card to maximize score
            Card bestCard = validCards.OrderByDescending(c => c.GetPointValue()).First(); 
            List<Card> toPlay = new List<Card> { bestCard };

            // Check if AI has matching combinations to play
            if (hand.HasBomb(bestCard))// Check bomb first, because if is a bigger combination
            {
                toPlay = hand.GetBomb(bestCard);
            }
            else if (hand.HasPair(bestCard))
            {
                toPlay = hand.GetPair(bestCard);
            }

            PlayCards(toPlay, playPile);
            ApplyCardEffects(toPlay);
            return false;// AI successfully played a move
        }
        else
        {
            hand.AddCard(deck.DrawCard());
            Console.WriteLine($"{this.Name} had no valid move and drew a card.");
            return true;// AI was forced to draw
        }
    }
}

