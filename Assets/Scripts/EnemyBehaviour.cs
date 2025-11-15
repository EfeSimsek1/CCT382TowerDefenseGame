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

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        agent.SetDestination(LevelManager.instance.endPoint.position);

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
}
