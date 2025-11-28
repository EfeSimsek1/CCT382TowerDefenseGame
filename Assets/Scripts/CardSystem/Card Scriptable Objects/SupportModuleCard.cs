using UnityEngine;

[CreateAssetMenu(fileName = "SupportModuleCard", menuName = "Card/SupportModule")]
public abstract class SupportModuleCard : ModuleCard, ISupportModule
{
    public abstract void Activate(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem);

    public abstract void DestroyModule(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem);
}
