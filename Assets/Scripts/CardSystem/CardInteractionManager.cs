using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
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

    [Header("Audio")]
    [SerializeField] private AudioClip shootingAudioClipInspector;
    // Expose a static accessor so existing static code can still read the clip,
    // while allowing you to assign it in the Inspector via shootingAudioClipInspector.
    public static AudioClip shootingAudioClip { get { var inst = FindFirstObjectByType<CardInteractionManager>(); return inst != null ? inst.shootingAudioClipInspector : null; } }
    [SerializeField] private AudioClip cardPickupAudioClipInspector;
    public static AudioClip cardPickupAudioClip { get { var inst2 = FindFirstObjectByType<CardInteractionManager>(); return inst2 != null ? inst2.cardPickupAudioClipInspector : null; } }
    public static AudioSource audioSource; 

    private void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
        deckManager = FindFirstObjectByType<DeckManager>();
        discardManager = FindFirstObjectByType<DiscardManager>();
        audioSource = GetComponent<AudioSource>();

    }

    public static void HoldCard(Card card)
    {
        heldCard = card;
        lastHeldCard = card;
        PlayAudioClip(cardPickupAudioClip);
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
        GameManager.onLoseMoney.Invoke(card.cost);
        PlayAudioClip(shootingAudioClip);
    }

    public static bool CanAffordCard(Card card)
    {
        return GameManager.currentPlayerMoney >= card.cost;
    }

    public static void PlayAudioClip(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        Debug.Log("Turret Placed");
    }
}
