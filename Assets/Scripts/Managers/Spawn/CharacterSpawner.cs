using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles instantiating, spawning, despawning and respawning characters.
/// </summary>
public class CharacterSpawner : Singleton<CharacterSpawner> {

    [Tooltip("Transform of the object which will contain all knight objects as children.")]
    [SerializeField]
    private Transform KnightContainer = null;
    [Tooltip("Transform of the object which will contain all samurai objects as children.")]
    [SerializeField]
    private Transform SamuraiContainer = null;

    //Variables counting how many characters should still be respawned. Act as a stack.
    public int KnightsSpawningCount { get; private set; }
    public int SamuraisSpawningCount { get; private set; }

    //Prefabs of the characters to spawn.
    private GameObject KnightPrefab = null;
    private GameObject SamuraiPrefab = null;

    private SpawnTimer mySpawnTimeManager;

    private void Awake()
    {
        mySpawnTimeManager = new SpawnTimer();
        KnightsSpawningCount = 0;
        SamuraisSpawningCount = 0;
        KnightPrefab = (GameObject)Resources.Load("Prefabs/KnightPrefab");
        SamuraiPrefab = (GameObject)Resources.Load("Prefabs/SamuraiPrefab");
        if (KnightPrefab == null || SamuraiPrefab == null)
        {
            Debug.LogError("Could not find Knight or Samurai prefab in Resources folder.");
        }
    }

    private void Start()
    {
        EventManager.Instance.AddPauseGameListener(OnGamePaused);
        EventManager.Instance.AddResumeGameListener(OnGameResumed);
    }

    /// <summary>
    /// Stops respawn when pausing game.
    /// </summary>
    private void OnGamePaused()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Restores respawn coroutines when resuming game.
    /// </summary>
    private void OnGameResumed()
    {
        GameManager myGameManager = GameManager.Instance;
        for(int i = 0; i < KnightsSpawningCount; i++)
        {
            StartCoroutine(RespawnKnightCoroutine(mySpawnTimeManager.RespawnDelayKnight,
                myGameManager.KnightPlayer.TeamList));
        }
        for(int j = 0; j < SamuraisSpawningCount; j++)
        {
            StartCoroutine(RespawnSamuraiCoroutine(mySpawnTimeManager.RespawnDelaySamurai,
                myGameManager.SamuraiPlayer.TeamList));
        }
    }

    /// <summary>
    /// Destroys all characters, resets spawner attributes. 
    /// </summary>
    public void ClearSpawner()
    {
        KnightsSpawningCount = 0;
        SamuraisSpawningCount = 0;
        StopAllCoroutines();
        mySpawnTimeManager.ResetSpawnCounters();
        foreach (Transform knight in KnightContainer)
        {
            Destroy(knight.gameObject);
        }
        foreach(Transform samurai in SamuraiContainer)
        {
            Destroy(samurai.gameObject);
        }
    }

    /// <summary>
    /// Returns a list of all the instantiated knight objects.
    /// </summary>
    /// <returns>List of knight obects.</returns>
    public List<GameObject> GetKnightObjects()
    {
        return GetChildrenList(KnightContainer);
    }

    /// <summary>
    /// Returns a list of all the instantiated samurai objects.
    /// </summary>
    /// <returns>List of samurai objects.</returns>
    public List<GameObject> GetSamuraiObjects()
    {
        return GetChildrenList(SamuraiContainer);
    }

    /// <summary>
    /// Builds and returns the list of children of a given parent Transform.
    /// </summary>
    /// <param name="parent">parent from which the children list should be built.</param>
    /// <returns>List of children Transform.</returns>
    private List<GameObject> GetChildrenList(Transform parent)
    {
        List<GameObject> childrenList = new List<GameObject>();
        foreach(Transform child in parent)
        {
            childrenList.Add(child.gameObject);
        }
        return childrenList;
    }

    #region Character instantiation

    /// <summary>
    /// Instantiates a character of given type.
    /// </summary>
    /// <param name="type">Type of character to instantiate.</param>
    /// <returns>Instantiated character.</returns>
    private GameObject InstantiateCharacter(GameParameters.Team type)
    {
        GameObject instantiatedCharacter = null;
        switch(type)
        {
            case GameParameters.Team.Knight:
                instantiatedCharacter = Instantiate(KnightPrefab, KnightContainer);
                IgnoreFriendlyCollisions(instantiatedCharacter.GetComponent<Collider2D>(),
                    GetKnightObjects());
                break;
            case GameParameters.Team.Samurai:
                instantiatedCharacter = Instantiate(SamuraiPrefab, SamuraiContainer);
                IgnoreFriendlyCollisions(instantiatedCharacter.GetComponent<Collider2D>(),
                    GetSamuraiObjects());
                break;
            default:
                break;
        }
        return instantiatedCharacter;
    }

    /// <summary>
    /// Ignores collisions of a given character collider with colliders of characters of the same team.
    /// </summary>
    /// <param name="instantiatedCollider">instantiated character collider.</param>
    /// <param name="instantiatedTeam">List of objects to avoid in the team.</param>
    private void IgnoreFriendlyCollisions(Collider2D instantiatedCollider, List<GameObject> instantiatedTeam)
    {
        for(int i = 0; i < instantiatedTeam.Count; i++)
        {
            Physics2D.IgnoreCollision(instantiatedCollider, instantiatedTeam[i].GetComponent<Collider2D>());
        }
    }

    /// <summary>
    /// Unit method to instantiate a knight.
    /// </summary>
    /// <returns>Instantiated gameObject.</returns>
    public GameObject InstantiateKnight()
    {
        return InstantiateCharacter(GameParameters.Team.Knight);
        
    }

    /// <summary>
    /// Unit method to instantiate a samurai.
    /// </summary>
    /// <returns>Instantiated gameObject</returns>
    public GameObject InstantiateSamurai()
    {
        return InstantiateCharacter(GameParameters.Team.Samurai);
    }
    #endregion

    #region Character spawn
    /// <summary>
    /// Instantiates a character and adjusts its position around spawn position.
    /// </summary>
    /// <param name="type">Type of character to instantiate.</param>
    /// <param name="isFirstSpawn">True if spawning on game start. False otherwise.</param>
    /// <returns>Instantiated character.</returns>
    private GameObject SpawnCharacter(GameParameters.Team type,
        bool isFirstSpawn = false)
    {
        GameObject instantiatedChar = null;
        switch (type)
        {
            case GameParameters.Team.Knight:
                instantiatedChar = InstantiateKnight();
                break;
            case GameParameters.Team.Samurai:
                instantiatedChar = InstantiateSamurai();
                break;
            default:
                break;
        }
        AdjustSpawnPosition(instantiatedChar.transform, type, isFirstSpawn);
        return instantiatedChar;
    }

    /// <summary>
    /// Unit method to spawn a knight.
    /// </summary>
    /// <param name="isFirstSpawn">True if spawning on game start. False otherwise.</param>
    /// <returns>Instantiated gameObject</returns>
    public GameObject SpawnKnight(bool isFirstSpawn = false)
    {
        return SpawnCharacter(GameParameters.Team.Knight, isFirstSpawn);
    }

    /// <summary>
    /// Unit method to spawn a samurai.
    /// </summary>
    /// <param name="isFirstSpawn">True if spawning on game start. False otherwise.</param>
    /// <returns>Instantiated gameObject</returns>
    public GameObject SpawnSamurai(bool isFisrtSpawn = false)
    {
        return SpawnCharacter(GameParameters.Team.Samurai, isFisrtSpawn);
    }

    /// <summary>
    /// Add offset to spawn position to avoid spawning on another character.
    /// </summary>
    /// <param name="transformToAdjust"></param>
    /// <param name="type">Type of character to adjust.</param>
    /// <param name="isFirstSpawn">True if spawning on game start. False otherwise.</param>
    private void AdjustSpawnPosition(Transform transformToAdjust, GameParameters.Team type, bool isFirstSpawn)
    {
        Vector2 spawnPos = new Vector2();
        Vector2 offsetPos = new Vector2();
        int characterIndex = 0; //Index of the character in spawn field.
        if(isFirstSpawn) //If first spawn, character index is set according to instantiated objects.
        {
            characterIndex = type == GameParameters.Team.Knight ?
                KnightContainer.childCount : SamuraiContainer.childCount;
        }
        else //If not first spawn, character index is set according to amount of currently spawning objects.
        {
            characterIndex = type == GameParameters.Team.Knight ?
                KnightsSpawningCount : SamuraisSpawningCount;
        }
        int ordinateOffsetSign = characterIndex % 2 == 0 ? 1 : -1; //Alternate upper and lower ordinate offset.

        switch (type)
        {
            case GameParameters.Team.Knight:
                spawnPos = GameParameters.KNIGHT_SPAWN_POSITION;
                break;
            case GameParameters.Team.Samurai:
                spawnPos = GameParameters.SAMURAI_SPAWN_POSITION;
                break;
        }
        offsetPos.x = (characterIndex / GameParameters.CHARACTERS_PER_COLUMN);
        offsetPos.y = GameParameters.CHARACTER_SPAWN_OFFSET_Y
            * Mathf.CeilToInt(characterIndex % GameParameters.CHARACTERS_PER_COLUMN * 0.5f)
            * ordinateOffsetSign;
        transformToAdjust.position = spawnPos + offsetPos;
    }
    #endregion

    #region Character despawn
    /// <summary>
    /// Destroys a gameObject and updates death count.
    /// </summary>
    /// <param name="charObject">Object to destroy.</param>
    /// <param name="type">Type of object to destroy.</param>
    private void DespawnCharacter(GameObject charObject, GameParameters.Team type)
    {
        Destroy(charObject);
        switch (type)
        {
            case GameParameters.Team.Knight:
                mySpawnTimeManager.IncreaseDeathCountKnight();
                break;
            case GameParameters.Team.Samurai:
                mySpawnTimeManager.IncreaseDeathCountSamurai();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Unit method to destroy a knight.
    /// </summary>
    /// <param name="knightObject">Knight object to destroy.</param>
    public void DespawnKnight(GameObject knightObject)
    {
        DespawnCharacter(knightObject, GameParameters.Team.Knight);
    }

    /// <summary>
    /// Unit method to destroy a samurai.
    /// </summary>
    /// <param name="samuraiObject">Samurai object to destroy.</param>
    public void DespawnSamurai(GameObject samuraiObject)
    {
        DespawnCharacter(samuraiObject, GameParameters.Team.Samurai);
    }
    #endregion

    #region Character respawn

    /// <summary>
    /// Unit method to respawn a knight.
    /// </summary>
    /// <param name="knightTeamList">List of controllers to which the respawned knight should be added.</param>
    public void RespawnKnight(List<NPCController> knightTeamList)
    {
        RespawnCharacter(GameParameters.Team.Knight, knightTeamList);
    }

    /// <summary>
    /// Unit method to respawn a knight.
    /// </summary>
    /// <param name="samuraiTeamList">List of controllers to which the respawned samurai should be added.</param>
    public void RespawnSamurai(List<NPCController> samuraiTeamList)
    {
        RespawnCharacter(GameParameters.Team.Samurai, samuraiTeamList);
    }

    /// <summary>
    /// Starts the coroutine to respawn a character of given type.
    /// </summary>
    /// <param name="type">Type of character to respawn.</param>
    /// <param name="charTeamList">List of controllers to which the respawned character should be added.</param>
    private void RespawnCharacter(GameParameters.Team type, List<NPCController> charTeamList)
    {
        int respawnDelay = -1;
        switch(type)
        {
            case GameParameters.Team.Knight:
                respawnDelay = mySpawnTimeManager.RespawnDelayKnight;
                KnightsSpawningCount += 1;
                StartCoroutine(RespawnKnightCoroutine(respawnDelay, charTeamList));
                break;
            case GameParameters.Team.Samurai:
                respawnDelay = mySpawnTimeManager.RespawnDelaySamurai;
                SamuraisSpawningCount += 1;
                StartCoroutine(RespawnSamuraiCoroutine(respawnDelay, charTeamList));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Coroutine handling knight respawn.
    /// </summary>
    /// <param name="respawnDelay">Delay before which the knight will be respawned.</param>
    /// <param name="charTeamList">List of controllers to which the respawned knight will be added.</param>
    /// <returns></returns>
    private IEnumerator RespawnKnightCoroutine(int respawnDelay, List<NPCController> charTeamList)
    {
        GameManager myGameManager = GameManager.Instance;
        GameObject instantiatedCharacter = null;
        yield return new WaitForSeconds(respawnDelay);
        instantiatedCharacter = SpawnKnight();
        charTeamList.Add(new KnightController(instantiatedCharacter.GetComponent<CharacterView>(),
            myGameManager.KnightVelocity, myGameManager.KnightPlayer.HasEnemyFlag()));
        KnightsSpawningCount -= 1;
    }

    /// <summary>
    /// Coroutine handling samurai respawn.
    /// </summary>
    /// <param name="respawnDelay">Delay before which the samurai will be respawned.</param>
    /// <param name="charTeamList">List of controllers to which the respawned samurai will be added.</param>
    /// <returns></returns>
    private IEnumerator RespawnSamuraiCoroutine(int respawnDelay, List<NPCController> charTeamList)
    {
        GameManager myGameManager = GameManager.Instance;
        GameObject instantiatedCharacter = null;
        yield return new WaitForSeconds(respawnDelay);
        instantiatedCharacter = SpawnSamurai();
        charTeamList.Add(new SamuraiController(instantiatedCharacter.GetComponent<CharacterView>(),
            myGameManager.SamuraiVelocity, myGameManager.SamuraiPlayer.HasEnemyFlag()));
        SamuraisSpawningCount -= 1;
    }
    #endregion 
}

