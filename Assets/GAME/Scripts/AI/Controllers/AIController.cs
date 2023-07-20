using UnityEngine;

public class AIController
{
    protected AIPawn pawn;

    public AIPawn Pawn { get { return pawn; } set { pawn = value; } }

    public AIController(AIPawn pawn)
    {
        this.pawn = pawn;
    }
    public virtual void Cleanup() { }

    public virtual void Initialize() { }

    public virtual void Update() { }
}
