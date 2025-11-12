using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth;

    [SerializeField] Card.DamageType[] damageWeaknesses;
    
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }

    public void Damage(int damage, Card.DamageType damageType)
    {
        if (damageWeaknesses.Contains(damageType))
        {
            // TODO: deal extra damage
            currentHealth -= damage;
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            GameManager.onEnemyDefeated.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
