using UnityEngine;
using TMPro;

public class TextOutline : MonoBehaviour
{
    private TextMeshProUGUI _textView;

    void Awake()
    {
        _textView = GetComponent<TextMeshProUGUI>();

        Material mat = _textView.fontMaterial;

        mat.EnableKeyword("OUTLINE_ON");

        mat.SetFloat("_OutlineWidth", 0.2f);
        mat.SetColor("_OutlineColor", new Color32(255, 128, 0, 255));

        _textView.UpdateMeshPadding();
    }
}