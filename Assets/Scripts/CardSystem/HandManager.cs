using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab; //Assign card prefab in inspector
    public Transform handTransform; //Root of the hand position
    public float fanSpread = 7.5f;
    public float cardSpacing = 100f;
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public int maxHandSize;
    public int currentHandSize;

    private void Start()
    {

    }

    public void AddCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        //Set the cardData of the Instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    public void DiscardCard(Card card)
    {
        GameObject cardToRemove = null;

        foreach (GameObject cardObject in cardsInHand) 
        {
            if(cardObject.GetComponent<CardDisplay>() != null && cardObject.GetComponent<CardDisplay>().cardData == card)
            {
                cardToRemove = cardObject;
            }
        }

        if (cardToRemove != null) 
        {
            cardsInHand.Remove(cardToRemove);
            Destroy(cardToRemove);
        }

        UpdateHandVisuals();
    }

    private void Update()
    {
        //UpdateHandVisuals();

        currentHandSize = cardsInHand.Count;
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for(int i = 0; i < cardCount; i++) 
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalize card position between -1, 1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);

            cardsInHand[i].GetComponent<CardMovement>().SetRestPositionAndRotation(new Vector3(horizontalOffset, verticalOffset, 0f), new Vector3(0f, 0f, rotationAngle));
        }
    }
}
