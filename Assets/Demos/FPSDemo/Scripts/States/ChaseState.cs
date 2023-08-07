public class ChaseState : IAIState<AggressiveAIPawn>
{
    public void Enter(AggressiveAIPawn pawn)
    {
        pawn.ResumeAgent();
        pawn.stateText.text = "chase";

        pawn.SetDestination(pawn.player.position);
    }

    public void Execute(AggressiveAIPawn pawn)
    {
        // if (pawn.IsPlayerInAttackRange() && pawn.IsPlayerInSight())
        //{
        //pawn.ChangeState(new AttackState());
        // }
        //set destination to player  
        //pawn.MoveToPlayer();
        //if player is in attack range change state to attack
        if (pawn.IsPlayerInAttackRange())
        {
            pawn.StopAgent();
            pawn.ChangeState(new AttackState());
        }
    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}