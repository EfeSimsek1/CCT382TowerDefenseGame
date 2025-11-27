using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawPileManager : MonoBehaviour
{
    List<Card> drawPile = new List<Card>();
    private HandManager handManager;
    private DeckManager deckManager;
    private DiscardManager discardManager;
    public Image radialDrawBar;
    public TextMeshProUGUI drawCount;
    public float drawTime = 1f;
    public float lastTimeDrawn = 0f;
    public int startingHandSize = 4;

    private void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
        deckManager = FindFirstObjectByType<DeckManager>();
        discardManager = FindFirstObjectByType<DiscardManager>();

        drawPile = ShuffleCards(deckManager.deck);

        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }

    private void Update()
    {
        if (lastTimeDrawn + drawTime < Time.time)
        {
            DrawCard(handManager);
            lastTimeDrawn = Time.time;
        }

        radialDrawBar.fillAmount = 1 - ((Time.time - lastTimeDrawn) / drawTime);
        //Debug.Log((Time.time - lastTimeDrawn) / drawTime);
    }

    public void DrawCard(HandManager handManager)
    {
        //TODO : Make it so when you draw a card when you have max cards that card is immediately added to the discard pile

        if (drawPile.Count > 0)
        {
            Card nextCard = drawPile[drawPile.Count - 1];
            drawPile.RemoveAt(drawPile.Count - 1);

            handManager.AddCardToHand(nextCard);
        }
        else if (discardManager.discardPile.Count > 0)
        {
            drawPile = ShuffleCards(discardManager.PullAllFromDiscard()); // Shuffle discard pile into drawPile
            DrawCard(handManager);
        }

        UpdateDrawCount();
    }

    private void UpdateDrawCount()
    {
        drawCount.text = drawPile.Count.ToString();
    }

    public List<Card> ShuffleCards(List<Card> list)
    {
        List<Card> shuffledList = new List<Card>();
        List<Card> temp = new List<Card>(list);

        for (int i = 0; i < list.Count; i++)
        {
            int index = Random.Range(0, temp.Count - 1);
            shuffledList.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return shuffledList;
    }
}
