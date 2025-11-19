using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private int damageOnDeath;
    [SerializeField]
    private float speed;

    public int moneyDroppedOnDeath;

    protected NavMeshAgent agent;

    [SerializeField] private AudioClip movementAudioClip;
    [SerializeField] private AudioSource audioSource;

    private Coroutine playCoroutine;

    protected virtual void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        if (movementAudioClip != null)
            playCoroutine = StartCoroutine(PlayRandomly());
    }

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    protected virtual void Update()
    {
        if (agent.enabled) agent.SetDestination(LevelManager.instance.endPoint.position);

        if (Vector3.Distance(transform.position, LevelManager.instance.endPoint.position) <= 0.1f)
        {
            // Destroy enemy and inflict damage to the player
            Destroy(gameObject);
            EnemySpawner.onEnemyDestroy.Invoke(gameObject);
            GameManager.onDamagePlayer.Invoke(damageOnDeath);
        }
    }
    private void FixedUpdate()
    {

    }

    private IEnumerator PlayRandomly()
    {
        while (true)
        {
            float delay = Random.Range(3f, 6f);
            yield return new WaitForSeconds(delay);
            if (movementAudioClip != null)
                audioSource.PlayOneShot(movementAudioClip);
        }
    }

    private void OnDisable()
    {
        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
    }

}
