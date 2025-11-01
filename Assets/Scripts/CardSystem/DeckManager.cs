using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> deck;

    private void Awake()
    {
    }

    public void DrawCard(HandManager handManager)
    {

    }

    public void AddCardToDeck(Card card)
    {
        deck.Add(card);
    }

    public void RemoveCardFromDeck(Card card) 
    {
        deck.Remove(card);
    }
}
