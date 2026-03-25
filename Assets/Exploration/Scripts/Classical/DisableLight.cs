using UnityEngine;

public class DisableLight : MonoBehaviour
{
    [SerializeField] public GameObject Light;

    private void OnCollisionEnter(Collision collision)
    {
        Light.SetActive(false);
    }

}
