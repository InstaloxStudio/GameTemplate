using UnityEngine;
using UnityEngine.Events;
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

    public bool autoSpawnPawn = true;
    private Pawn defaultCharacter;

    public Action<Pawn> OnCharacterSpawned;
    public UnityEvent OnBeginPlay;


    //list of all pawns in the scene
    public List<Pawn> Pawns = new List<Pawn>();

    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    public virtual void Awake()
    {
        Instance = this;
        Initialize();
        GatherPawns();
        GatherSpawnPoints();
        InitPlayerController();
        InitPawn();
        BeginPlay();
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
    //called before pawns or playercontrollers are spawned
    public virtual void Initialize()
    {

    }

    public virtual void GatherSpawnPoints()
    {
        //gather all spawn points in the scene
        spawnPoints.AddRange(FindObjectsOfType<SpawnPoint>());
    }

    public virtual void GatherPawns()
    {
        //gather all pawns in the scene
        Pawns.AddRange(FindObjectsOfType<Pawn>());
    }

    public virtual Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            return Vector3.zero;
        }
        //get a random spawn point from the list of spawn points
        int randomSpawnPoint = UnityEngine.Random.Range(0, spawnPoints.Count);
        return spawnPoints[randomSpawnPoint].transform.position;
    }

    public virtual void InitPlayerController()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            GameObject playerControllerObject = new GameObject("PlayerController");
            playerController = playerControllerObject.AddComponent<PlayerController>();
        }
    }

    public virtual void InitPawn()
    {
        Vector3 spawnPosition = GetRandomSpawnPoint();
        if (autoSpawnPawn)
        {
            //spawn in the default pawn
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
    }

    //called after pawns and playercontrollers are spawned
    public virtual void BeginPlay()
    {
        if (OnBeginPlay != null)
        {
            OnBeginPlay.Invoke();
        }
    }

}