using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    [SerializeField]
    public List<Card> discardPile = new List<Card>();
    public TextMeshProUGUI discardCount;
    public int discardPileCount;

    private void Awake()
    {
        UpdateDiscardCount();
    }

    private void UpdateDiscardCount()
    {
        discardCount.text = discardPile.Count.ToString();
        discardPileCount = discardPile.Count;
    }

    public void AddToDiscard(Card card)
    {
        if (card != null)
        {
            discardPile.Add(card);
            UpdateDiscardCount();
        }
    }

    public Card PullFromDiscard()
    {
        if (discardPile.Count > 0)
        {
            Card cardToReturn = discardPile[discardPile.Count - 1];
            discardPile.RemoveAt(discardPile.Count - 1);
            UpdateDiscardCount();
            return cardToReturn;
        }

        return null;
    }

    public bool PullSelectCardFromDiscard(Card card)
    {
        if (discardPile.Count > 0 && discardPile.Contains(card))
        {
            discardPile.Remove(card);
            UpdateDiscardCount();
            return true;
        }

        return false;
    }

    public List<Card> PullAllFromDiscard()
    {
        if (discardPile.Count > 0)
        {
            List<Card> cardsToReturn = new List<Card>(discardPile);
            discardPile.Clear();
            UpdateDiscardCount();
            return cardsToReturn;
        }

        return new List<Card>();
    }


}
