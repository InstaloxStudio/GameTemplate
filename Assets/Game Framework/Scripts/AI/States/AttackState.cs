using UnityEngine;

public class AttackState : IAIState<AggressiveAIPawn>
{
    public float attackTime = 1f;
    private float attackTimer = 0f;
    public void Enter(AggressiveAIPawn pawn)
    {
        attackTimer = attackTime;
        pawn.stateText.text = "attack";

    }

    public void Execute(AggressiveAIPawn pawn)
    {
        if (pawn.IsPlayerInRange())
        {
            if (pawn.IsPlayerInAttackRange())
            {
                if (pawn.IsFacingPlayer())
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        //shoot bullet at player
                        pawn.Shoot();
                        attackTimer = attackTime;
                    }
                }
                else
                {
                    pawn.AimAtPlayer();
                }
            }
        }
        else
        {
            pawn.ChangeState(new ChaseState());
        }
    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}
