using UnityEngine;

public class FlyingEnemyBehaviour : EnemyBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float flyingSpeed = 5f;
    [SerializeField]
    private float elevation;

    //References
    private Rigidbody rb;

    void Start()
    {
        transform.position += Vector3.up * elevation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 destination = LevelManager.instance.endPoint.position;
        Vector3 xzPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 xzDestination = new Vector3(destination.x, 0f, destination.z);

        rb.linearVelocity = (xzDestination - xzPosition).normalized * flyingSpeed;

        float xzDistance = Vector3.Distance(xzPosition, xzDestination);

        if (xzDistance < 0.1f)
        {
            // Destroy enemy and inflict damage to the player
            Destroy(gameObject);
            EnemySpawner.onEnemyDestroy.Invoke(gameObject);
            GameManager.onDamagePlayer.Invoke(damageOnDeath);
        }
    }
}
