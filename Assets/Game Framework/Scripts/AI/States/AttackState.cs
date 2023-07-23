using UnityEngine;

public class AttackState : IAIState
{
    public float attackTime = 1f;
    private float attackTimer = 0f;

    public void Enter(AIPawn pawn)
    {
        attackTimer = attackTime;
        ((AggressiveAIPawn)pawn).stateText.text = "attack";

    }

    public void Execute(AIPawn pawn)
    {
        if (((AggressiveAIPawn)pawn).IsPlayerInRange())
        {
            //check if we are facing player
            if (((AggressiveAIPawn)pawn).IsFacingPlayer())
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    //shoot bullet at player
                    ((AggressiveAIPawn)pawn).Shoot();
                    attackTimer = attackTime;
                }
            }
            else
            {
                pawn.ChangeState(new AimAtPlayerState());
            }

        }
        else
        {
            pawn.ChangeState(new ChaseState());
        }
    }

    public void Exit(AIPawn pawn)
    {
    }
}
