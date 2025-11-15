using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth;

    [SerializeField] Card.DamageType[] damageWeaknesses;

    private int currentHealth;
    private EnemyBehaviour behaviour;

    private AudioSource audioSource;
    public AudioClip deathClip;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        behaviour = GetComponent<EnemyBehaviour>();
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
            Die();
        }
    }
    
    public void Die()
    {
        EnemySpawner.onEnemyDestroy.Invoke();
        GameManager.onEnemyDefeated.Invoke(gameObject);
        audioSource.PlayOneShot(deathClip);

        // Change the Layer so towers won't shoot at the dying enemy
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        gameObject.layer = LayerMask.NameToLayer("Default");

        behaviour.enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;


        StartCoroutine(WaitAndDestroy());

        System.Collections.IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(2f);
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
