using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public int cost;
    public CardType cardType;
    public string description;
    public Sprite image;

    public enum CardType
    {
        Turret,
        Module,
        Ability
    }

    public enum DamageType
    {
        Kinetic,
        Thermal,
        Eletric,
        Explosive,
        Corrosive
    }

    public enum ModuleType
    {
        Firing,
        Support
    }

    public enum AbilityType
    {
        AOE
    }
}
