public class IdleState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        if (pawn is FriendlyAIPawn)
            pawn.StopAgent();

        if (pawn is AggressiveAIPawn)
        {
            pawn.StopAgent();
        }
    }

    public void Execute(AIPawn pawn)
    {
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
