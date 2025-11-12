using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardInteractionManager : MonoBehaviour
{
    private static Card heldCard;
    private static Card lastHeldCard;
    public static Card HeldCard { get { return heldCard; }}
    public static Card LastHeldCard { get { return lastHeldCard; }}

    public static bool cursorOnPlot;
    public static bool cardReleasedTrigger;

    private static HandManager handManager;
    private static DeckManager deckManager;
    private static DiscardManager discardManager;

    private void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
        deckManager = FindFirstObjectByType<DeckManager>();
        discardManager = FindFirstObjectByType<DiscardManager>();


    }

    public static void HoldCard(Card card)
    {
        heldCard = card;
        lastHeldCard = card;
    }

    public static void ReleaseCard()
    {
        lastHeldCard = heldCard;
        heldCard = null;
        cardReleasedTrigger = true;
        //handManager.UpdateHandVisuals();
    }

    public static bool IsCardHeld()
    {
        return heldCard != null;
    }

    public static void PlayCard(Card card)
    {
        discardManager.AddToDiscard(card);
        handManager.DiscardCard(card);
    }
}
