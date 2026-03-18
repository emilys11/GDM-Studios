using System.Collections.Generic;
using UnityEngine;

public class NoteLane : MonoBehaviour
{
    private Queue<INote> notes = new Queue<INote>();
    private HitBar hitBar;
    public HitBarAnim hitAnim;

    void OnEnable()
    {
        RhythmEvents.OnDeath += ResetLane;
        RhythmEvents.OnWin += ResetLane;
    }

    void OnDisable()
    {
        RhythmEvents.OnDeath -= ResetLane;
        RhythmEvents.OnWin -= ResetLane;
    }

    void Awake()
    {
        hitBar = GetComponent<HitBar>();
    }

    void ResetLane()
    {
        notes.Clear();
        if (hitAnim != null)
            hitAnim.EndHold();
    }

    public void Register(INote note)
    {
        notes.Enqueue(note);
    }

    public void Unregister(INote note)
    {
        if (notes.Count > 0 && notes.Peek() == note)
            notes.Dequeue();
    }

    public void HandleInput()
    {
        if (notes.Count == 0)
        {
            RhythmEvents.NoteMissed();
            hitAnim.PlayFeedback(hitAnim.missSprite);
            return;
        }

        INote note = notes.Peek();
        bool resolved = note.TryResolve();

        if (resolved)
        {
            notes.Dequeue();
            hitAnim.PlayFeedback(hitAnim.hitSprite);
        }
    }

    public bool IsKeyHeld()
    {
        return hitBar.IsKeyHeld();
    }

    public void StartHold()
    {
        if (hitAnim != null)
            hitAnim.StartHold();
    }

    public void EndHold()
    {
        if (hitAnim != null)
            hitAnim.EndHold();
    }

    public void PlayHit()
    {
        hitAnim.PlayFeedback(hitAnim.hitSprite);
    }

    public void PlayMiss()
    {
        hitAnim.PlayFeedback(hitAnim.missSprite);
    }
}