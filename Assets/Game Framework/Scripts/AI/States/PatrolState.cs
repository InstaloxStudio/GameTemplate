public class PatrolState : IAIState
{
    public void Enter(AIPawn pawn)
    {
        ((PatrollingAIPawn)pawn).MoveToNextWaypoint();
    }

    public void Execute(AIPawn pawn)
    {
        if (pawn is PatrollingAIPawn patrolPawn)
        {
            if (patrolPawn.Agent.remainingDistance <= patrolPawn.Agent.stoppingDistance)
            {
                patrolPawn.MoveToNextWaypoint();
            }
        }
    }

    public void Exit(AIPawn pawn)
    {
    }
}
