using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(LevelManager.instance.path[LevelManager.instance.path.Length - 1].position);
    }

    private void FixedUpdate()
    {

    }
}
