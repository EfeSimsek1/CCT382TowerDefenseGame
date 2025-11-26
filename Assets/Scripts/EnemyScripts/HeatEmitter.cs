using Unity.VisualScripting;
using UnityEngine;

public class HeatEmitter : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float heat;

    private SphereCollider aura;

    private void Awake()
    {
        aura = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        OverheatSystem os;
        os = other.gameObject.GetComponentInChildren<OverheatSystem>();
        if (os != null) 
        {
            float distanceMultiplier = (aura.radius / Vector3.Distance(transform.position, other.gameObject.transform.position));
            Debug.Log(distanceMultiplier);
            os.HeatUp((distanceMultiplier * heat) * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        if (aura != null) 
        {
            Gizmos.DrawWireSphere(transform.position, aura.radius);
        }
    }
}
