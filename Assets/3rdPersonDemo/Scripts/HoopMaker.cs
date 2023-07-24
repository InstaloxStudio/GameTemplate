using UnityEngine;

public class HoopMaker : MonoBehaviour
{
    public GameObject RimPrefab;
    public GameObject GoalPrefab;

    public int numRims = 1;
    public float rimRadius = 1f;
    public float rimOffset = 0f;

    void Start()
    {
        CreateHoop(rimRadius, numRims, RimPrefab, transform.position, rimOffset);
    }

    public void CreateHoop(float radius, int numObjects, GameObject prefab, Vector3 center, float offset)
    {
        for (int i = 0; i < numObjects; i++)
        {
            // Calculate angle around circle
            float angle = i * Mathf.PI * 2f / numObjects;

            // Calculate position around circle
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (radius + offset);

            // Offset position
            position += center;

            // Instantiate prefab at this position
            var hoop = Instantiate(prefab, position, Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0));
            hoop.transform.parent = transform;
        }

        // Create goal
        if (GoalPrefab == null)
            return;

        var goal = Instantiate(GoalPrefab, center, Quaternion.identity);
        goal.transform.parent = transform;
        goal.transform.localScale = new Vector3(radius * 2f, .2f, radius * 2f);
    }

    void OnDrawGizmosSelected()
    {
        //draw a debug sphere at the center of the hoop
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, rimRadius);

        //draw debug cubes at the positions of the rims
        for (int i = 0; i < numRims; i++)
        {
            // Calculate angle around circle
            float angle = i * Mathf.PI * 2f / numRims;

            // Calculate position around circle
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * (rimRadius + rimOffset);

            // Offset position
            position += transform.position;

            //draw the debug cube
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(position, Vector3.one);
        }
    }

}