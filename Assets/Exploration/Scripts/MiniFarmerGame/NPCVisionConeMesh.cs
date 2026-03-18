using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NPCVisionConeMesh : MonoBehaviour
{
    [SerializeField] private float visionDistance = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private float coneDirectionOffset = 45f;
    [SerializeField] private int coneSegments = 20;
    [SerializeField] private float heightOffset = 0.05f;

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "VisionConeMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawCone();
    }

    public void SetVisible(bool visible)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.enabled = visible;
        }
    }

    private void DrawCone()
    {
        Vector3 origin = transform.position + Vector3.up * heightOffset;
        Vector3 baseDirection = Quaternion.Euler(0f, coneDirectionOffset, 0f) * transform.forward;

        int vertexCount = coneSegments + 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[coneSegments * 3];

        vertices[0] = transform.InverseTransformPoint(origin);

        float startAngle = -visionAngle * 0.5f;
        float angleStep = visionAngle / coneSegments;

        for (int i = 0; i <= coneSegments; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, currentAngle, 0f) * baseDirection;
            Vector3 worldPoint = origin + direction.normalized * visionDistance;

            vertices[i + 1] = transform.InverseTransformPoint(worldPoint);
        }

        int triangleIndex = 0;
        for (int i = 1; i <= coneSegments; i++)
        {
            triangles[triangleIndex++] = 0;
            triangles[triangleIndex++] = i;
            triangles[triangleIndex++] = i + 1;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}