using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/GunModule")]
public class GunModuleCard: ModuleCard
{
    public int damagePerShot;
    public List<DamageType> damageTypes;
}
