using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Deck : IEnumerator, IEnumerable
{
    public List<Card> CardList { get; set; } = new List<Card>();

    public int Count { get => CardList.Count; }

    int position = -1;

    public IEnumerator GetEnumerator()
    {
        return (IEnumerator)this;
    }
    public bool MoveNext()
    {
        position++;
        return (position < CardList.Count);
    }
    public void Reset()
    {
        position = -1;
    }
    public object Current
    {
        get 
        { 
            return CardList[position]; 
        }
    }

    public Deck() { }
    public Deck(List<Card> cards)
    {
        this.CardList = cards;
    }
    public Deck(string cards)
    {
        string[] splited = cards.Split('|');

        foreach (string card in splited)
        {
            CardList.Add(new Card(card));
        }
    }

    public void Add(Card card)
    {
        this.CardList.Add(card);
    }
    public void Add<T>(List<T> cards)
    {
        foreach (T card in cards)
        {
            this.CardList.Add(new Card(card.ToString()));
        }
    }

    public Card Top()
    {
        return this.CardList[this.CardList.Count - 1];
    }

    public Card Remove()
    {
        if (this.Count > 0)
        {
            Card tempCard = this.CardList[this.CardList.Count - 1];
            this.CardList.RemoveAt(this.CardList.Count - 1);

            return tempCard;
        }

        return null;
    }

    public void Shuffler()
    {
        var rand = new System.Random();

        for (int i = 0; i < this.CardList.Count; i++)
        {
            int position = rand.Next(this.CardList.Count);

            Card cardTemp = this.CardList[i];
            this.CardList[i] = this.CardList[position];
            this.CardList[position] = cardTemp;
        }
    }

    public List<Deck> Divide(int byNum)
    {
        List<Deck> dividedDeck = new List<Deck>();

        for (int i = 0; i < byNum; i++)
        {
            dividedDeck.Add(new Deck());
        }

        int deckIndex = 0;

        for (int i = 0; i < this.CardList.Count; i++)
        {
            dividedDeck[deckIndex].Add(this.CardList[i]);

            deckIndex = deckIndex < (byNum - 1) ? ++deckIndex : 0;
        }

        return dividedDeck;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        foreach (Card card in CardList)
        {
            builder.Append(card.ToString());
            builder.Append("|");
        }
        builder.Remove(builder.Length - 1, 1);

        return builder.ToString();
    }
}
