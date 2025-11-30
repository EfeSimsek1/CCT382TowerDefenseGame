using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathProgress : MonoBehaviour
{
    private NavMeshAgent agent;

    // Total length of the current path (start -> destination)
    private float totalPathLength;

    // How far along the path we are (0 = start, totalPathLength = end)
    public float DistanceTraveled { get; private set; }

    // Normalized progress [0,1]
    public float NormalizedProgress
    {
        get
        {
            if (totalPathLength <= 0.01f) return 0f;
            return Mathf.Clamp01(DistanceTraveled / totalPathLength);
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Only update if we have a valid, non-pending path
        if (!agent.hasPath || agent.pathPending ||
            agent.pathStatus != NavMeshPathStatus.PathComplete)
            return;

        // Remaining distance along the path (our own calculation)
        float remaining = GetPathRemainingDistance(agent);

        DistanceTraveled = Mathf.Clamp(totalPathLength - remaining, 0f, totalPathLength);

        // Debug
        //Debug.Log(NormalizedProgress);
    }

    /// <summary>
    /// Call this *right after* you set a new destination, or when the path is recomputed.
    /// </summary>
    public void RecalculateTotalPathLength()
    {
        if (!agent.hasPath || agent.pathPending)
            return;

        totalPathLength = CalculatePathLength(agent.path);
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

    float GetPathRemainingDistance(NavMeshAgent agent)
    {
        var path = agent.path;

        if (agent.pathPending ||
            path.status == NavMeshPathStatus.PathInvalid ||
            path.corners == null ||
            path.corners.Length == 0)
            return Mathf.Infinity;

        float distance = 0f;
        var corners = path.corners;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            distance += Vector3.Distance(corners[i], corners[i + 1]);
        }

        return distance;
    }
}