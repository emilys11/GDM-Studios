using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float groundDistance;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null) {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDistance;
                transform.position = movePos;
            }
        }

        

}

    void FixedUpdate()
    {
        // Get the player's input for horizontal and vertical movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Get the camera's forward direction, but ignore the Y axis (since it's 2.5D, we only care about X and Z)
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;  // Ignore the vertical (Y) axis since we are working in 2.5D
        cameraForward.Normalize();

        // Get the camera's right direction
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;  // Ignore the vertical (Y) axis
        cameraRight.Normalize();

        // Calculate movement direction based on the camera's rotation
        Vector3 moveDir = cameraForward * z + cameraRight * x;

        // Apply the movement to the Rigidbody
        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);
    }



}