using UnityEngine;

public class AggressiveAIController : AIController
{
    bool isPlayerInRange = false;
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
        isPlayerInRange = aggressivePawn.IsPlayerInRange();

    }
}
