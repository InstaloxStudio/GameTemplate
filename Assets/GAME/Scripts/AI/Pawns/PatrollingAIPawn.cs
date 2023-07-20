using UnityEngine;
using UnityEngine.AI;

public class PatrollingAIPawn : AIPawn
{
    public Transform[] waypoints;  // Array of waypoints
    [SerializeField]
    private int currentWaypointIndex = 0;  // The waypoint the pawn is currently moving towards

    protected override void Start()
    {
        base.Start();
        AIController = new PatrollingAIController(this);
    }

    public void MoveToNextWaypoint()
    {
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}
