using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretAim : MonoBehaviour
{
    public LayerMask enemyMask;
    public LayerMask cloakedEnemyMask;

    public float targetRadius;
    public float cloakedTargetRadius;

    public Transform target;

    private ModuleController moduleController;

    void Start()
    {
        moduleController = GetComponentInParent<ModuleController>();
    }

    void Update()
    {
       DetermineTarget();

        if(target != null)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            transform.LookAt(target.position);
        }
    }

    private void DetermineTarget()
    {
        target = null;

        Collider[] detectedEnemyColliders = Physics.OverlapSphere(transform.position, targetRadius, enemyMask);
        Collider[] detectedCloakedEnemyColliders = Physics.OverlapSphere(transform.position, cloakedTargetRadius, cloakedEnemyMask);
        List<GameObject> detectedEnemies = new List<GameObject>();

        foreach (Collider collider in detectedEnemyColliders)
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
            Transform closestNonCloaked = null;

            foreach (Collider hitCollider in detectedEnemyColliders)
            {
                if (closestNonCloaked == null || Vector3.Distance(transform.position, hitCollider.gameObject.transform.position) < Vector3.Distance(transform.position, closestNonCloaked.position))
                {
                    closestNonCloaked = hitCollider.gameObject.transform;
                }
            }

            Transform closestCloaked = null;

            foreach (Collider hitCollider in detectedCloakedEnemyColliders)
            {
                if (!hitCollider.gameObject.transform.IsChildOf(transform) && (closestCloaked == null || Vector3.Distance(transform.position, hitCollider.gameObject.transform.position) < Vector3.Distance(transform.position, closestCloaked.position)))
                {
                    closestCloaked = hitCollider.gameObject.transform;
                }
            }

            if (closestNonCloaked == null || (closestNonCloaked != null && closestCloaked != null && Vector3.Distance(transform.position, closestCloaked.position) < Vector3.Distance(transform.position, closestNonCloaked.position)))
            {
                target = closestCloaked;
            }
            else
            {
                target = closestNonCloaked;
            }
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
