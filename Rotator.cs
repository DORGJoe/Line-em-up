using UnityEngine;

/// <summary>
/// Click-and-drag rotation for 3D objects, restricted to the Z axis only.
/// On mouse-down, fires a 3D raycast; whatever is hit, this walks up the
/// hierarchy until it finds the first ancestor tagged "Rotator" and treats
/// that as the pivot to rotate (so clicking any child — cylinder, pipe,
/// Cube, etc. — drags the whole key around its own pivot).
///
/// Rotation is computed from the on-screen angle between the pivot and the
/// mouse cursor, so the object visually "follows" the cursor like a dial/
/// clock hand rather than drifting from raw mouse-delta accumulation.
///
/// EDITOR SETUP:
///  1. Put this script on any single object in the scene (e.g. your
///     "RotationManager"), or on the camera. It doesn't need to be on the
///     rotating objects themselves.
///  2. Tag each key's root parent (Key_1, Key_2, ...) with "Rotator".
///  3. Make sure every clickable child (cylinder, pipe, Cube, etc.) has a
///     Collider so the raycast can hit it.
///  4. Assign the camera, or it falls back to Camera.main.
/// </summary>
public class Rotator : MonoBehaviour
{
    [Header("References")]
    public Camera cam;

    [Header("Settings")]
    public string rotatorTag = "Rotator";
    public LayerMask raycastMask = ~0;
    public float maxRayDistance = 200f;

    private Transform currentPivot;
    private float angleOffset;
    private bool isDragging;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        RandomizeAllRotators();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();

        if (Input.GetMouseButtonUp(0))
            StopDrag();

        if (isDragging)
            UpdateDrag();

        if (Input.GetKeyDown(KeyCode.J))
        {
            RandomizeAllRotators();
        }
    }

    private void TryStartDrag()
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, raycastMask))
            return;

        Transform pivot = FindRotatorParent(hit.transform);
        if (pivot == null) return;

        currentPivot = pivot;
        isDragging = true;

        float mouseAngle = GetScreenAngle(currentPivot.position, Input.mousePosition);
        angleOffset = currentPivot.eulerAngles.z - mouseAngle;
    }

    private void StopDrag()
    {
        isDragging = false;
        currentPivot = null;
    }

    private void UpdateDrag()
    {
        if (currentPivot == null) return;

        float mouseAngle = GetScreenAngle(currentPivot.position, Input.mousePosition);
        float newZ = mouseAngle + angleOffset;

        Vector3 e = currentPivot.eulerAngles;
        currentPivot.eulerAngles = new Vector3(e.x, e.y, newZ);
    }

    /// <summary>Walks up from the hit transform until it finds an ancestor (or itself) tagged "Rotator".</summary>
    private Transform FindRotatorParent(Transform hitTransform)
    {
        Transform t = hitTransform;
        while (t != null)
        {
            if (t.CompareTag(rotatorTag))
                return t;
            t = t.parent;
        }
        return null;
    }

    /// <summary>Angle, in degrees, of the mouse position relative to the pivot's projected screen position.</summary>
    private float GetScreenAngle(Vector3 worldPivot, Vector3 mouseScreenPos)
    {
        Vector3 pivotScreenPos = cam.WorldToScreenPoint(worldPivot);
        Vector2 dir = (Vector2)mouseScreenPos - (Vector2)pivotScreenPos;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public  void RandomizeAllRotators()
    {
        GameObject[] rotators = GameObject.FindGameObjectsWithTag(rotatorTag);
        foreach (GameObject r in rotators)
        {
            float randomZ = Random.Range(-60f, 60f);
            Vector3 e = r.transform.eulerAngles;
            r.transform.eulerAngles = new Vector3(e.x, e.y, randomZ);
        }
    }
}