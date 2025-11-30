using UnityEngine;

[CreateAssetMenu(fileName = "RangeMultiplierCard", menuName = "Card/SupportModule/RangeMultiplierCard")]
public class RangeMultiplierCard : SupportModuleCard
{
    public float multiplier;

    public override void Activate(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem)
    {
        turretAim.targetRadius *= multiplier;
    }

    public override void DestroyModule(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem)
    {
        turretAim.targetRadius /= multiplier;
    }
}
