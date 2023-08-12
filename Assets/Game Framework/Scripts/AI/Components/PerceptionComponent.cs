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
    public Transform eyeTransform;

    [Header("Hearing Parameters")]
    public float hearingRange = 5f;
    public LayerMask soundEmitterMask;
    public float minimumHearingThreshold;
    [Header("Memory Parameters")]
    public float memoryDecayRate = 0.1f;
    public float forgetThreshold = 0.01f;
    private Dictionary<IDetectable, MemoryRecord> memory = new();
    private List<IDetectable> detectedEntities = new();
    private List<IDetectable> detectedBySightEntities = new();
    private List<IDetectable> detectedByHearingEntities = new();

    // Event declaration
    public event Action<IDetectable> OnDetected;
    public event Action<IDetectable> OnSeen;
    public event Action<IDetectable> OnHeard;

    private void Update()
    {
        detectedEntities.Clear(); // Clear the previous frame's detected entities.
        detectedByHearingEntities.Clear(); // Clear the previous frame's detected entities.
        detectedBySightEntities.Clear();
        DetectEntitiesBySight();
        DetectEntitiesByHearing();
        detectedEntities.AddRange(detectedBySightEntities);
        detectedEntities.AddRange(detectedByHearingEntities);
        NotifyDetectedEntities();
        UpdateMemory();
    }

    private void UpdateMemory()
    {
        // Update memory with current detections
        foreach (var entity in detectedEntities)
        {
            memory[entity] = new MemoryRecord
            {
                position = entity.GetPosition(),
                timestamp = Time.time,
                strength = 1f
            };
        }

        // Decay and potentially forget old memories
        List<IDetectable> toForget = new();
        foreach (var entry in memory)
        {
            var record = entry.Value;
            float elapsed = Time.time - record.timestamp;
            record.strength *= Mathf.Exp(-elapsed * memoryDecayRate);

            if (record.strength < forgetThreshold)
            {
                toForget.Add(entry.Key);
            }
        }

        foreach (var key in toForget)
        {
            memory.Remove(key);
        }
    }

    private void DetectEntitiesBySight()
    {
        Collider[] entitiesInViewRadius = Physics.OverlapSphere(eyeTransform.position, sightRange, targetMask);

        for (int i = 0; i < entitiesInViewRadius.Length; i++)
        {
            Transform entity = entitiesInViewRadius[i].transform;
            IDetectable detectableEntity = entity.GetComponent<IDetectable>();

            if (detectableEntity != null)
            {
                Vector3 directionToEntity = (entity.position - eyeTransform.position).normalized;

                if (Vector3.Angle(eyeTransform.forward, directionToEntity) < fieldOfViewAngle / 2f)
                {
                    float distanceToEntity = Vector3.Distance(eyeTransform.position, entity.position);

                    if (!Physics.Raycast(eyeTransform.position, directionToEntity, distanceToEntity, obstructionMask))
                    {
                        detectedBySightEntities.Add(detectableEntity);
                    }
                }
            }
        }
    }


    public List<IDetectable> GetDetectedEntities()
    {
        return detectedEntities;
    }

    public List<IDetectable> GetDetectedEntitiesByType<T>() where T : IDetectable
    {
        List<IDetectable> detectedEntitiesByType = new();
        foreach (var entity in detectedEntities)
        {
            if (entity is T)
            {
                detectedEntitiesByType.Add(entity);
            }
        }
        return detectedEntitiesByType;
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
                        detectedByHearingEntities.Add(detectable);
                    }
                }
            }
        }
    }

    private void NotifyDetectedEntities()
    {
        foreach (var entity in detectedByHearingEntities)
        {
            entity.Detected(this.gameObject);
            OnDetected?.Invoke(entity);
            if (entity is INotifiable)
            {
                (entity as INotifiable).Notify(this.gameObject);
            }
        }

        foreach (var entity in detectedBySightEntities)
        {
            entity.Detected(this.gameObject);
            OnDetected?.Invoke(entity);

            if (entity is INotifiable)
            {
                (entity as INotifiable).Notify(this.gameObject);
            }
        }

        foreach (var entity in detectedEntities)
        {
            entity.Detected(this.gameObject);
            OnDetected?.Invoke(entity);
        }
    }

    public bool IsDetectedBySight(IDetectable entity)
    {
        return detectedBySightEntities.Contains(entity);
    }

    public bool IsDetectedBySight(GameObject entity)
    {
        return detectedBySightEntities.Contains(entity.GetComponent<IDetectable>());
    }

    public bool IsDetectedBySight(Transform entity)
    {
        return detectedBySightEntities.Contains(entity.GetComponent<IDetectable>());
    }

    public bool IsDetectedByHearing(IDetectable entity)
    {
        return detectedByHearingEntities.Contains(entity);
    }

    public bool IsDetectedByHearing(GameObject entity)
    {
        return detectedByHearingEntities.Contains(entity.GetComponent<IDetectable>());
    }

    public bool IsDetectedByHearing(Transform entity)
    {
        return detectedByHearingEntities.Contains(entity.GetComponent<IDetectable>());
    }

    public bool IsDetected(IDetectable entity)
    {
        return detectedEntities.Contains(entity);
    }

    public List<IDetectable> GetDetectedBySightEntities()
    {
        return detectedBySightEntities;
    }

    public List<IDetectable> GetDetectedByHearingEntities()
    {
        return detectedByHearingEntities;
    }

    public List<IDetectable> GetDetectables()
    {
        return detectedEntities;
    }



    public struct MemoryRecord
    {
        public Vector3 position;
        public float timestamp;
        public float strength;
    }

    public MemoryRecord? GetMemoryOf(IDetectable entity)
    {
        if (memory.TryGetValue(entity, out var record))
        {
            return record;
        }

        return null;
    }

    public MemoryRecord? GetMemoryOf(Transform entity)
    {
        if (memory.TryGetValue(entity.GetComponent<IDetectable>(), out var record))
        {
            return record;
        }

        return null;
    }

    public MemoryRecord? GetMemoryOf(GameObject entity)
    {
        if (memory.TryGetValue(entity.GetComponent<IDetectable>(), out var record))
        {
            return record;
        }

        return null;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyeTransform.position, sightRange);

        Vector3 fieldOfViewLine1 = Quaternion.AngleAxis(fieldOfViewAngle / 2f, Vector3.up) * eyeTransform.forward * sightRange;
        Vector3 fieldOfViewLine2 = Quaternion.AngleAxis(-fieldOfViewAngle / 2f, Vector3.up) * eyeTransform.forward * sightRange;

        Gizmos.DrawRay(eyeTransform.position, fieldOfViewLine1);
        Gizmos.DrawRay(eyeTransform.position, fieldOfViewLine2);

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
    Vector3 GetPosition();
    void Detected(GameObject detector);
    void OnSeen(GameObject detector);
    void OnHeard(GameObject detector);

}