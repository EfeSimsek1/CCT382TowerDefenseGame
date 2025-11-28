using UnityEngine;

public interface ISupportModule
{
    void Activate(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem);

    void DestroyModule(ShootingController controller, TurretAim turretAim, OverheatSystem overheatSystem);
}
