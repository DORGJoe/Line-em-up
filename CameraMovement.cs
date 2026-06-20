using UnityEngine;
/// <summary>
/// Moves this object (intended for Main Camera) along the Z axis from
/// startZ to endZ at "speed" units/second, then enables "endPanel" once.
/// Attach directly to the Main Camera.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float startZ = 45f;
    public float endZ = -12f;
    public float speed = 5f;

    public GameObject endPanel;

    private bool hasArrived;

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.z = startZ;
        transform.position = pos;
    }

    private void Update()
    {
        if (hasArrived) return;

        Vector3 pos = transform.position;
        pos.z = Mathf.MoveTowards(pos.z, endZ, speed * Time.deltaTime);
        transform.position = pos;

        if (Mathf.Approximately(pos.z, endZ))
        {
            hasArrived = true;
            if (endPanel != null)
                endPanel.SetActive(true);
        }
    }
}