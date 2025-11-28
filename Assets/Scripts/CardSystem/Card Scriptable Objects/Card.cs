using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public int cost;
    public CardType cardType;
    public string description;

    public enum CardType
    {
        Turret,
        Module
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
}
