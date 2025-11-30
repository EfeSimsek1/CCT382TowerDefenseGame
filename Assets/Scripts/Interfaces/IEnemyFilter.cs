using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFilter
{
    public abstract GameObject Filter(List<GameObject> enemies);
}
