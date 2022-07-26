using System.Text;

public class Card
{
    public enum Suits { Heart, Spade, Diamond, Clubs }
    public Suits Suit { get; private set; }
    public int Value { get; private set; }

    public Card() { }
    public Card(string card)
    {
        switch (card[0])
        {
            case 'H':
                this.Suit = Suits.Heart;
                break;
            case 'S':
                this.Suit = Suits.Spade;
                break;
            case 'D':
                this.Suit = Suits.Diamond;
                break;
            case 'C':
                this.Suit = Suits.Clubs;
                break;
        }

        this.Value = int.Parse(card.Substring(1));
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(Suit.ToString()[0]);
        builder.Append(Value);

        return builder.ToString();
    }
}
