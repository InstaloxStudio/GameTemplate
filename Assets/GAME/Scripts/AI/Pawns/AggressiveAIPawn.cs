using UnityEngine;
using UnityEngine.AI;

public class AggressiveAIPawn : AIPawn
{
    public Transform player;  // Reference to the player
    public float detectionRadius = 10f;  // Radius in which the pawn can detect the player
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

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRadius;
    }
}
