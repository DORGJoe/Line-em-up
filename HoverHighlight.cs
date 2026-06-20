using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    MeshRenderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        foreach (MeshRenderer r in renderers)
        {
            r.material.EnableKeyword("_EMISSION");
            r.material.SetColor("_EmissionColor", Color.purple * 2f);
        }
    }

    private void OnMouseExit()
    {
        foreach (MeshRenderer r in renderers)
        {
            r.material.SetColor("_EmissionColor", Color.black);
        }
    }
}