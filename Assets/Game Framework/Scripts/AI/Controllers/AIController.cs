using UnityEngine;

public abstract class AIController<T> where T : AIPawn<T>
{
    protected T pawn;

    public T Pawn { get { return pawn; } set { pawn = value; } }

    public AIController(T pawn)
    {
        this.pawn = pawn;
    }
    public virtual void Cleanup() { }

    public virtual void Initialize() { }

    public virtual void Update() { }
}
