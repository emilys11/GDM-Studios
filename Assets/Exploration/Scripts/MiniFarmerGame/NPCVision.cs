using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NPCVision : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float visionDistance = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private float coneDirectionOffset = 45f;
    [SerializeField] private float eyeHeightOffset = 1.5f;
    [SerializeField] private float playerTargetHeightOffset = 1.0f;

    [Header("Cone Visual")]
    [SerializeField] private int coneSegments = 20;
    [SerializeField] private float coneHeightOffset = 1.0f;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = false;
        lineRenderer.enabled = false;
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 origin = transform.position + Vector3.up * eyeHeightOffset;
        Vector3 target = player.position + Vector3.up * playerTargetHeightOffset;

        Vector3 baseDirection = Quaternion.Euler(0f, coneDirectionOffset, 0f) * transform.forward;
        Vector3 toPlayer = target - origin;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > visionDistance)
            return false;

        Vector3 dirToPlayer = toPlayer.normalized;

        float angleToPlayer = Vector3.Angle(baseDirection, dirToPlayer);
        if (angleToPlayer > visionAngle * 0.5f)
            return false;

#if UNITY_EDITOR
        Debug.DrawRay(origin, dirToPlayer * distanceToPlayer, Color.red);

        Vector3 leftEdge = Quaternion.Euler(0f, -visionAngle * 0.5f, 0f) * baseDirection;
        Vector3 rightEdge = Quaternion.Euler(0f, visionAngle * 0.5f, 0f) * baseDirection;

        Debug.DrawRay(origin, leftEdge * visionDistance, Color.yellow);
        Debug.DrawRay(origin, baseDirection * visionDistance, Color.green);
        Debug.DrawRay(origin, rightEdge * visionDistance, Color.yellow);
#endif

        if (Physics.Raycast(origin, dirToPlayer, out RaycastHit hit, distanceToPlayer, visionMask))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    public void ShowCone(bool show)
    {
        if (lineRenderer == null) return;

        lineRenderer.enabled = show;

        if (show)
        {
            DrawVisionCone();
        }
    }

    private void LateUpdate()
    {
        if (lineRenderer != null && lineRenderer.enabled)
        {
            DrawVisionCone();
        }
    }

    private void DrawVisionCone()
    {
        if (lineRenderer == null) return;

        Vector3 origin = transform.position + Vector3.up * coneHeightOffset;
        Vector3 baseDirection = Quaternion.Euler(0f, coneDirectionOffset, 0f) * transform.forward;

        int pointCount = coneSegments + 3;
        lineRenderer.positionCount = pointCount;

        float startAngle = -visionAngle * 0.5f;
        float angleStep = visionAngle / coneSegments;

        lineRenderer.SetPosition(0, origin);

        for (int i = 0; i <= coneSegments; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, currentAngle, 0f) * baseDirection;
            Vector3 endPoint = origin + direction.normalized * visionDistance;
            lineRenderer.SetPosition(i + 1, endPoint);
        }

        lineRenderer.SetPosition(pointCount - 1, origin);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * coneHeightOffset;
        Vector3 baseDirection = Quaternion.Euler(0f, coneDirectionOffset, 0f) * transform.forward;

        Vector3 leftEdge = Quaternion.Euler(0f, -visionAngle * 0.5f, 0f) * baseDirection;
        Vector3 rightEdge = Quaternion.Euler(0f, visionAngle * 0.5f, 0f) * baseDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(origin, leftEdge * visionDistance);
        Gizmos.DrawRay(origin, rightEdge * visionDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(origin, baseDirection * visionDistance);
    }
}