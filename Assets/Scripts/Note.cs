using UnityEngine;

public class Note : MonoBehaviour
{
    public int laneIndex;          // 0..3
    public float speed = 6f;
    public float destroyY = -6f;

    private bool canBeHit;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < destroyY)
        {
            GameManager.Instance.Miss();
            Destroy(gameObject);
        }
    }

    public void SetHittable(bool value)
    {
        canBeHit = value;
    }

    public bool TryHit()
    {
        if (!canBeHit) return false;

        GameManager.Instance.Hit();
        Destroy(gameObject);
        return true;
    }
}
