using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text typeText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public Image[] typeImages;

    void Start()  
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        cardImage.sprite = cardData.image;
        costText.text = cardData.cost.ToString();
        typeText.text = cardData.cardType.ToString();
    }
}
