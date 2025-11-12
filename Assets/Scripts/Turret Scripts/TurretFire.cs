using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TurretFire : MonoBehaviour
{
    [Header("Firing attributes")]

    [SerializeField]
    private bool AddBulletSpread = false;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float BulletSpeed = 100;

    [Header("Damage attributes")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private Card.DamageType damageType;

    private Animator Animator;
    private float LastShootTime;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        var main = ShootingSystem.main;
        main.playOnAwake = false;
        ShootingSystem.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GetComponentInParent<TurretAim>().target != null && LastShootTime + ShootDelay < Time.time)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            ShootingSystem.Stop();

            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            //Animator.SetBool("IsShooting", true);
            if (ShootingSystem.isStopped)
            {
                ShootingSystem.Play();
            }

            Vector3 direction = GetDirection();

            if (Physics.Raycast(BulletSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, hit.collider.gameObject.transform));

                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                enemy.Damage(damage, damageType);

                LastShootTime = Time.time;
            }
            // this has been updated to fix a problem where you cannot fire if you would not hit anything
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + GetDirection() * 100, Vector3.zero, false, null));

                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact, Transform objectHit)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            ParticleSystem hitParticle = Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            hitParticle.transform.SetParent(objectHit);
        }

        Destroy(Trail.gameObject, Trail.time);
    }
}