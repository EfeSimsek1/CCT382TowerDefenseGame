using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float deathAnimTime = 0.5f;
    public bool killTrigger;

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

    public void Damage(int damage, Card.DamageType damageType)
    {
        Debug.Log(gameObject.name + " hit");

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
        if (audioSource) audioSource.PlayOneShot(deathClip);

        // Change the Layer so towers won't shoot at the dying enemy
        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        gameObject.layer = LayerMask.NameToLayer("Default");

        behaviour.enabled = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        //Destroy(GetComponent<Collider>());
        GetComponent<Collider>().enabled = false;

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(deathAnimTime);
        Destroy(gameObject);
    }
}
