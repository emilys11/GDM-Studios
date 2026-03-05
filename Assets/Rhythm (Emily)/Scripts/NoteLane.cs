using System.Collections.Generic;
using UnityEngine;

public class NoteLane : MonoBehaviour
{
    private Queue<INote> notes = new Queue<INote>();
    private HitBar hitBar;

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
            return;
        }

        INote note = notes.Peek();

        bool resolved = note.TryResolve();

        if (resolved)
        {
            notes.Dequeue();
        }
    }

    public bool IsKeyHeld()
    {
        return hitBar.IsKeyHeld();
    }
}