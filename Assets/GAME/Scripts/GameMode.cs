using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// GameMode is a singleton class that manages the game state and contains references to all the pawns in the scene
/// this is where the player controller is created and the default pawn is spawned
/// if no spawn points are found, the default pawn will be spawned at the origin.
/// </summary>
public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    public PlayerController playerController;
    public static PlayerController PlayerController { get { return Instance.playerController; } }

    public Pawn defaultPawn;
    private Pawn defaultCharacter;

    //list of all pawns in the scene
    public List<Pawn> Pawns = new List<Pawn>();

    public virtual void Awake()
    {
        Instance = this;

        //gather all pawns in the scene
        Pawns.AddRange(FindObjectsOfType<Pawn>());


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
            int randomSpawnPoint = UnityEngine.Random.Range(0, spawnPoints.Length);
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
            //load prefab from Prefabs folder
            defaultPawn = Instantiate(Resources.Load<Pawn>("DefaultPawn"), spawnPosition, Quaternion.identity);
            //set the default pawn as the active character
            playerController.Possess(defaultPawn);
        }
        else
        {
            //spawn in the default pawn
            defaultCharacter = SpawnCharacter(defaultPawn.gameObject, spawnPosition);
        }
    }

    public virtual Pawn SpawnCharacter(GameObject prefab, Vector3 spawnPosition)
    {
        GameObject characterObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Pawn newCharacter = characterObject.GetComponent<Pawn>();
        playerController.Possess(newCharacter);
        return newCharacter;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

}