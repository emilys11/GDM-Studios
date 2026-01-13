using UnityEngine;

public class ProceduralSquare : MonoBehaviour
{
    void Awake()
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();

        GetComponent<SpriteRenderer>().sprite =
            Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }
}