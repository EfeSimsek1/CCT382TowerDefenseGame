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

    public NavMeshAgent agent;

    [SerializeField] private AudioClip movementAudioClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float distanceTraveled;
    private float totalPathLength;

    private Coroutine playCoroutine;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.SetDestination(LevelManager.instance.endPoint.position);
        distanceTraveled = 0f;
    }

    protected virtual void Update()
    {
        float remaining = Mathf.Max(0f, agent.remainingDistance);
        distanceTraveled = Mathf.Clamp(totalPathLength - remaining, 0f, totalPathLength);

        if (Vector3.Distance(transform.position, LevelManager.instance.endPoint.position) <= 1.2f)
        {
            // Destroy enemy and inflict damage to the player
            Destroy(gameObject);
            EnemySpawner.onEnemyDestroy.Invoke(gameObject);
            GameManager.onDamagePlayer.Invoke(damageOnDeath);
        }
    }

    protected virtual void Awake()
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

    private float CalculatePathLength(NavMeshPath path)
    {
        float length = 0f;
        if (path == null || path.corners.Length < 2)
            return 0f;

        var corners = path.corners;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            length += Vector3.Distance(corners[i], corners[i + 1]);
        }
        return length;
    }
}
