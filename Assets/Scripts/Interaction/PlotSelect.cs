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
            Destroy(previewTurret.GetComponent<ShootingController>());
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

        Card lastHeldCard = CardInteractionManager.LastHeldCard;

        if (CardInteractionManager.cardReleasedTrigger && lastHeldCard.cardType == Card.CardType.Turret && CardInteractionManager.CanAffordCard(lastHeldCard))
        {
            Destroy(previewTurret);
            Instantiate(turretPrefab, bc.bounds.center + Vector3.up * (bc.bounds.extents.y + turretBC.bounds.extents.y), Quaternion.identity);
            CardInteractionManager.PlayCard(CardInteractionManager.LastHeldCard);
            CardInteractionManager.cardReleasedTrigger = false;
        }
    }

    private GameObject edgeIndicator;

    private void Update()
    {
        CreateEdgeIndicator();
    }

    [SerializeField] private bool showSceneWidget = true;
    [SerializeField] private Color sceneWidgetColor = new Color(0f, 1f, 0f, 0.6f);
    [SerializeField, Range(0.01f, 0.25f)] private float sceneWidgetInsetFraction = 0.10f;
    [SerializeField, Range(1f, 8f)] private float sceneWidgetLinePixels = 3f;

    private void OnDrawGizmos()
    {
        if (!showSceneWidget) return;
        if (bc == null) bc = GetComponent<BoxCollider>();
        if (bc == null) return;

        Vector3 center = bc.bounds.center;
        Vector3 ext = bc.bounds.extents;
        float y = center.y + ext.y + 0.01f; // slightly above surface

        float insetX = ext.x * sceneWidgetInsetFraction;
        float insetZ = ext.z * sceneWidgetInsetFraction;
        float innerExtX = Mathf.Max(0.01f, ext.x - insetX);
        float innerExtZ = Mathf.Max(0.01f, ext.z - insetZ);

        Vector3[] corners = new Vector3[5]
        {
            new Vector3(center.x - innerExtX, y, center.z - innerExtZ),
            new Vector3(center.x - innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z - innerExtZ),
            new Vector3(center.x - innerExtX, y, center.z - innerExtZ) // close loop
        };

        // filled thin quad (semi-transparent)
        Color prevGiz = Gizmos.color;
        Gizmos.color = new Color(sceneWidgetColor.r, sceneWidgetColor.g, sceneWidgetColor.b, sceneWidgetColor.a * 0.22f);
        Vector3 fillCenter = new Vector3(center.x, y, center.z);
        Vector3 fillSize = new Vector3(innerExtX * 2f, 0.01f, innerExtZ * 2f);
        Gizmos.DrawCube(fillCenter, fillSize);
        Gizmos.color = prevGiz;

        // outline using Gizmos (always visible in Scene view). For a crisper, always-on-top outline use editor Handles below.
        prevGiz = Gizmos.color;
        Gizmos.color = sceneWidgetColor;
        for (int i = 0; i < corners.Length - 1; i++)
            Gizmos.DrawLine(corners[i], corners[i + 1]);
        Gizmos.color = prevGiz;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Also draw an always-on-top thicker line using Handles so it shows through scene geometry.
        if (!showSceneWidget) return;
        if (bc == null) bc = GetComponent<BoxCollider>();
        if (bc == null) return;

        Vector3 center = bc.bounds.center;
        Vector3 ext = bc.bounds.extents;
        float y = center.y + ext.y + 0.01f;

        float insetX = ext.x * sceneWidgetInsetFraction;
        float insetZ = ext.z * sceneWidgetInsetFraction;
        float innerExtX = Mathf.Max(0.01f, ext.x - insetX);
        float innerExtZ = Mathf.Max(0.01f, ext.z - insetZ);

        Vector3[] corners = new Vector3[5]
        {
            new Vector3(center.x - innerExtX, y, center.z - innerExtZ),
            new Vector3(center.x - innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z - innerExtZ),
            new Vector3(center.x - innerExtX, y, center.z - innerExtZ)
        };

        // draw always-on-top (ignore depth) thick line
        var prevZ = UnityEditor.Handles.zTest;
        UnityEditor.Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
        var prevColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = sceneWidgetColor;
        UnityEditor.Handles.DrawAAPolyLine(sceneWidgetLinePixels, corners);
        UnityEditor.Handles.color = prevColor;
        UnityEditor.Handles.zTest = prevZ;
    }
    #endif

    private void CreateEdgeIndicator()
    {
        if (bc == null) bc = GetComponent<BoxCollider>();

        // reuse existing indicator if present
        if (edgeIndicator == null)
        {
            edgeIndicator = new GameObject("PlotEdgeIndicator");
            edgeIndicator.transform.SetParent(transform, true);
            edgeIndicator.layer = 0;
            edgeIndicator.AddComponent<LineRenderer>();
        }

        LineRenderer lr = edgeIndicator.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.loop = true;
        lr.positionCount = 4;
        lr.numCornerVertices = 8;
        lr.numCapVertices = 8;

        // compute square corners at the top surface of the box collider
        Vector3 center = bc.bounds.center;
        Vector3 ext = bc.bounds.extents;
        float y = center.y + ext.y + 0.01f; // slightly above the surface to avoid z-fighting

        // inset the indicator so the lines are inside the plot
        float insetFraction = 0.10f; // 10% inset from each side
        float insetX = ext.x * insetFraction;
        float insetZ = ext.z * insetFraction;
        float innerExtX = Mathf.Max(0.01f, ext.x - insetX);
        float innerExtZ = Mathf.Max(0.01f, ext.z - insetZ);

        Vector3[] corners = new Vector3[4]
        {
            new Vector3(center.x - innerExtX, y, center.z - innerExtZ),
            new Vector3(center.x - innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z + innerExtZ),
            new Vector3(center.x + innerExtX, y, center.z - innerExtZ)
        };
        lr.SetPositions(corners);

        // make the line thicker (scale with plot size but clamp to reasonable min/max)
        float width = Mathf.Max(innerExtX, innerExtZ) * 0.16f; // larger multiplier for thicker lines
        lr.startWidth = lr.endWidth = Mathf.Clamp(width, 0.03f, 0.25f);

        Color col = new Color(0f, 1f, 0f, Mathf.Clamp01(turretPreviewTransparency)); // greenish with preview alpha

        // reuse material if present otherwise create a simple one
        if (lr.material == null || lr.material.shader == null || lr.material.shader.name != "Unlit/Color")
        {
            lr.material = new Material(Shader.Find("Unlit/Color"));
        }
        lr.material.color = col;

        lr.startColor = lr.endColor = col;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
    }
    
}
