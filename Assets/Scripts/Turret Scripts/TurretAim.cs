using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretAim : MonoBehaviour
{
    public LayerMask enemyMask;

    public float targetRadius;

    public Transform target;

    private ModuleController moduleController;

    void Start()
    {
        moduleController = GetComponentInParent<ModuleController>();
    }

    void Update()
    {
        target = null;

        Collider[] detectedEnemyColliders = Physics.OverlapSphere(transform.position, targetRadius, enemyMask);
        List<GameObject> detectedEnemies = new List<GameObject>();

        foreach(Collider collider in detectedEnemyColliders)
        {
            detectedEnemies.Add(collider.gameObject);
        }

        IEnemyFilter enemyFilter = null;

        if (moduleController)
        {
            foreach (Module module in moduleController.modules)
            {
                if (module.filter != null)
                {
                    enemyFilter = module.filter;
                }
            }
        }

        if (enemyFilter != null)
        {
            target = enemyFilter.Filter(detectedEnemies).transform;
        }
        else
        {
            foreach (Collider hitCollider in detectedEnemyColliders)
            {
                if (target == null || Vector3.Distance(transform.position, hitCollider.gameObject.transform.position) < Vector3.Distance(transform.position, target.position))
                {
                    target = hitCollider.gameObject.transform;
                }
            }
        }

        if(target != null)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            transform.LookAt(target.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * targetRadius);
    }
}
