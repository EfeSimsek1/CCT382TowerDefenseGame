using UnityEngine;

public class FlamethrowerModule : MonoBehaviour, IFiringModule
{
    [Header("References")]
    [SerializeField]
    GroupHitBox hitBox;
    [SerializeField]
    ParticleSystem flameThrowerSystem;

    private TurretAim turretAim;
    private ShootingController shootingController;


    void Start()
    {
        turretAim = GetComponentInParent<TurretAim>();
        shootingController = GetComponentInParent<ShootingController>();
    }

    void Update()
    {
        //Debug.Log(turretAim == null);

        if (turretAim.target != null && !flameThrowerSystem.isPlaying && shootingController.canFire)
        {
            flameThrowerSystem.Play();
        }
        else if((turretAim.target == null || !shootingController.canFire) && flameThrowerSystem.isPlaying)
        {
            flameThrowerSystem.Stop();
        }
    }

    public void DestroyModule()
    {
        Destroy(gameObject);
    }

    public void Shoot(LayerMask mask, int damage, Card.DamageType damageType)
    {
        foreach (EnemyHealth enemy in hitBox.enemiesDetected)
        {
            enemy.Damage(damage, damageType);
        }
    }

}
