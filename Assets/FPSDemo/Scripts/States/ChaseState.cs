public class ChaseState : IAIState<AggressiveAIPawn>
{
    public void Enter(AggressiveAIPawn pawn)
    {
        pawn.ResumeAgent();
        pawn.stateText.text = "chase";

    }

    public void Execute(AggressiveAIPawn pawn)
    {

        //set destination to player  
        pawn.MoveToPlayer();
        if (pawn.IsPlayerInAttackRange())
        {
            pawn.ChangeState(new AttackState());
        }
    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}