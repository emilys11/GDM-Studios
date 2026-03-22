using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private float eyeHeightOffset = 1.5f;
    [SerializeField] private float playerTargetHeightOffset = 1.0f;

    [Header("Rectangle Vision")]
    [SerializeField] private float visionWidth = 4f;
    [SerializeField] private float visionLength = 8f;
    [SerializeField] private float forwardOffset = 4f;

    [SerializeField] private float directionOffset = 45f;

    [Header("Visual")]
    [SerializeField] private GameObject visionRectangleVisual;

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeightOffset;
        Vector3 target = player.position + Vector3.up * playerTargetHeightOffset;

        Vector3 worldToTarget = target - transform.position;
        Vector3 localTarget = Quaternion.Inverse(transform.rotation * Quaternion.Euler(0f, directionOffset, 0f)) * worldToTarget;
        
        float halfWidth = visionWidth * 0.5f;
        float minZ = forwardOffset - (visionLength * 0.5f);
        float maxZ = forwardOffset + (visionLength * 0.5f);

        bool insideRectangle =
            localTarget.x >= -halfWidth &&
            localTarget.x <= halfWidth &&
            localTarget.z >= minZ &&
            localTarget.z <= maxZ;

        if (!insideRectangle)
            return false;

#if UNITY_EDITOR
        Debug.DrawLine(origin, target, Color.red);
#endif

        Vector3 directionToPlayer = (target - origin).normalized;
        float distanceToPlayer = Vector3.Distance(origin, target);

        if (Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, distanceToPlayer, visionMask))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    public void SetVisionActive(bool active)
    {
        if (visionRectangleVisual != null)
        {
            visionRectangleVisual.SetActive(active);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 centerLocal = new Vector3(0f, 0f, forwardOffset);
        Vector3 centerWorld = transform.TransformPoint(centerLocal);

        Vector3 size = new Vector3(visionWidth, 0.1f, visionLength);

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(centerWorld, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = oldMatrix;
    }
}