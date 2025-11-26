using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static UnityEvent<int> onDamagePlayer = new UnityEvent<int>();
    public static UnityEvent<GameObject> onEnemyDefeated = new UnityEvent<GameObject>();
    public static UnityEvent<int> onGainMoney = new UnityEvent<int>();
    public static UnityEvent<int> onLoseMoney = new UnityEvent<int>();


    [Header("References")]
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private TextMeshProUGUI moneyCount;

    [Header("Attributes")]
    [SerializeField]
    private int maxPlayerHealth = 100;
    [SerializeField]
    private int startingPlayerMoney = 2;

    public static int currentPlayerMoney = 100;
    private int currentPlayerHealth;

    private void Awake()
    {
        onDamagePlayer.AddListener(damage =>
        {
            // Can add effects here for when the player is damaged, like screen shake

            OnDamaged(damage);
        });

        onEnemyDefeated.AddListener(enemy =>
        {
            // Can add effects here for when an enemy is destroyed

            ExtractFromEnemy(enemy);
        });

        onGainMoney.AddListener(amountOfMoney =>
        {
            // Can add effects here for when the player gains money, like a sound effect

            GainMoney(amountOfMoney);
        });

        onLoseMoney.AddListener(amountOfMoney =>
        {
            // Can add effects here for when the player loses money, like a sound effect

            LoseMoney(amountOfMoney);
        });
    }

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        currentPlayerMoney = startingPlayerMoney;
        gameOverText.gameObject.SetActive(false);
        moneyCount.text = $"Money: {currentPlayerMoney}";
    }

    private void OnDamaged(int damage)
    {
        currentPlayerHealth -= damage;
        healthBar.value = (float)currentPlayerHealth / (float)maxPlayerHealth;

        if (currentPlayerHealth <= 0)
        {
            // TODO: Add functionality for player losing that isn't just text appearing on screen

            // Remove this line after implementing game over menu
            gameOverText.gameObject.SetActive(true);
        }
    }

    private void GainMoney(int amountOfMoney)
    {
        currentPlayerMoney += amountOfMoney;
        moneyCount.text = $"Money: {currentPlayerMoney}";
    }

    private void LoseMoney(int amountOfMoney)
    {
        currentPlayerMoney -= amountOfMoney;
        moneyCount.text = $"Money: {currentPlayerMoney}";
    }

    private void ExtractFromEnemy(GameObject enemy)
    {
        EnemyBehaviour enemyB = enemy.GetComponent<EnemyBehaviour>(); 
        if (enemyB)
        {
            GainMoney(enemyB.moneyDroppedOnDeath);
        }
    }
}
