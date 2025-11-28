using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleController : MonoBehaviour
{
    [Header("Attributes")]
    public Transform gunModuleSocket;

    [Header("References")]
    [SerializeField]
    private Card startingGunModule;
    [SerializeField]
    private Sprite slotUnfilled;
    [SerializeField]
    private Sprite slotFilled;
    [SerializeField]
    private Image gunSlotUIImage;
    [SerializeField]
    private Image[] supportSlotUIImages;


    public List<Module> modules = new List<Module>();
    private Card gunModule;
    private bool usingBaseGun;
    
    public int supportModuleLimit;
    public int supportSlotsFilled;

    private BoxCollider bc;
    ShootingController shootControl;
    TurretAim turretAim;


    void Start()
    {
        modules = new List<Module>();
        bc = GetComponent<BoxCollider>();
        shootControl = GetComponent<ShootingController>();
        turretAim = GetComponent<TurretAim>();
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
            if (((ModuleCard)card).moduleType == Card.ModuleType.Firing && shootControl.firingModule == null)
            {
                #region Add starting module

                GameObject module = Instantiate(((ModuleCard)card).moduleModel, gunModuleSocket.position, gunModuleSocket.rotation, gunModuleSocket);

                GetComponent<ShootingController>().damage = ((GunModuleCard)card).damagePerShot;

                //Debug.Log(module.name);

                IFiringModule firingModule = module.GetComponentInChildren<IFiringModule>();

                if (firingModule != null)
                {
                    shootControl.firingModule = firingModule;
                }

                #endregion
            }
            else if(((ModuleCard)card).moduleType == Card.ModuleType.Firing && usingBaseGun) 
            {
                #region replace starting module

                GameObject module = Instantiate(((ModuleCard)card).moduleModel, gunModuleSocket.position, gunModuleSocket.rotation, gunModuleSocket);

                IFiringModule firingModule = module.GetComponentInChildren<IFiringModule>();

                if (firingModule != null)
                {
                    shootControl.firingModule.DestroyModule();

                    shootControl.firingModule = firingModule;
                }

                usingBaseGun = false;

                gunSlotUIImage.sprite = slotFilled;
                #endregion
            }
            else if(((ModuleCard)card).moduleType == Card.ModuleType.Support)
            {
                //add support module
                supportSlotsFilled++;
                ((SupportModuleCard)card).Activate(shootControl, turretAim);
                Debug.Log(shootControl.shootDelay);
                supportSlotUIImages[supportSlotsFilled - 1].sprite = slotFilled;
            }
        }
    }

    public bool CanAddModule(ModuleCard card)
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
            gunSlotUIImage.sprite = slotUnfilled;
        }
        else if (supportSlotsFilled == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            supportSlotUIImages[supportSlotsFilled - 1].sprite = slotFilled;

            // destroy support module
            supportSlotsFilled--;
        }
    }
}
