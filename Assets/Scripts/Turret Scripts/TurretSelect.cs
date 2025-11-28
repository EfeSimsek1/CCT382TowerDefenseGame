using UnityEngine;

public class TurretSelect : Interactable
{
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private GameObject selectIndicator;
    [SerializeField] private GameObject moduleSlotsIndicator;
    [SerializeField] private float modulePreviewTransparency;
    [SerializeField] private GameObject previewModule;
    private float turretRadius;
    private ModuleController mc;
    private BoxCollider bc;

    void Start()
    {
        turretRadius = GetComponent<TurretAim>().targetRadius;
        rangeIndicator.SetActive(false);
        mc = GetComponent<ModuleController>();
        bc = GetComponent<BoxCollider>();
    }

    void Update()
    {
        rangeIndicator.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        rangeIndicator.transform.localScale = new Vector3(turretRadius, rangeIndicator.transform.localScale.y, turretRadius);
    }

    public override void OnMouseDownObj()
    {
        base.OnMouseDownObj();
    }

    public override void OnMouseEnterObj()
    {
        base.OnMouseEnterObj();
        rangeIndicator.SetActive(true);
        selectIndicator.SetActive(true);
        moduleSlotsIndicator.SetActive(true);

        CardInteractionManager.cardReleasedTrigger = false;

        Card lastCard = CardInteractionManager.LastHeldCard;
        Card heldCard = CardInteractionManager.HeldCard;

        if (CardInteractionManager.IsCardHeld() && heldCard.cardType == Card.CardType.Module && ((ModuleCard)lastCard).moduleModel != null)
        {
            if (mc.CanAddModule((ModuleCard)heldCard))
            {
                previewModule = Instantiate(((ModuleCard)heldCard).moduleModel, mc.gunModuleSocket.position, mc.gunModuleSocket.rotation, mc.gunModuleSocket);
                previewModule.GetComponentInChildren<PreviewObject>().transparency = modulePreviewTransparency;

                Destroy(previewModule.GetComponent<ShootingController>());
                Destroy((MonoBehaviour)previewModule.GetComponentInChildren<IFiringModule>());
            }
            else
            {
                // Can't add any more modules. Display invalid indicator
            }

        }

    }

    public override void OnMouseExitObj()
    {
        base.OnMouseExitObj();
        rangeIndicator.SetActive(false);
        selectIndicator.SetActive(false);
        moduleSlotsIndicator.SetActive(false);

        // Destroy Module Preview
        Destroy(previewModule);
    }

    public override void OnMouseHoverObj()
    {
        base.OnMouseHoverObj();

        Card lastCard = CardInteractionManager.LastHeldCard;

        if (CardInteractionManager.cardReleasedTrigger && lastCard.cardType == Card.CardType.Module && ((ModuleCard)lastCard).moduleModel != null && CardInteractionManager.CanAffordCard(lastCard))
        {
            // Add module to turret, and add module model. Also destroy module preview
            Destroy(previewModule);

            if (mc.CanAddModule((ModuleCard)CardInteractionManager.LastHeldCard))
            {
                mc.AddModule(CardInteractionManager.LastHeldCard);
            }
            CardInteractionManager.PlayCard(CardInteractionManager.LastHeldCard);
            CardInteractionManager.cardReleasedTrigger = false;
        }
        else if (CardInteractionManager.cardReleasedTrigger && lastCard.cardType == Card.CardType.Module && CardInteractionManager.CanAffordCard(lastCard))
        {
            // Add module to turret, but not the module model. Also destroy module preview
            Destroy(previewModule);

            if (mc.CanAddModule((ModuleCard)CardInteractionManager.LastHeldCard))
            {
                mc.AddModule(CardInteractionManager.LastHeldCard);
            }

            CardInteractionManager.PlayCard(CardInteractionManager.LastHeldCard);
            CardInteractionManager.cardReleasedTrigger = false;
        }
    }

    public override void OnMouseUpObj()
    {
        base.OnMouseUpObj();
    }
}
