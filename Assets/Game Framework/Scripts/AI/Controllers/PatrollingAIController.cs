using UnityEngine;

public class PatrollingAIController : AIController<PatrollingAIPawn>
{
    public PatrollingAIController(PatrollingAIPawn pawn) : base(pawn)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Pawn.ChangeState(new PatrolState());
    }
}
