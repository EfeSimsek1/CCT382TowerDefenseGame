using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ShootingController : MonoBehaviour
{
    [Header("Firing attributes")]
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask mask;

    [Header("Damage attributes")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private Card.DamageType damageType;

    [Header("Audio")]
    [SerializeField]
    private AudioClip ShootingAudioClip;

    public IFiringModule firingModule;
    public bool canFire;
    public UnityEvent onShoot = new UnityEvent();
    private Animator Animator;
    private float LastShootTime;
    private AudioSource ShootingAudioSource;
    private TurretAim turretAim;

    private void Awake()
    {
        canFire = true;
        Animator = GetComponent<Animator>();
        turretAim = GetComponentInParent<TurretAim>();

        // Use the AudioSource attached to the same GameObject
        ShootingAudioSource = GetComponent<AudioSource>();
        if (ShootingAudioSource == null)
        {
            //Debug.LogWarning("No AudioSource found on Turret object. Shooting audio will not play.");
        }
    }

    private void Update()
    {
        if (turretAim.target != null && LastShootTime + ShootDelay < Time.time && canFire && firingModule != null)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        onShoot.Invoke();

        // Have firing module shoot
        firingModule.Shoot(mask, damage, damageType);

        LastShootTime = Time.time;

        //Debug.Log("Can fire: " + canFire);
    }

    public void Cool()
    {
        canFire = true;
        LastShootTime = Time.time;
    }

    private void PlayShootingSound()
    {
        if (ShootingAudioSource != null && ShootingAudioClip != null)
        {
            ShootingAudioSource.PlayOneShot(ShootingAudioClip);
        }
    }
}