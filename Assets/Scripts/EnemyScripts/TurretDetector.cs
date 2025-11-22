using UnityEngine;
using UnityEngine.Events;

public class TurretDetector : MonoBehaviour
{
    public static UnityEvent<Transform> onTurretDetected = new UnityEvent<Transform>();
    private SphereCollider detectionField;

    private void Start()
    {
        detectionField = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("turret detected!", other.gameObject);
        onTurretDetected.Invoke(other.transform);
    }

    private void OnDrawGizmos()
    {
        if (detectionField != null)
        {
            Gizmos.DrawWireSphere(transform.position, detectionField.radius);
        }
    }
}
