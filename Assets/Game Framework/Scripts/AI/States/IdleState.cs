public class IdleState : IAIState<AggressiveAIPawn>
{
    public void Enter(AggressiveAIPawn pawn)
    {
        //set state text
        pawn.stateText.text = "Idle";
        pawn.StopAgent();
    }

    public void Execute(AggressiveAIPawn pawn)
    {
        if (pawn.IsPlayerInRange())
        {
            pawn.ChangeState(new ChaseState());
        }
    }

    public void Exit(AggressiveAIPawn pawn)
    {
        pawn.ResumeAgent();
    }
}
