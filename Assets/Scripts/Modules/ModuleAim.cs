using UnityEngine;

public class ModuleAim : MonoBehaviour
{
    private TurretAim turretAim;

    void Start()
    {
        turretAim = GetComponentInParent<TurretAim>();
    }

    void Update()
    {
        if (turretAim != null && turretAim.target != null) 
        {
            transform.LookAt(turretAim.target.position);
        }
    }
}
