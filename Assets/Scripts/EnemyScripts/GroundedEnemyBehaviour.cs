using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    public int damageOnDeath;
    [SerializeField]
    public float speed;

    public int moneyDroppedOnDeath;

    private NavMeshAgent agent;

    [SerializeField] private AudioClip movementAudioClip;
    [SerializeField] private AudioSource audioSource;

    private Coroutine playCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        agent.SetDestination(LevelManager.instance.endPoint.position);

        if (Vector3.Distance(transform.position, LevelManager.instance.endPoint.position) <= 1.2f)
        {
            // Destroy enemy and inflict damage to the player
            Destroy(gameObject);
            EnemySpawner.onEnemyDestroy.Invoke(gameObject);
            GameManager.onDamagePlayer.Invoke(damageOnDeath);
        }
    }

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        if (movementAudioClip != null)
            playCoroutine = StartCoroutine(PlayRandomly());
    }

    private System.Collections.IEnumerator PlayRandomly()
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

    private void FixedUpdate()
    {

    }
}
