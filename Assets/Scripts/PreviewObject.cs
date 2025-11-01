using System;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [Range(0, 1)]
    public float transparency = 1f; // 1 = opaque, 0 = invisible
    public bool invalidIndicatorOn;
    public Material invalidIndicatorMaterial;

    private Renderer[] renderers;

    // Internal storage for original materials
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    // Used to detect toggle changes in editor or play mode
    private bool lastToggleState = false;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {

        if (invalidIndicatorOn != lastToggleState)
        {
            if (invalidIndicatorOn)
                ReplaceAllMaterials();
            else
                RestoreOriginalMaterials();

            lastToggleState = invalidIndicatorOn;
        }



        foreach (Renderer rend in renderers)
        {

                foreach (Material mat in rend.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color c = mat.color;
                        c.a = transparency;
                        mat.color = c;

                        // Make sure the material supports transparency mode
                        mat.SetFloat("_Surface", 1); // for URP Lit
                        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }
        }
    }


    private void ReplaceAllMaterials()
    {
        if (invalidIndicatorMaterial == null)
        {
            Debug.LogWarning("No invalid indicator material assigned.");
            return;
        }

        originalMaterials.Clear();

        foreach (Renderer renderer in renderers)
        {
            // Store original materials if not already stored
            if (!originalMaterials.ContainsKey(renderer))
                originalMaterials[renderer] = renderer.sharedMaterials;

            // Replace with the single material for all submeshes
            Material[] newMats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMats.Length; i++)
                newMats[i] = invalidIndicatorMaterial;

            renderer.sharedMaterials = newMats;
        }
    }

    private void RestoreOriginalMaterials()
    {
        foreach (var entry in originalMaterials)
        {
            if (entry.Key != null)
                entry.Key.sharedMaterials = entry.Value;
        }
    }

}