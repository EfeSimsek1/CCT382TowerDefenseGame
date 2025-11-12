using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public int health;
    public int damage;
    public CardType cardType;
    public List<DamageType> damageType;
    public GameObject moduleModel;

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
}
