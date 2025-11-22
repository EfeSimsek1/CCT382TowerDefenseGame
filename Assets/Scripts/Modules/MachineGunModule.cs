using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class MachineGunModule : MonoBehaviour, IFiringModule
{

    [Header("References")]
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem impactEffect;
    public TrailRenderer bulletTrail;
    public Transform bulletSpawnPoint;
    [Header("Attributes")]
    public float BulletSpeed;

    private void Awake()
    {
        var main = muzzleFlashEffect.main;
        main.playOnAwake = false;
        muzzleFlashEffect.gameObject.SetActive(true);
    }

    public GameObject Shoot(LayerMask mask)
    {
        muzzleFlashEffect.Stop();

        // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
        // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

        //Animator.SetBool("IsShooting", true);
        if (muzzleFlashEffect.isStopped)
        {
            muzzleFlashEffect.Play();
        }

        RaycastHit[] hits = Physics.RaycastAll(bulletSpawnPoint.position, transform.forward, float.MaxValue, mask);
        RaycastHit hit = ClosestHit(hits);

        if (hit.collider != null)
        {

            //if (hit.collider.gameObject.name == "Jumper(Clone)" && hit.collider.gameObject.transform.IsChildOf(transform)) Debug.Log("BUG - Jumper is being hit by the turret it's attached to");

            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true, hit.collider.gameObject.transform));

            return hit.collider.gameObject;
        }
        // this has been updated to fix a problem where you cannot fire if you would not hit anything
        // uncomment the code below if you decide to add bullet spread
/*        else
        {
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, bulletSpawnPoint.position + transform.forward * 100, Vector3.zero, false, null));

            return null;
        }*/

        return null;
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

        if (MadeImpact && objectHit)
        {
            //Debug.Log("Spawn particle!");

            ParticleSystem hitParticle = Instantiate(impactEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            //Debug.Log("a", hitParticle);
            hitParticle.transform.SetParent(objectHit);
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    // Returns the closest hit that isn't a hit on a target attached to the turret
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

