using UnityEngine;

public abstract class AbilityCard : Card, IAbility
{
    public AbilityType abilityType;

    public abstract void Activate();
}
