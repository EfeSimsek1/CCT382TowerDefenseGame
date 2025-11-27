using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    [Header("Attributes")]
    public Transform gunModuleSocket;

    [Header("References")]
    [SerializeField]
    private Card startingGunModule;

    public List<Module> modules = new List<Module>();
    private Card gunModule;
    private bool usingBaseGun;
    
    public int supportModuleLimit;
    public int supportSlotsFilled;

    private BoxCollider bc;
    ShootingController shootControl;

    void Start()
    {
        modules = new List<Module>();
        bc = GetComponent<BoxCollider>();
        shootControl = GetComponent<ShootingController>();
        if (startingGunModule)
        {
            AddModule(startingGunModule);
        }
        usingBaseGun = true;
        supportSlotsFilled = 0;
    }

    public void AddModule(Card card)
    {
        if (card.cardType == Card.CardType.Module)
        {
            if (card.moduleType == Card.ModuleType.Firing && shootControl.firingModule == null)
            {
                #region Add starting module

                GameObject module = Instantiate(card.moduleModel, gunModuleSocket.position, gunModuleSocket.rotation, gunModuleSocket);

                GetComponent<ShootingController>().damage = card.damage;

                //Debug.Log(module.name);

                IFiringModule firingModule = module.GetComponentInChildren<IFiringModule>();

                if (firingModule != null)
                {
                    shootControl.firingModule = firingModule;
                }

                #endregion
            }
            else if(card.moduleType == Card.ModuleType.Firing && usingBaseGun) 
            {
                #region replace starting module

                GameObject module = Instantiate(card.moduleModel, gunModuleSocket.position, gunModuleSocket.rotation, gunModuleSocket);

                IFiringModule firingModule = module.GetComponentInChildren<IFiringModule>();

                if (firingModule != null)
                {
                    shootControl.firingModule.DestroyModule();

                    shootControl.firingModule = firingModule;
                }

                usingBaseGun = false;

                #endregion
            }
            else if(card.moduleType == Card.ModuleType.Support)
            {
                //add support module
                supportSlotsFilled++;
                shootControl.shootDelay /= 2;
            }
        }
    }

    public bool CanAddModule(Card card)
    {
        return (card.moduleType == Card.ModuleType.Firing && usingBaseGun) || (card.moduleType == Card.ModuleType.Support && (supportSlotsFilled < supportModuleLimit));
    }

    public void DamageTurret()
    {
        if (supportSlotsFilled == 0 && !usingBaseGun)
        {
            shootControl.firingModule.DestroyModule();
            shootControl.firingModule = null;
            AddModule(startingGunModule);
            usingBaseGun = true;
        }
        else if (supportSlotsFilled == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            // destroy support module
            supportSlotsFilled--;
        }
    }
}
