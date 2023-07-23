public class ChaseState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        pawn.ResumeAgent();
        ((AggressiveAIPawn)pawn).stateText.text = "chase";

    }

    public void Execute(AIPawn pawn)
    {

        if (pawn is AggressiveAIPawn)
        {
            //set destination to player        {
            ((AggressiveAIPawn)pawn).MoveToPlayer();

            if (((AggressiveAIPawn)pawn).IsPlayerInRange())
            {
                pawn.ChangeState(new AttackState());
            }
        }
    }

    public void Exit(AIPawn pawn)
    {
        pawn.StopAgent();
    }
}