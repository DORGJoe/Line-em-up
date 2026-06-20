using UnityEngine;

/// <summary>
/// Drives a single wall toward the player at a steady pace and fires the
/// pass/fail check at the moment it reaches the item's plane.
///
/// EDITOR SETUP (per wall prefab):
///  1. Put this on the wall prefab root, alongside WallGapMatch.
///  2. Set "moveDirection" to whatever axis is "toward the player" in your
///     scene (defaults to -Z, i.e. moving toward the origin from +Z).
///  3. Set "checkZ" to the world Z position where the item pivot sits —
///     this is where the match gets evaluated, the instant the wall arrives.
///  4. "speed" is in units/second.
///  5. "destroyDistancePastCheck" cleans the wall up once it's passed the
///     player so prefabs don't pile up behind the camera. A future
///     level/spawner script can replace this with pooling if needed.
///
/// A future level script just needs to Instantiate() one of these prefabs
/// at a starting Z position; this script handles the rest on its own.
/// </summary>
/// 
//[RequireComponent(typeof(WallGapMatch))]
public class WallMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public Vector3 moveDirection = Vector3.back;

    [Header("Check")]
    [Tooltip("World Z position where the item pivot sits — the match is evaluated when the wall arrives here.")]
    public float checkZ = 0f;

    [Header("Cleanup")]
    public float destroyDistancePastCheck = 5f;

    //private WallGapMatch gapMatch;
    private bool hasEvaluated;

    private void Awake()
    {
        //gapMatch = GetComponent<WallGapMatch>();
        GameManager_3D gm = FindAnyObjectByType<GameManager_3D>();
        if(gm.moveToNextLevel == true)
        {
            gm.moveToNextLevel = false;
        }
    }

    private void Update()
    {
        transform.position += moveDirection.normalized * speed * Time.deltaTime;

        if (!hasEvaluated && HasReachedCheckPoint())
            Evaluate();

        if (hasEvaluated && Mathf.Abs(transform.position.z - checkZ) > destroyDistancePastCheck)
        {
            GameManager_3D gameManager_3D = FindAnyObjectByType<GameManager_3D>();
            gameManager_3D.moveToNextLevel = true;

        }
    }

    private bool HasReachedCheckPoint()
    {
        // Works whether the wall approaches from +Z or -Z.
        return moveDirection.z <= 0
            ? transform.position.z <= checkZ
            : transform.position.z >= checkZ;
    }

    private void Evaluate()
    {
        hasEvaluated = true;

    //    //ItemHolder holder = ItemHolder.Instance;
    //    //ItemTag heldTag = holder != null ? holder.CurrentItemTag : null;
    //    //Quaternion heldRotation = holder != null ? holder.CurrentRotation : Quaternion.identity;

    //    //bool result = gapMatch.Evaluate(heldTag, heldRotation);
    //    //Debug.Log(name + " — wall reached player: " + (result ? "MATCH" : "MISMATCH"));
    }

}
