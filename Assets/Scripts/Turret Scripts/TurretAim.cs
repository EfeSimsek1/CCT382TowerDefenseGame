using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        Collider[] detectedEnemyColliders = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * 10f, targetRadius, enemyMask);
        Collider[] detectedCloakedEnemyColliders = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * 10f, targetRadius, cloakedEnemyMask);


        List <GameObject> detectedEnemies = new List<GameObject>();

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
            #region Base Filter
            Transform closestNonCloaked = null;

            foreach (Collider hitCollider in detectedEnemyColliders)
            {
                if (closestNonCloaked == null || GetXYDistance(transform.position, hitCollider.gameObject.transform.position) < GetXYDistance(transform.position, closestNonCloaked.position))
                {
                    closestNonCloaked = hitCollider.gameObject.transform;
                }
            }

            Transform closestCloaked = null;

            foreach (Collider hitCollider in detectedCloakedEnemyColliders)
            {
                if (!hitCollider.gameObject.transform.IsChildOf(transform) && (closestCloaked == null || GetXYDistance(transform.position, hitCollider.gameObject.transform.position) < GetXYDistance(transform.position, closestCloaked.position)))
                {
                    closestCloaked = hitCollider.gameObject.transform;
                }
            }

            if (closestNonCloaked == null || (closestNonCloaked != null && closestCloaked != null && GetXYDistance(transform.position, closestCloaked.position) < GetXYDistance(transform.position, closestNonCloaked.position)))
            {
                target = closestCloaked;
            }
            else
            {
                target = closestNonCloaked;
            }
            #endregion
        }
    }

    private Vector3 GetXYPos(Vector3 pos)
    {
        return new Vector3(pos.x, 0f, pos.y);
    }

    private float GetXYDistance(Vector3 p1, Vector3 p2)
    {
        return Vector3.Distance(GetXYPos(p1), GetXYPos(p2));
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * targetRadius);
    }
}
