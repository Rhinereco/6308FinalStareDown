using System;

class Card
{
    public string Suit { get; private set; }
    public string Rank { get; private set; }
    public int BaseValue { get; private set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
        BaseValue = DetermineBaseValue(rank);
    }

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

    // ✅ Fix: Implement GetPointValue() to apply suit effects
    public int GetPointValue()
    {
        int finalValue = BaseValue;

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

        return finalValue > 0 ? finalValue : 0; // Ensure points never go below 0
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
