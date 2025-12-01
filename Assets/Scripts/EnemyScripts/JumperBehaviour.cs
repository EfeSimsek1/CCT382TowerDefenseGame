using UnityEngine;

public class JumperBehaviour : EnemyBehaviour
{
    [Header("Jumper Attributes")]
    [SerializeField]
    private float height = 5f;
    [SerializeField]
    private float damageTime = 5f;
    private bool launchInitiated;
    private GameObject victimTurret;
    //private float damageTimer = 0f;


    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();

        launchInitiated = false;

        TurretDetector.onTurretDetected.AddListener(target =>
        {
            agent.enabled = false;
            Launch(target);
        });
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(!launchInitiated)
        {
            base.Update();
        }
    }

    private void Launch(Vector3 target)
    {
        //Debug.Log(CalculateLaunchVelocity(transform.position, target.position, height));
        if (!launchInitiated) rb.linearVelocity = CalculateLaunchVelocity(transform.position, target, height);
        launchInitiated = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.SetParent(collision.gameObject.transform.root);
        rb.isKinematic = true;
        victimTurret = collision.gameObject.transform.root.gameObject;
        InvokeRepeating("DamageTurret", damageTime, damageTime);
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

    private void DamageTurret()
    {
        victimTurret.GetComponent<ModuleController>().DamageTurret();
    }

    private void OnDestroy()
    {
        EnemySpawner.onEnemyDestroy.Invoke(gameObject);
    }
}
