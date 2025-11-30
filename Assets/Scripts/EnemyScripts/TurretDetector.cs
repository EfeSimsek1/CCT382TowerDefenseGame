using UnityEngine;
using UnityEngine.Events;

public class TurretDetector : MonoBehaviour
{
    public static UnityEvent<Vector3> onTurretDetected = new UnityEvent<Vector3>();
    private SphereCollider detectionField;

    private void Start()
    {
        detectionField = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("turret detected!", other.gameObject);
        onTurretDetected.Invoke(other.GetComponent<BoxCollider>().bounds.center);
    }

    private void OnDrawGizmos()
    {
        if (detectionField != null)
        {
            Gizmos.DrawWireSphere(transform.position, detectionField.radius);
        }
    }
}
