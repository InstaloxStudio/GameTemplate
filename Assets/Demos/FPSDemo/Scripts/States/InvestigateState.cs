using UnityEngine;

public class InvestigateState : IAIState<AggressiveAIPawn>
{
    private float wanderTimer = 0f;
    private float wanderDuration = 5f;

    private float wanderRadius = 5f;

    public void Enter(AggressiveAIPawn pawn)
    {
        var memoryRecord = pawn.perceptionComponent.GetMemoryOf(pawn.player);
        if (memoryRecord != null)
        {
            pawn.investigateTarget = memoryRecord.Value.position + pawn.lastSeenDirection * wanderRadius;
            pawn.SetDestination(pawn.investigateTarget);
        }
        pawn.stateText.text = "investigate";
    }

    public void Execute(AggressiveAIPawn pawn)
    {
        if (pawn.perceptionComponent.IsDetectedBySight(pawn.player))
        {
            pawn.ChangeState(new ChaseState());
        }
        else if (pawn.Agent.remainingDistance.Equals(0f))
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderDuration)
            {
                pawn.MoveToRandomSpotWithinRange(wanderRadius);
                wanderTimer = 0f;
            }
        }
    }


    public void Exit(AggressiveAIPawn pawn)
    {

    }
}
