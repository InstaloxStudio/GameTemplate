public class PatrolState : IAIState<PatrollingAIPawn>
{
    public void Enter(PatrollingAIPawn pawn)
    {
        pawn.MoveToNextWaypoint();
    }

    public void Execute(PatrollingAIPawn pawn)
    {

        if (pawn.Agent.remainingDistance <= pawn.Agent.stoppingDistance)
        {
            pawn.MoveToNextWaypoint();
        }

    }

    public void Exit(PatrollingAIPawn pawn)
    {
    }
}
