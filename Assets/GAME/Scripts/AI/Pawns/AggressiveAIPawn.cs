using UnityEngine;
using UnityEngine.AI;

public class AggressiveAIPawn : AIPawn
{
    public Transform player;  // Reference to the player
    public float detectionRadius = 100f;  // Radius in which the pawn can detect the player
    protected override void Start()
    {
        base.Start();
        AIController = new AggressiveAIController(this);

        //if player is null find any pawn
        if (player == null)
        {
            player = FindObjectOfType<Pawn>().transform;
        }
    }

    public void AttackPlayer()
    {
        agent.destination = player.position;
    }

    public void AimAtPlayer()
    {
        //turn to look at player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        //if we are facing the player, shoot
        if (Vector3.Angle(transform.forward, direction) < 10f)
        {
            //enter shooting state
            ChangeState(new AttackState());
        }
    }

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRadius;
    }
}
