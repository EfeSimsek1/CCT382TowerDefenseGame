using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChainLightningModule : MonoBehaviour, IFiringModule
{
    [Header("Attributes")]
    [SerializeField] float bulletSpeed;

    [Header("References")]
    [SerializeField]
    TrailRenderer lightningTrail;
    [SerializeField]
    ParticleSystem impactEffect;
    [SerializeField]
    Transform bulletSpawnPoint;

    private TurretAim turretAim;

    void Start()
    {
        turretAim = GetComponentInParent<TurretAim>();
    }

    void Update()
    {
        
    }

    public void DestroyModule()
    {
        Destroy(gameObject);
    }

    public void Shoot(LayerMask mask, int damage, Card.DamageType damageType)
    {
        RaycastHit[] hits = Physics.RaycastAll(bulletSpawnPoint.position, transform.forward, float.MaxValue, mask);
        RaycastHit hit = ClosestHit(hits);

        if (hit.collider != null)
        {
            TrailRenderer trail = Instantiate(lightningTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position, hit.point, true, hit.collider.gameObject.transform));

            List<GameObject> enemiesHit = new List<GameObject>();
            enemiesHit.Add(hit.collider.gameObject);

            enemiesHit = FindEnemyChain(mask, hit.collider.gameObject.transform.position, 5, enemiesHit);

            //Debug.Log("# of enemies hit: " + enemiesHit.Count);

            foreach(GameObject enemy in enemiesHit)
            {
                ParticleSystem hitParticle = Instantiate(impactEffect, enemy.transform.position, Quaternion.identity);

                hitParticle.transform.SetParent(enemy.transform);

                enemy.GetComponent<EnemyHealth>().Damage(damage, damageType);
            }

            if (enemiesHit.Count > 1)
            {
                for (int i = 1; i < enemiesHit.Count; i++)
                {
                    TrailRenderer chainTrail = Instantiate(lightningTrail, bulletSpawnPoint.position, Quaternion.identity);

                    StartCoroutine(SpawnTrail(chainTrail, enemiesHit[i - 1].transform.position, enemiesHit[i].transform.position, true, hit.collider.gameObject.transform));
                }

            }

        }
    }

    private List<GameObject> FindEnemyChain(LayerMask mask, Vector2 pos, int bounces, List<GameObject> enemiesHit)
    {
        if (bounces == 0) return enemiesHit;

        GameObject hitEnemy = DetermineClosestInRadius(pos, enemiesHit);

        if (hitEnemy == null) return enemiesHit;

        enemiesHit.Add(hitEnemy);

        return FindEnemyChain(mask, pos, bounces - 1, enemiesHit);
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 startPos, Vector3 endPos, bool MadeImpact, Transform objectHit)
    {
        float distance = Vector3.Distance(startPos, endPos);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPos, endPos, 1 - (remainingDistance / distance));

            remainingDistance -= bulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = endPos;

        Destroy(Trail.gameObject, Trail.time);
    }

    private GameObject DetermineClosestInRadius(Vector3 pos, List<GameObject> ignoreList)
    {
        Collider[] detectedEnemyColliders = Physics.OverlapCapsule(pos, pos + Vector3.up * 10f, turretAim.targetRadius, turretAim.enemyMask);
        detectedEnemyColliders.AddRange(Physics.OverlapCapsule(pos, pos + Vector3.up * 10f, turretAim.targetRadius, turretAim.cloakedEnemyMask));
        //Debug.Log(detectedEnemyColliders.Length);

        GameObject closestEnemy = null;
        foreach(Collider enemy in detectedEnemyColliders)
        {
            if (!ignoreList.Contains(enemy.gameObject) && (closestEnemy == null || Vector3.Distance(enemy.transform.position, pos) < Vector3.Distance(closestEnemy.transform.position, pos)))
            {
                closestEnemy = enemy.gameObject;
            }
        }

        return closestEnemy;
    }
    private RaycastHit ClosestHit(RaycastHit[] hits)
    {
        RaycastHit closestHit = new RaycastHit();

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.gameObject.transform.IsChildOf(transform) && Vector3.Distance(transform.position, hit.point) < Vector3.Distance(transform.position, closestHit.point))
            {
                closestHit = hit;
            }
        }

        return closestHit;
    }
}
