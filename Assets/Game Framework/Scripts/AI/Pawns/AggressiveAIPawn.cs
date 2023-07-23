using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AggressiveAIPawn : AIPawn
{
    public TMPro.TextMeshPro stateText;  // Reference to the text that displays the current state
    public Transform player;  // Reference to the player
    public float detectionRadius = 100f;  // Radius in which the pawn can detect the player



    protected override void Start()
    {
        base.Start();

        //create text object and position it above the pawn
        stateText = GetComponentInChildren<TMPro.TextMeshPro>();


        AIController = new AggressiveAIController(this);

        //if player is null find any pawn
        if (player == null)
        {
            player = FindObjectOfType<Pawn>().transform;
        }
    }

    public void MoveToPlayer()
    {

        var locationNearPlayer = Random.insideUnitSphere * 5f + player.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(locationNearPlayer, out hit, 5f, NavMesh.AllAreas);
        agent.destination = hit.position;


    }

    public void Shoot()
    {
        //spawn bullet
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = transform.position + transform.forward + transform.up;
        bullet.transform.forward = transform.forward;
        bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        bullet.AddComponent<Rigidbody>();
        bullet.AddComponent<Bullet>();
        bullet.GetComponent<Bullet>().speed = 30f;
        bullet.GetComponent<Bullet>().bounces = 0;
        bullet.GetComponent<Bullet>().damage = 10f;


    }

    public void AimAtPlayer()
    {
        //turn to look at player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        if (IsFacingPlayer())
            ChangeState(new ChaseState());
    }

    public bool IsFacingPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, direction) < 5f;
    }

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRadius;
    }
}
