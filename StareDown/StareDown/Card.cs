using System;

class Card
{
    public string Suit { get; private set; }  // Card suit (♠, ♥, ♦, ♣)
    public string Rank { get; private set; }  // Card rank (3-10, J, Q, K, A)
    public int BaseValue { get; private set; }  // Card base value (before suit effects)

    // ✅ Updated constructor to accept three arguments
    public Card(string suit, string rank, int baseValue)
    {
        Suit = suit;
        Rank = rank;
        BaseValue = baseValue;
    }

    // ✅ Alternate constructor to maintain backward compatibility (without baseValue)
    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
        BaseValue = DetermineBaseValue(rank);
    }

    // Determines base point value based on rank
    private int DetermineBaseValue(string rank)
    {
        return rank switch
        {
            "J" => 10,
            "Q" => 10,
            "K" => 10,
            "A" => 15,
            "2" => 20,
            "Joker" => 25,
            _ => int.Parse(rank) // Convert numbers (3-10) to integer values
        };
    }

    public int GetPointValue()
    {
        int finalValue = BaseValue;

        /*
            ============================
            if 2♠，value = 20 × 2 = 40
            if 2♥，value = 20 + 2 = 22
            if 2♦，value = 20 - 1 = 19
            if 2♣，value = 20（no change）
            ============================
        */
        // Apply suit effects
        switch (Suit)
        {
            case "Spades":   // ♠ Doubles the card's value
                finalValue *= 2;
                break;
            case "Hearts":   // ♥ Adds 2 points
                finalValue += 2;
                break;
            case "Diamonds": // ♦ Decreases 1 point
                finalValue -= 1;
                break;
            case "Clubs":    // ♣ No effect
                break;
        }

        return Math.Max(finalValue, 0); // Ensure points never go below 0
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
