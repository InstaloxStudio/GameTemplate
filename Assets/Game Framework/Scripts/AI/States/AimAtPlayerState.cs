public class AimAtPlayerState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        ((AggressiveAIPawn)pawn).stateText.text = "aiming";

    }

    public void Execute(AIPawn pawn)
    {
        ((AggressiveAIPawn)pawn).AimAtPlayer();

    }

    public void Exit(AIPawn pawn)
    {
    }
}
