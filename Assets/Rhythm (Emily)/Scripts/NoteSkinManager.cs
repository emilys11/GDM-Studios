using UnityEngine;

public class NoteSkinManager : MonoBehaviour
{
    public static NoteSkin CurrentSkin;

    public NoteSkin sceneSkin;

    void Awake()
    {
        CurrentSkin = sceneSkin;
    }
}