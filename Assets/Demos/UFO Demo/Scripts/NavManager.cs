using Unity.AI.Navigation;
using UnityEngine;


//singleton to handle navigation generation
public class NavManager : MonoBehaviour
{
    public NavMeshSurface surface;
    public static NavManager instance;

    public bool generateOnStart = true;
    void Awake()
    {
        instance = this;

        if (generateOnStart)
        {
            GenerateNavMesh();
        }
    }

    public void GenerateNavMesh()
    {
        surface.BuildNavMesh();
    }
}