using System.Collections.Generic;
using System;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour, INotifiable
{
    [Header("Sight Parameters")]
    public float sightRange = 10f;
    public float fieldOfViewAngle = 110f;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Hearing Parameters")]
    public float hearingRange = 5f;
    public LayerMask soundEmitterMask;
    public float minimumHearingThreshold;

    private List<IDetectable> detectedEntities = new();
    // Event declaration
    public event Action<IDetectable> OnDetected;
    private void Update()
    {
        detectedEntities.Clear(); // Clear the previous frame's detected entities.

        DetectEntitiesBySight();
        DetectEntitiesByHearing();
        NotifyDetectedEntities();
    }

    private void DetectEntitiesBySight()
    {
        Collider[] entitiesInViewRadius = Physics.OverlapSphere(transform.position, sightRange, targetMask);

        for (int i = 0; i < entitiesInViewRadius.Length; i++)
        {
            Transform entity = entitiesInViewRadius[i].transform;
            IDetectable detectableEntity = entity.GetComponent<IDetectable>();

            if (detectableEntity != null)
            {
                Vector3 directionToEntity = (entity.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToEntity) < fieldOfViewAngle / 2f)
                {
                    float distanceToEntity = Vector3.Distance(transform.position, entity.position);

                    if (!Physics.Raycast(transform.position, directionToEntity, distanceToEntity, obstructionMask))
                    {
                        detectedEntities.Add(detectableEntity);
                    }
                }
            }
        }
    }

    private void DetectEntitiesByHearing()
    {
        Collider[] heardColliders = Physics.OverlapSphere(transform.position, hearingRange, soundEmitterMask);

        foreach (var col in heardColliders)
        {
            ISoundEmitter emitter = col.GetComponent<ISoundEmitter>();
            if (emitter != null)
            {
                float distanceToEmitter = Vector3.Distance(transform.position, col.transform.position);
                float effectiveNoiseLevel = emitter.GetNoiseLevel() / (1 + distanceToEmitter); // Simple attenuation

                if (effectiveNoiseLevel > minimumHearingThreshold)
                {
                    var detectable = col.GetComponent<IDetectable>();
                    if (detectable != null && !detectedEntities.Contains(detectable))
                    {
                        detectedEntities.Add(detectable);
                    }
                }
            }
        }
    }

    private void NotifyDetectedEntities()
    {
        foreach (var entity in detectedEntities)
        {
            entity.Detected(this.gameObject);
            OnDetected?.Invoke(entity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 fieldOfViewLine1 = Quaternion.AngleAxis(fieldOfViewAngle / 2f, Vector3.up) * transform.forward * sightRange;
        Vector3 fieldOfViewLine2 = Quaternion.AngleAxis(-fieldOfViewAngle / 2f, Vector3.up) * transform.forward * sightRange;

        Gizmos.DrawRay(transform.position, fieldOfViewLine1);
        Gizmos.DrawRay(transform.position, fieldOfViewLine2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }

    public void Notify(GameObject entity)
    {
        // Handle being notified (in case this component also implements INotifiable)
    }
}



public interface INotifiable
{
    void Notify(GameObject entity);
}

public interface ISoundEmitter
{
    float GetNoiseLevel();
}
public interface IDetectable
{
    void Detected(GameObject detector);
}