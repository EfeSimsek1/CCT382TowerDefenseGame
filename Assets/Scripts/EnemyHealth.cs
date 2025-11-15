using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth;
    private bool killTrigger;

    [SerializeField] Card.DamageType[] damageWeaknesses;

    private int currentHealth;
    private EnemyBehaviour behaviour;

    private AudioSource audioSource;
    public AudioClip deathClip;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        behaviour = GetComponent<EnemyBehaviour>();
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
            Die();
        }
    }
    
    public void Die()
    {
            killTrigger = true;
        EnemySpawner.onEnemyDestroy.Invoke(gameObject);
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
