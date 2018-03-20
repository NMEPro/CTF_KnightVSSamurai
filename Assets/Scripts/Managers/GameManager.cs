using UnityEngine;

/// <summary>
/// Class which manages dynamic game parameters and players instantiation.
/// </summary>
[RequireComponent(typeof(EventForwarder))]
[RequireComponent(typeof(CharacterSpawner))]
[RequireComponent(typeof(FlagManager))]
[RequireComponent(typeof(EventManager))]
public class GameManager : Singleton<GameManager> {
   
    #region Selectable game parameters
    [Header("Character parameters")]

    [Header("NPC Amount")]
    [Tooltip("Amount of knights on the field.")]
    [Range(GameParameters.MIN_KNIGHT_AMOUNT, GameParameters.MAX_KNIGHT_AMOUNT)]
    public int KnightAmount = 4;

    [Tooltip("Ratio by which samurai amount is greater. The actual amount of samurai is ceiled to the next integer.")]
    [Range(GameParameters.MIN_SAMURAI_AMOUNT_RATIO, GameParameters.MAX_SAMURAI_AMOUNT_RATIO)]
    public float SamuraiAmountRatio = 1.5f;

    [Header("Velocities")]
    [Tooltip("Speed of a knight in Unity units / seconds.")]
    [Range(GameParameters.MIN_VELOCITY, GameParameters.MAX_VELOCITY)]
    public float KnightVelocity = 0.05f;

    [Tooltip("Ratio by which samurai speed is greater.")]
    [Range(GameParameters.MIN_SAMURAI_VELOCITY_RATIO, GameParameters.MAX_SAMURAI_VELOCITY_RATIO)]
    public float SamuraiVelocityRatio = 1.5f;

    [Header("Damages")]
    [Tooltip("Damages dealt to a samurai when a knight collides with it.")]
    [Range(1, GameParameters.MAX_HEALTH)]
    public int KnightDamage = 3;

    [Tooltip("Damages dealt to a knight when a samurai collides with it.")]
    [Range(1, GameParameters.MAX_HEALTH)]
    public int SamuraiDamage = 1;

    [Header("Score")]
    [Tooltip("Score a player has to reach to win the game.")]
    [Range(GameParameters.MIN_WINNING_SCORE, GameParameters.MAX_WINNING_SCORE)]
    public int WinningScore = 5;
    #endregion

    [HideInInspector]
    public int SamuraiAmount { get; private set; }
    [HideInInspector]
    public float SamuraiVelocity { get; private set; }

    /// <summary>
    /// Name of the winning team.
    /// </summary>
    [HideInInspector]
    public string Winner { get; private set; }

    public GameStateMachine MyGameStateMachine { get; private set; }
    public Player KnightPlayer { get; private set; }
    public Player SamuraiPlayer { get; private set; }
    
    private EventForwarder PlayerEventForwarder;
    private CharacterSpawner GameCharacterSpawner;

    private void Awake()
    {
        PlayerEventForwarder = GetComponent<EventForwarder>();
        GameCharacterSpawner = GetComponent<CharacterSpawner>();
        MyGameStateMachine = new GameStateMachine();
        SamuraiVelocity = KnightVelocity *  SamuraiVelocityRatio;
        SamuraiAmount = Mathf.CeilToInt(KnightAmount * SamuraiAmountRatio);
        Winner = "";
    }

    private void Start()
    {
        KnightPlayer = new KnightPlayer(GameCharacterSpawner, PlayerEventForwarder);
        SamuraiPlayer = new SamuraiPlayer(GameCharacterSpawner, PlayerEventForwarder);
    }

    private void Update()
    {
        PlayPauseOnkeyPress();
    }
    
    /// <summary>
    /// Handles Spacebar inputs to play/pause the game.
    /// </summary>
    private void PlayPauseOnkeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MyGameStateMachine.CurrentState == GameStateMachine.State.Paused)
            {
                MyGameStateMachine.NextState();
            }
            else if (MyGameStateMachine.CurrentState == GameStateMachine.State.Playing)
            {
                MyGameStateMachine.PauseGame();
            }
        }
    }

    #region Game parameters management
    /// <summary>
    /// Computes samurai amount according to KnightAmount attribute.
    /// </summary>
    private void UpdateSamuraiAmount()
    {
        SamuraiAmount = Mathf.CeilToInt(KnightAmount * SamuraiAmountRatio);
    }

    public bool TryIncreaseKnightAmount()
    {
        if(KnightAmount < GameParameters.MAX_KNIGHT_AMOUNT)
        {
            KnightAmount++;
            UpdateSamuraiAmount();
            return true;
        }
        return false;
    }

    public bool TryDecreaseKnightAmount()
    {
        if (KnightAmount > GameParameters.MIN_KNIGHT_AMOUNT)
        {
            KnightAmount--;
            UpdateSamuraiAmount();
            return true;
        }
        return false;
    }

    public bool TryDecreaseWinningScore()
    {
        if(WinningScore > GameParameters.MIN_WINNING_SCORE)
        {
            WinningScore--;
            return true;
        }
        return false;
    }

    public bool TryIncreaseWinningScore()
    {
        if (WinningScore < GameParameters.MAX_WINNING_SCORE)
        {
            WinningScore++;
            return true;
        }
        return false;
    }
    #endregion

    #region Scores management
    /// <summary>
    /// Locks the score of every player.
    /// </summary>
    public void LockScores()
    {
        KnightPlayer.LockScore();
        SamuraiPlayer.LockScore();
    }

    /// <summary>
    /// Updates winning team name based on winning team type.
    /// </summary>
    /// <param name="winningTeam">Winning team type.</param>
    public void UpdateWinner(GameParameters.Team winningTeam)
    {
        Winner = winningTeam.ToString().ToUpper();
    }
    #endregion

}
