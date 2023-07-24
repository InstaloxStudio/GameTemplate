using UnityEngine;

public class AggressiveAIController : AIController<AggressiveAIPawn>
{
    bool isPlayerInRange = false;
    bool IsPlayerInAttackRange = false;

    public AggressiveAIController(AggressiveAIPawn pawn) : base(pawn)
    {
    }
    public override void Initialize()
    {
        base.Initialize();
        pawn.ChangeState(new IdleState());
    }

    public override void Update()
    {
        base.Update();
        IsPlayerInAttackRange = pawn.IsPlayerInAttackRange();
        isPlayerInRange = pawn.IsPlayerInRange();
    }
}
