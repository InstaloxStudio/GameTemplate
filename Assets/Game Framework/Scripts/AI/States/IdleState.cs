public class IdleState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        if (pawn is FriendlyAIPawn)
            pawn.StopAgent();

        if (pawn is AggressiveAIPawn)
        {
            //set state text
            ((AggressiveAIPawn)pawn).stateText.text = "Idle";

            pawn.StopAgent();
        }
    }

    public void Execute(AIPawn pawn)
    {
        if (pawn is AggressiveAIPawn)
        {
            if (((AggressiveAIPawn)pawn).IsPlayerInRange())
            {
                pawn.ChangeState(new AimAtPlayerState());
            }
        }
    }

    public void Exit(AIPawn pawn)
    {
        if (pawn is FriendlyAIPawn)
            pawn.ResumeAgent();

        if (pawn is AggressiveAIPawn)
        {
            pawn.ResumeAgent();
        }
    }
}
