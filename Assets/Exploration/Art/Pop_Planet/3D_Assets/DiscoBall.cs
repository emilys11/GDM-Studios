using UnityEngine;

public class DiscoBall : MonoBehaviour
{
   

    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
