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

    void ResetLane()
    {
        notes.Clear();
    }

    void Awake()
    {
        hitBar = GetComponent<HitBar>();
    }

    public void Register(INote note)
    {
        notes.Enqueue(note);
    }

    //we are removing this floptina note
    public void Unregister(INote note)
    {
        if (notes.Count > 0 && notes.Peek() == note)
        {
            notes.Dequeue();
        }
    }

    //see what the freak is going on when a key is pressed
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

    public void PlayHit()
    {
        hitAnim.PlayFeedback(hitAnim.hitSprite);
    }

    public void PlayMiss()
    {
        hitAnim.PlayFeedback(hitAnim.missSprite);
    }
}