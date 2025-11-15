using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth;
    private bool killTrigger;

    [SerializeField] Card.DamageType[] damageWeaknesses;
    
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        killTrigger = false;
    }

    void Start()
    {

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

        if (currentHealth <= 0 && !killTrigger)
        {
            killTrigger = true;
            EnemySpawner.onEnemyDestroy.Invoke(gameObject);
            GameManager.onEnemyDefeated.Invoke(gameObject);
            Destroy(gameObject);
        }
    }
}
