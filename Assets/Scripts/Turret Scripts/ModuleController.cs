using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{

    public List<Module> modules = new List<Module>();
    private Card gunModule;
    
    public int moduleLimit;
    public int moduleSlotsFilled;

    private BoxCollider bc;
    ShootingController shootControl;

    void Start()
    {
        modules = new List<Module>();
        bc = GetComponent<BoxCollider>();
        shootControl = GetComponent<ShootingController>();
    }

    public void AddModule(Card card)
    {
        if (card.cardType == Card.CardType.Module)
        {
            // Add module
            if (moduleSlotsFilled == 0)
            {
                GameObject module = Instantiate(card.moduleModel, transform.position, transform.rotation, transform);

                Collider moduleCollider = module.GetComponent<Collider>();

                module.transform.localPosition = transform.forward * (bc.bounds.extents.z + moduleCollider.bounds.extents.z);

                IFiringModule firingModule = module.GetComponent<IFiringModule>();

                if (firingModule != null)
                {
                    shootControl.firingModule = firingModule;
                }
            }
           
            //modules.Add(module);
            moduleSlotsFilled++;
        }
    }

    public bool CanAddModule()
    {
        return moduleSlotsFilled < moduleLimit;
    }

    public void DamageTurret()
    {
        if (moduleSlotsFilled > 0)
        {
            
        }
    }
}
