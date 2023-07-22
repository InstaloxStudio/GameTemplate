public class AttackState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        ((AggressiveAIPawn)pawn).AttackPlayer();
    }

    public void Execute(AIPawn pawn)
    {
    }

    public void Exit(AIPawn pawn)
    {
    }
}

public class AimAtPlayerState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        ((AggressiveAIPawn)pawn).AimAtPlayer();
    }

    public void Execute(AIPawn pawn)
    {
    }

    public void Exit(AIPawn pawn)
    {
    }
}
