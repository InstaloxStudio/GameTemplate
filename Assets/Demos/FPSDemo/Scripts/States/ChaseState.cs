using System;
using UnityEngine;

public class ChaseState : IAIState<AggressiveAIPawn>
{
    public void Enter(AggressiveAIPawn pawn)
    {
        pawn.ResumeAgent();
        pawn.stateText.text = "chase";

        pawn.SetDestination(pawn.player.position);
        pawn.lastSeenDirection = (pawn.player.position - pawn.playerLastFrame).normalized;
        pawn.playerLastFrame = pawn.player.position;
    }

    public void Execute(AggressiveAIPawn pawn)
    {
        var distance = Vector3.Distance(pawn.transform.position, pawn.player.position);
        //if we can see the player, chase them until we get in attack range
        if (pawn.perceptionComponent.IsDetectedBySight(pawn.player))
        {
            pawn.MoveToPlayer();
            if (distance <= pawn.attackRange)
            {
                pawn.ChangeState(new AttackState());
            }
        }
        else if (pawn.perceptionComponent.GetMemoryOf(pawn.player) is PerceptionComponent.MemoryRecord record)
        {
            pawn.SetDestination(record.position);
            pawn.ChangeState(new InvestigateState());
        }
        pawn.playerLastFrame = pawn.player.position;

    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}
