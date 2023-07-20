using UnityEngine;
using UnityEngine.AI;
public abstract class AIPawn : MonoBehaviour
{
    protected AIController controller;

    private IAIState currentState;

    public AIController AIController
    {
        get => controller;
        set
        {
            if (controller != null)
            {
                controller.Cleanup();
            }
            controller = value;
            if (controller != null)
            {
                controller.Initialize();
            }
        }
    }
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        AIController = new AIController(this);
        AIController.Initialize();
    }

    private void Update()
    {
        AIController.Update();
        currentState?.Execute(this);
    }

    public void ChangeState(IAIState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    public virtual void StopAgent()
    {
        agent.isStopped = true;
    }

    public virtual void ResumeAgent()
    {
        agent.isStopped = false;
    }

    public virtual void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
    }

    public virtual bool HasReachedDestination()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public virtual bool IsAgentMoving()
    {
        return agent.velocity.magnitude > 0;
    }

    public virtual void SetSpeed(float speed)
    {
        agent.speed = speed;
    }

    public virtual void SetAngularSpeed(float angularSpeed)
    {
        agent.angularSpeed = angularSpeed;
    }

    public virtual void SetStoppingDistance(float stoppingDistance)
    {
        agent.stoppingDistance = stoppingDistance;
    }

    public virtual void SetAcceleration(float acceleration)
    {
        agent.acceleration = acceleration;
    }

    public virtual void SetAutoBraking(bool autoBraking)
    {
        agent.autoBraking = autoBraking;
    }

    public virtual void SetAutoRepath(bool autoRepath)
    {
        agent.autoRepath = autoRepath;
    }

    public virtual void SetAreaMask(int areaMask)
    {
        agent.areaMask = areaMask;
    }

    public virtual void SetAvoidancePriority(int avoidancePriority)
    {
        agent.avoidancePriority = avoidancePriority;
    }

    public virtual void SetObstacleAvoidanceType(ObstacleAvoidanceType obstacleAvoidanceType)
    {
        agent.obstacleAvoidanceType = obstacleAvoidanceType;
    }

    public virtual void SetAutoTraverseOffMeshLink(bool autoTraverseOffMeshLink)
    {
        agent.autoTraverseOffMeshLink = autoTraverseOffMeshLink;
    }

    public virtual void SetRadius(float radius)
    {
        agent.radius = radius;
    }

    public virtual void SetHeight(float height)
    {
        agent.height = height;
    }

    public virtual void SetBaseOffset(float baseOffset)
    {
        agent.baseOffset = baseOffset;
    }

}
