using UnityEngine;

public class CreateCardPick : MonoBehaviour
{
    [SerializeField] 
    GameObject cardPrefab;
    [SerializeField]
    CardPool cardPool;

    private void Awake()
    {
        GameObject newCard = Instantiate(cardPrefab, transform.position, Quaternion.identity, transform);

        //Set the cardData of the Instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardPool.cards[Random.Range(0, cardPool.cards.Count - 1)];
    }
}
