public class AimAtPlayerState : IAIState<AggressiveAIPawn>
{
    public void Enter(AggressiveAIPawn pawn)
    {
        pawn.stateText.text = "aiming";

    }

    public void Execute(AggressiveAIPawn pawn)
    {
        pawn.AimAtPlayer();

    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}
