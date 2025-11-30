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
        Collider[] detectedCloakedEnemyColliders = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * 10f, cloakedTargetRadius, cloakedEnemyMask);

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
            GameObject farthestInPath = null;
            float greatestProgress = 0f;

            #region Base Filter
            foreach(GameObject enemy in detectedEnemies)
            {
                if(enemy.GetComponent<PathProgress>().NormalizedProgress >= greatestProgress)
                {
                    farthestInPath = enemy;
                    greatestProgress = enemy.GetComponent<PathProgress>().NormalizedProgress;
                }
            }

            GameObject closestCloaked = null;
            float closestDistance = float.MaxValue;
            foreach(Collider enemy in detectedCloakedEnemyColliders)
            {
                if(Vector2.Distance(transform.position, enemy.gameObject.transform.position) <= closestDistance)
                {
                    closestCloaked = enemy.gameObject;
                    closestDistance = Vector2.Distance(transform.position, enemy.gameObject.transform.position);
                }
            }

            if (closestCloaked != null)
            {
                target = closestCloaked.transform;
            }
            else if (farthestInPath != null)
            {
                target = farthestInPath.transform;
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
