using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public Module[] modules;
    
    public int moduleLimit;
    public int moduleSlotsFilled;
    public Transform[] modulePositions;

    void Start()
    {
        modules = new Module[0];
        modulePositions = new Transform[moduleLimit];
        for (int i = 0; i < moduleLimit; i++)
        {
            modulePositions[i] = transform.Find("Module Positions").GetChild(i);
        }
    }

    public void AddModule(Card card)
    {
        if (card.cardType == Card.CardType.Module)
        {
            // Add module
            GameObject module = Instantiate(CardInteractionManager.LastHeldCard.moduleModel, modulePositions[moduleSlotsFilled].position, transform.rotation, transform);
            moduleSlotsFilled++;
        }
    }

    public bool CanAddModule()
    {
        return moduleSlotsFilled < moduleLimit;
    }
}
