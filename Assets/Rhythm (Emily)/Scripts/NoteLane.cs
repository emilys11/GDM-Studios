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
        RhythmEvents.OnWin += ResetLane;
        //rhythm events onr eady
        RhythmEvents.OnReady += ResetLane;
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
        Queue<INote> newQueue = new Queue<INote>();

        while (notes.Count > 0)
        {
            var n = notes.Dequeue();
            if (n != note)
                newQueue.Enqueue(n);
        }

        notes = newQueue;
    }

    public void HandleInput()
    {
        if (notes.Count == 0)
        {
            hitAnim.PlayFeedback(hitAnim.emptySprite);
            return;
        }

        var note = notes.Peek();

        if (note == null)
        {
            notes.Dequeue();
            return;
        }

        if (!IsFirstNote(note))
            return;

        bool resolved = note.TryResolve(); 

        if (resolved)
        {
            notes.Dequeue(); 
            hitAnim.PlayFeedback(hitAnim.hitSprite);
        }
        else
        {
            notes.Dequeue(); 
            hitAnim.PlayFeedback(hitAnim.missSprite);
        }
    }

    public bool IsFirstNote(INote note)
    {
        return notes.Count > 0 && notes.Peek() == note;
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