using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public List<Card> deck;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
