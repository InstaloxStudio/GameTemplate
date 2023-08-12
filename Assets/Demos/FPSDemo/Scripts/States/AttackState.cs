using UnityEngine;

public class AttackState : IAIState<AggressiveAIPawn>
{
    public float attackTime = 1f;
    private float attackTimer = 0f;

    public void Enter(AggressiveAIPawn pawn)
    {
        attackTimer = attackTime;
        pawn.stateText.text = "attack";
        pawn.StopAgent();

    }

    public void Execute(AggressiveAIPawn pawn)
    {
        if (pawn.player == null)
        {
            return;
        }
        //check if we can see the player
        if (pawn.perceptionComponent.IsDetectedBySight(pawn.player))
        {
            pawn.AimAtPlayer();
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
            //if we can't see the player, go back to chasing the player
            pawn.ChangeState(new ChaseState());
        }
    }

    public void Exit(AggressiveAIPawn pawn)
    {
    }
}
