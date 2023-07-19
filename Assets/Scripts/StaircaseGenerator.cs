using UnityEngine;

public class StaircaseGenerator : MonoBehaviour
{
    public GameObject stepPrefab; // Assign the Cube prefab here
    public int stepsCount = 10; // The number of steps in the staircase
    public Vector3 stepSize = new Vector3(1, 0.2f, 2); // The size of each step
    public Vector3 stepSpacing = new Vector3(0, 0.2f, 2); // The distance between each step

    void Start()
    {
        GenerateStaircase();
    }

    void GenerateStaircase()
    {
        for (int i = 0; i < stepsCount; i++)
        {
            GameObject step = Instantiate(stepPrefab, transform);
            step.transform.localScale = stepSize;
            step.transform.localPosition = i * stepSpacing;
        }
    }
}
