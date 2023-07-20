using UnityEngine;

public class PatrollingAIController : AIController
{
    public PatrollingAIController(AIPawn pawn) : base(pawn)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Pawn.ChangeState(new PatrolState());
    }
}
