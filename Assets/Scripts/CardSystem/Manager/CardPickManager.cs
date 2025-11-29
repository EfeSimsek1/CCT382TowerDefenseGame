using UnityEngine;

public class CardPickManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPickMenu;
    [SerializeField] CreateCardPick[] cardPicks;
    public static CardPickManager instance;

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

        cardPickMenu.SetActive(false);
    }

    public void OpenCardPickMenu()
    {
        cardPickMenu.SetActive(true);
        foreach(CreateCardPick cardPick in cardPicks)
        {
            cardPick.GenerateNewCard();
        }
    }

    public void CloseCardMenu()
    {
        cardPickMenu.SetActive(false);
    }


}
