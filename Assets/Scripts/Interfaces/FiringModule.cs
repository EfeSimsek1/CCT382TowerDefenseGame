using System.Collections;
using UnityEngine;
using static Card;

public interface IFiringModule
{
    GameObject Shoot(LayerMask mask);
}
