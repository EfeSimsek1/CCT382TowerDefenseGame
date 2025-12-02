using System.Collections;
using System.Collections.Generic;
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

            Debug.DrawRay(hit.collider.gameObject.transform.position, Vector3.up, Color.yellow);

            int bouncesUsed = FindEnemyChain(mask, hit.collider.gameObject.transform.position, 5, 5, enemiesHit);

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

            #region IMPLEMENT LATER
            /*            List<GameObject> secondEnemiesHit = new List<GameObject>(enemiesHit);

                        // Do it again
                        FindEnemyChain(mask, hit.collider.gameObject.transform.position, 5 - bouncesUsed, 5 - bouncesUsed, enemiesHit);

                        foreach(GameObject enemy in enemiesHit)
                        {
                            secondEnemiesHit.Remove(enemy);
                        }

                        foreach (GameObject enemy in secondEnemiesHit)
                        {
                            ParticleSystem hitParticle = Instantiate(impactEffect, enemy.transform.position, Quaternion.identity);

                            hitParticle.transform.SetParent(enemy.transform);

                            enemy.GetComponent<EnemyHealth>().Damage(damage, damageType);
                        }

                        if (secondEnemiesHit.Count > 1)
                        {
                            for (int i = 1; i < enemiesHit.Count; i++)
                            {
                                TrailRenderer chainTrail = Instantiate(lightningTrail, bulletSpawnPoint.position, Quaternion.identity);

                                StartCoroutine(SpawnTrail(chainTrail, secondEnemiesHit[i - 1].transform.position, secondEnemiesHit[i].transform.position, true, hit.collider.gameObject.transform));
                            }

                        }*/
            #endregion
        }
    }

    private int FindEnemyChain(LayerMask mask, Vector3 pos, int initialBounces, int bouncesLeft, List<GameObject> enemiesHit)
    {
        if (initialBounces == 0) return initialBounces - bouncesLeft;

        GameObject hitEnemy = DetermineClosestInRadius(pos, turretAim.targetRadius, enemiesHit);

        if (hitEnemy == null) return initialBounces - bouncesLeft;

        //Debug.Log(hitEnemy.name, hitEnemy);


        enemiesHit.Add(hitEnemy);

        return FindEnemyChain(mask, hitEnemy.transform.position, initialBounces, bouncesLeft - 1, enemiesHit);
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

    private GameObject DetermineClosestInRadius(Vector3 pos, float radius, List<GameObject> ignoreList)
    {
        Collider[] hits = Physics.OverlapSphere(pos, radius, turretAim.enemyMask);

        GameObject closest = null;
        float closestDistance = float.PositiveInfinity;

        foreach (Collider hit in hits)
        {
            GameObject newEnemy = hit.gameObject;

            if (ignoreList != null && ignoreList.Contains(newEnemy))
                continue;

            //Debug.Log(go.name, go);

            float new_distance = Vector3.Distance(pos, newEnemy.transform.position);

            if (new_distance < closestDistance)
            {
                //Debug.Log($"Closer enemy: {newEnemy.name}, {new_distance}. Original closest enemy: {closest}, {closestDistance}", newEnemy);

                closestDistance = new_distance;
                closest = newEnemy;
            }
            else
            {
                //Debug.Log($"{closestDistance}, {new_distance}", newEnemy);
            }
        }

        return closest;
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
