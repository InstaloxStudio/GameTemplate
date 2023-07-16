using UnityEngine;

//singleton GameMode Class

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    public PlayerController playerController;
    public static PlayerController PlayerController { get { return Instance.playerController; } }

    public Pawn defaultPawn;
    private Pawn defaultCharacter;

    void Awake()
    {
        Instance = this;

        // Ensure PlayerController is assigned first
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            GameObject playerControllerObject = new GameObject("PlayerController");
            playerController = playerControllerObject.AddComponent<PlayerController>();
        }

        Vector3 spawnPosition = Vector3.zero;

        // Find any spawn points in the scene
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        // If any spawnpoints are found choose a random one to spawn at
        if (spawnPoints.Length > 0)
        {
            int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            spawnPosition = spawnPoints[randomSpawnPoint].transform.position;
        }
        else
        {
            // If no spawn points are found, spawn at the origin
            Debug.Log("No spawn points found. Spawning at origin.");
            spawnPosition = Vector3.zero;
        }

        if (defaultPawn == null)
        {
            //spawn in the default pawn
            GameObject pawnObject = new GameObject("DefaultPawn");
            defaultPawn = pawnObject.AddComponent<DefaultPawn>();
            GameObject cameraObject = new GameObject("Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            var freecam = cameraObject.AddComponent<FreeCamera>();
            freecam.Cam = camera;
            cameraObject.transform.SetParent(pawnObject.transform);
        }

        // Spawn the default character
        defaultCharacter = SpawnCharacter(defaultPawn.gameObject, spawnPosition);

        // Now possess the character
        //playerController.PossessCharacter(defaultCharacter);
    }

    public Pawn SpawnCharacter(GameObject prefab, Vector3 spawnPosition)
    {
        GameObject characterObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Pawn newCharacter = characterObject.GetComponent<Pawn>();
        playerController.PossessCharacter(newCharacter);
        return newCharacter;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

}