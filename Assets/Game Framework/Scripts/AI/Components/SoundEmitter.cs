using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundEmitter : MonoBehaviour, ISoundEmitter
{
    [SerializeField]
    private float noiseLevel = 1.0f;

    public float GetNoiseLevel()
    {
        return noiseLevel;
    }

    public void EmitNoise(float level)
    {
        noiseLevel = level;
    }
}
