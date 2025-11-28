using UnityEngine;

[CreateAssetMenu(fileName = "DoubleSpeedModuleCard", menuName = "Card/SupportModule/FireRateMultiplierModuleCard")]
public class FireRateMultiplierModuleCard : SupportModuleCard
{
    public float multiplier;

    public override void Activate(ShootingController controller, TurretAim turretAim)
    {
        controller.shootDelay /= multiplier;
    }

    public override void DestroyModule(ShootingController controller, TurretAim turretAim)
    {
        controller.shootDelay *= multiplier;
    }
}
