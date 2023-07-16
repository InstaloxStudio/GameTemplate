using UnityEngine;

//singleton GameMode Class

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    public PlayerController playerController;
    public static PlayerController PlayerController { get { return Instance.playerController; } }

    public GameObject characterPrefab;
    private PlayerCharacter defaultCharacter;

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

        if (characterPrefab == null)
        {
            Debug.LogError("No default character prefab set in GameMode.");
        }

        // Spawn the default character
        defaultCharacter = SpawnCharacter(characterPrefab, spawnPosition);

        // Now possess the character
        //playerController.PossessCharacter(defaultCharacter);
    }

    public PlayerCharacter SpawnCharacter(GameObject prefab, Vector3 spawnPosition)
    {
        GameObject characterObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        PlayerCharacter newCharacter = characterObject.GetComponent<PlayerCharacter>();
        playerController.PossessCharacter(newCharacter);
        return newCharacter;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

}