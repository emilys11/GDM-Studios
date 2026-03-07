using System;
using UnityEngine;

public static class RhythmInput
{
    public static Action<HitBarType> OnHitInput;

    public static void Fire(HitBarType type)
    {
        OnHitInput?.Invoke(type);
    }
}