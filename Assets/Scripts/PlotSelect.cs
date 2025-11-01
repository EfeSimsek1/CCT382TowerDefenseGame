using UnityEngine;

public class PlotSelect : Interactable
{
    [SerializeField] private GameObject turretPrefab;
    private GameObject previewTurret;
    [SerializeField] private float turretPreviewTransparency;
    BoxCollider bc;
    Collider turretBC;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
        turretBC = GetComponent<Collider>();
    }
    public override void OnMouseEnterObj()
    {
        CardInteractionManager.cardReleasedTrigger = false;

        base.OnMouseEnterObj();
        //Debug.Log($"entered: {gameObject.name}");

        #region Initialize Preview Turret
        if (CardInteractionManager.IsCardHeld() && CardInteractionManager.HeldCard.cardType == Card.CardType.Turret)
        {
            previewTurret = Instantiate(turretPrefab, bc.bounds.center + Vector3.up * (bc.bounds.extents.y + turretBC.bounds.extents.y), Quaternion.identity);
            previewTurret.GetComponent<PreviewObject>().transparency = turretPreviewTransparency;
            float turretRadius = previewTurret.GetComponent<TurretAim>().targetRadius;
            Destroy(previewTurret.GetComponent<Interactable>());
            Destroy(previewTurret.GetComponent<TurretAim>());
            Destroy(previewTurret.GetComponent<TurretFire>());
            previewTurret.layer = 0;
            Transform rangeIndicator = previewTurret.transform.Find("RangeIndicator");
            if (rangeIndicator != null)
            {
                rangeIndicator.gameObject.SetActive(true);
                rangeIndicator.localScale = new Vector3(turretRadius, rangeIndicator.localScale.y, turretRadius);
            }
        }
        #endregion
    }
    public override void OnMouseExitObj()
    {
        base.OnMouseExitObj();
        //Debug.Log($"exited: {gameObject.name}");
        Destroy(previewTurret);
    }

    public override void OnMouseHoverObj()
    {
        base.OnMouseHoverObj();

        if (CardInteractionManager.cardReleasedTrigger && CardInteractionManager.LastHeldCard.cardType == Card.CardType.Turret)
        {
            Destroy(previewTurret);
            Instantiate(turretPrefab, bc.bounds.center + Vector3.up * (bc.bounds.extents.y + turretBC.bounds.extents.y), Quaternion.identity);
            CardInteractionManager.PlayCard(CardInteractionManager.LastHeldCard);
            CardInteractionManager.cardReleasedTrigger = false;
        }
    }
}
