using UnityEngine;
using UnityEngine.EventSystems;

public class CardPick : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        DeckManager.instance.AddCardToDeck(GetComponent<CardDisplay>().cardData);
        SceneController.instance.NextLevel();
    }
}
