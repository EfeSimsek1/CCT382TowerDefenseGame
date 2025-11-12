using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static UnityEvent<int> onDamagePlayer = new UnityEvent<int>();

    [Header("References")]
    [SerializeField]
    private Slider healthBar;

    [Header("Attributes")]
    [SerializeField]
    private int maxPlayerHealth = 100;

    private int currentPlayerHealth;

    private void Awake()
    {
        onDamagePlayer.AddListener(damage =>
        {
            // Can add effects for when the player is damaged here, like screen shake

            OnDamaged(damage);
        });
    }

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDamaged(int damage)
    {
        currentPlayerHealth -= damage;
        healthBar.value = (float)currentPlayerHealth / (float)maxPlayerHealth;

        if (currentPlayerHealth <= 0)
        {
            // TODO: Add functionality for player losing
        }
    }
}
