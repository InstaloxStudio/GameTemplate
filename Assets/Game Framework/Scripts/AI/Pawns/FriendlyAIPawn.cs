using UnityEngine;
using UnityEngine.AI;

public class FriendlyAIPawn : AIPawn<FriendlyAIPawn>
{
    public Transform player; // Reference to the player's transform
    public float followDistance = 5f; // Distance at which the pawn starts following the player


    protected override void Start()
    {
        AIController = new FriendlyAIController(this);
        AIController.Initialize();
    }

    public void FollowPlayer()
    {
        agent.destination = player.position;
    }

    public void StopFollowingPlayer()
    {
        agent.destination = transform.position;
    }

    public bool IsPlayerTooFar()
    {
        return Vector3.Distance(transform.position, player.position) > followDistance;
    }
}
