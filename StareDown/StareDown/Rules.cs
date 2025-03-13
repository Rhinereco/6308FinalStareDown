using System;
using System.Collections.Generic;


/*
    This class defines the rules for playing valid moves in the game. It ensures that 
    players and AI can only play legal cards based on the game rules. Checks validity 
    for single cards, pairs, and bombs (four-of-a-kind).
*/
class Rules
{
    public static bool IsValidMove(Card playedCard, List<Card> playPile)
    {
        if (playPile.Count == 0) return true; // If it's the first move, any card is valid
        return playedCard.GetPointValue() >= playPile[playPile.Count - 1].GetPointValue();
    }

    public static bool IsValidPair(List<Card> playedCards, List<Card> playPile)
    {
        if (playedCards.Count != 2 || playedCards[0].Rank != playedCards[1].Rank) return false;
        if (playPile.Count < 2) return true; // If no pairs were played before, any pair is valid

        List<Card> lastPlayed = playPile.GetRange(playPile.Count - 2, 2);
        return playedCards[0].GetPointValue() > lastPlayed[0].GetPointValue();
    }

    public static bool IsValidBomb(List<Card> playedCards, List<Card> playPile)
    {
        if (playedCards.Count != 4) return false; // A bomb must be four cards
        string rank = playedCards[0].Rank;
        foreach (Card card in playedCards) if (card.Rank != rank) return false;

        if (playPile.Count < 4) return true; // If no bomb was played before, any bomb is valid
        List<Card> lastPlayed = playPile.GetRange(playPile.Count - 4, 4);
        return playedCards[0].GetPointValue() > lastPlayed[0].GetPointValue();
    }
}
