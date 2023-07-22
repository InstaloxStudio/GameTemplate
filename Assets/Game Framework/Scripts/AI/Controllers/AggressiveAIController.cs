using UnityEngine;

public class AggressiveAIController : AIController
{
    public AggressiveAIController(AIPawn pawn) : base(pawn)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        Pawn.ChangeState(new IdleState());
    }

    public override void Update()
    {
        base.Update();
        AggressiveAIPawn aggressivePawn = (AggressiveAIPawn)Pawn;
        if (aggressivePawn.IsPlayerInRange())
        {
            aggressivePawn.ChangeState(new AttackState());
        }
        else
        {
            aggressivePawn.ChangeState(new IdleState());
        }
    }
}
