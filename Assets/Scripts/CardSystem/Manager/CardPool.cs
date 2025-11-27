using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardPool", menuName = "Scriptable Objects/CardPool")]
public class CardPool : ScriptableObject
{
    public List<Card> cards;
}
