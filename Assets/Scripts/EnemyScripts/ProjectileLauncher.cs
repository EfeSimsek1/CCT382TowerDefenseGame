using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    public Transform target;
    public float apexHeight = 5f;      // Height relative to start point
    private Rigidbody projectile;        // The object you want to launch

    [Header("Debug")]
    public bool launchOnStart = false;

    void Start()
    {
        projectile = GetComponent<Rigidbody>();

        if (launchOnStart)
        {
            LaunchProjectile();
        }
    }

    public void LaunchProjectile()
    {
        projectile.linearVelocity = CalculateLaunchVelocity(transform.position, target.position, apexHeight);
    }

    /// Calculates the initial velocity needed to launch from A to B with a specific apex height.
    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Physics.gravity.y; // Usually -9.81

        // --- Vertical motion ---
        float displacementY = end.y - start.y;

        // Choose a vertical initial velocity that reaches the desired apex.
        float initialVelocityY = Mathf.Sqrt(-2f * gravity * height);

        // Time to reach apex:
        float timeToApex = initialVelocityY / -gravity;

        // The apex height relative to the start point
        float apexY = start.y + height;

        // Time from apex to target:
        float timeFromApex = Mathf.Sqrt(2f * Mathf.Max(0, apexY - end.y) / -gravity);

        float totalTime = timeToApex + timeFromApex;

        // --- Horizontal motion ---
        Vector3 horizontalDisplacement = new Vector3(end.x - start.x, 0f, end.z - start.z);
        Vector3 initialVelocityXZ = horizontalDisplacement / totalTime;

        return new Vector3(initialVelocityXZ.x, initialVelocityY, initialVelocityXZ.z);
    }
}
