using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player of the game. Manages its NPC team.
/// </summary>
public abstract class Player {

    public GameParameters.Team Team { get; protected set; }
    public GameParameters.Team Enemy { get; protected set; }
    public List<NPCController> TeamList { get; protected set; }
    public GameObject Flag { get; protected set; }
    public bool IsFlagCapturable { get; protected set; }
    public int Score { get; protected set; }

    protected CharacterSpawner MyCharacterSpawner;
    protected EventForwarder MyEventForwarder;
    protected ScoreKeeper PlayerScoreKeeper;

    public Player(CharacterSpawner gameCharacterSpawner, EventForwarder playerEventForwarder)
    {
        IsFlagCapturable = true;
        TeamList = new List<NPCController>();
        Score = 0;
        MyCharacterSpawner = gameCharacterSpawner;
        MyEventForwarder = playerEventForwarder;
        MyEventForwarder.OnGameStartedEvent += OnGameStarted;
        MyEventForwarder.OnGameEndedEvent += OnGameEnded;
        MyEventForwarder.OnTryCaptureFlagEvent += OnTryCaptureFlag;
    }

    /// <summary>
    /// Prevent player's score from being updated.
    /// </summary>
    public void LockScore()
    {
        PlayerScoreKeeper.LockScore();
    }

    /// <summary>
    /// Find and return the flagbearer in this team.
    /// </summary>
    /// <returns>Transform of the flag bearer if found. Null otherwise.</returns>
    public Transform GetFlagBearer()
    {
        Transform flagBearer = null;
        int i = 0;
        while(i < TeamList.Count && flagBearer == null)
        {
            if(TeamList[i].View.HasFlag)
            {
                flagBearer = TeamList[i].View.transform;
            }
            i++;
        }
        return flagBearer;
    }

    /// <summary>
    /// Checks if a flag bearer exists.
    /// </summary>
    /// <returns>True if a flag bearer is found among the team NPCs. False otherwise.</returns>
    public bool HasEnemyFlag()
    {
        return GetFlagBearer() != null;
    }

    #region Event handlers

    /// <summary>
    /// Handles despawning and respawning a character.
    /// </summary>
    /// <param name="deadCharController">Character to despawn.</param>
    protected abstract void OnDeathEvent(NPCController deadCharController);

    /// <summary>
    /// Handles the own player's flag capture.
    /// </summary>
    /// <param name="captor">Captor which tried to capture player's flag.</param>
    /// <param name="captorType">Type of captor</param>
    protected abstract void OnTryCaptureFlag(CharacterView captor, GameParameters.Team captorType);

    protected abstract void OnFlagReleased();

    protected void OnGameStarted()
    {
        Flag = FlagManager.Instance.SpawnFlag(Team);
        PlayerScoreKeeper = new ScoreKeeper(GameManager.Instance.WinningScore);
        Score = 0;
        CreateTeam();
    }

    protected void OnGameEnded()
    {
        FlagManager.Instance.DestroyFlag(Flag);
        TeamList.Clear();
        MyCharacterSpawner.ClearSpawner();
    }

    /// <summary>
    /// Handles new score event in favor of this player.
    /// </summary>
    protected void OnScore()
    {
        GameManager myGameManager = GameManager.Instance;
        //Update score
        bool hasWon = PlayerScoreKeeper.IncrementScore();
        Score = PlayerScoreKeeper.CurrentScore;
        //Update winner if necessary
        if (hasWon)
        {
            myGameManager.LockScores();
            myGameManager.UpdateWinner(Team);
            myGameManager.MyGameStateMachine.NextState();
        }
        //Update score UI
        UIManager.Instance.UpdateScoreDisplay();
    }

    #endregion

    #region Team management

    /// <summary>
    /// Creates a character controller and its object. Adds it to the TeamList attribute.
    /// </summary>
    protected abstract void CreateCharacter();

    /// <summary>
    /// Creates all the characters of the team.
    /// </summary>
    protected abstract void CreateTeam();

    /// <summary>
    /// Safely removes given character from TeamList attribute.
    /// </summary>
    /// <param name="characterToRemove">Character to remove from TeamList attribute.</param>
    protected void RemoveCharacter(NPCController characterToRemove)
    {
        if(TeamList.Contains(characterToRemove))
        {
            TeamList.Remove(characterToRemove);
        }
    }
    #endregion 

}
