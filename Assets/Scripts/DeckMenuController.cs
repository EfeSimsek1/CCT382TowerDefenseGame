using System.Collections.Generic;
using UnityEngine;

public class DeckMenuController : MonoBehaviour
{
    [SerializeField] GameObject deckMenu;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<RectTransform> cardPositions;
    private List<GameObject> cards;

    private void Start()
    {
        deckMenu.SetActive(false);
        cards = new List<GameObject>();
    }

    public void OpenMenu()
    {
        deckMenu.SetActive(true);
        InitializeCards();
    }

    public void CloseMenu()
    {
        ClearCards();
        deckMenu?.SetActive(false);
    }
    private void InitializeCards()
    {
        for (int i = 0; i < DeckManager.instance.deck.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardPositions[i].position, Quaternion.identity, deckMenu.transform);
            cards.Add(newCard);

            //Set the cardData of the Instantiated card
            newCard.GetComponent<CardDisplay>().cardData = DeckManager.instance.deck[i];
        }
    }

    private void ClearCards()
    {
        foreach(GameObject card in cards)
        {
            Destroy(card);
        }
    }
}
