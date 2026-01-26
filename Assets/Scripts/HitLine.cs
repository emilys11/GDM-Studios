using UnityEngine;

public class HitLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var note = other.GetComponent<Note>();
        if (note != null) note.SetHittable(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var note = other.GetComponent<Note>();
        if (note != null) note.SetHittable(false);
    }
}
