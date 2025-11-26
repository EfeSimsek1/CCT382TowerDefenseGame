using System.Collections;
using UnityEngine;
using static Card;

public interface IFiringModule
{
    void Shoot(LayerMask mask, int damage, DamageType damageType);

    void DestroyModule();
}
