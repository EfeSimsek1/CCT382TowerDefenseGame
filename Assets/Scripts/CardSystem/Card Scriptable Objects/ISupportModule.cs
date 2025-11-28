using UnityEngine;

public interface ISupportModule
{
    void Activate(ShootingController controller, TurretAim turretAim);

    void DestroyModule(ShootingController controller, TurretAim turretAim);
}
