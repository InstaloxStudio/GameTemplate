using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AggressiveAIPawn : AIPawn<AggressiveAIPawn>
{
    public TMPro.TextMeshPro stateText;  // Reference to the text that displays the current state
    public Transform player;  // Reference to the player
    public float detectionRadius = 100f;  // Radius in which the pawn can detect the player

    public float attackRange = 50f;

    public PerceptionComponent perceptionComponent;
    protected override void Start()
    {

        //create text object and position it above the pawn
        stateText = GetComponentInChildren<TMPro.TextMeshPro>();


        AIController = new AggressiveAIController(this);
        AIController.Initialize();

        perceptionComponent = GetComponent<PerceptionComponent>();
        perceptionComponent.OnDetected += HandleDetected;

        //if player is null find any pawn
        // if (player == null)
        //{
        // player = FindObjectOfType<Pawn>().transform;
        //}
    }

    private void HandleDetected(IDetectable detectedEntity)
    {
        // Assuming the player implements IDetectable
        if (detectedEntity is Pawn)
        {
            //get transform of detected entity
            var detectedPawn = detectedEntity as Pawn;
            var detectedPawnTransform = detectedPawn.transform;
            this.stateText.text = "detected";
            player = detectedPawnTransform;

        }
    }


    public void MoveToPlayer()
    {

        var locationNearPlayer = Random.insideUnitSphere * 2f + player.position;
        NavMesh.SamplePosition(locationNearPlayer, out NavMeshHit hit, 2f, NavMesh.AllAreas);
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
        var bulletComponent = bullet.AddComponent<Bullet>();
        bulletComponent.speed = 30f;
        bulletComponent.bounces = 0;
        bulletComponent.damage = 10f;
    }

    public void AimAtPlayer()
    {
        //turn to look at player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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

    public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    public bool IsPlayerInSight()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + transform.up + transform.forward * 1.5f, direction, out RaycastHit hit, detectionRadius))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }
    private void OnDestroy()
    {
        if (perceptionComponent != null)
        {
            perceptionComponent.OnDetected -= HandleDetected;
        }
    }


}
