using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NPCVisionRectangleMesh : MonoBehaviour
{
    [Header("Rectangle Settings")]
    [SerializeField] private float width = 4f;
    [SerializeField] private float length = 8f;
    [SerializeField] private float forwardOffset = 4f;
    [SerializeField] private float heightOffset = 0.05f;

    [Header("Direction")]
    [SerializeField] private float directionOffset = 45f; // keep this if your NPC is rotated

    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "VisionRectangleMesh";
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawRectangle();
    }

    public void SetVisible(bool visible)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.enabled = visible;
        }
    }

    private void DrawRectangle()
    {
        Vector3 origin = transform.position + Vector3.up * heightOffset;

        // Adjust direction like your cone did
        Quaternion rotationOffset = Quaternion.Euler(0f, directionOffset, 0f);
        Vector3 forward = rotationOffset * transform.forward;
        Vector3 right = rotationOffset * transform.right;

        float halfWidth = width * 0.5f;

        // Center of rectangle in front of NPC
        Vector3 center = origin + forward * forwardOffset;

        // 4 corners
        Vector3 frontLeft = center + forward * (length * 0.5f) - right * halfWidth;
        Vector3 frontRight = center + forward * (length * 0.5f) + right * halfWidth;
        Vector3 backLeft = center - forward * (length * 0.5f) - right * halfWidth;
        Vector3 backRight = center - forward * (length * 0.5f) + right * halfWidth;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = transform.InverseTransformPoint(backLeft);
        vertices[1] = transform.InverseTransformPoint(backRight);
        vertices[2] = transform.InverseTransformPoint(frontLeft);
        vertices[3] = transform.InverseTransformPoint(frontRight);

        int[] triangles = new int[]
        {
            0, 2, 1, // first triangle
            2, 3, 1  // second triangle
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}