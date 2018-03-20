using UnityEngine;

/// <summary>
/// Useful static game parameters
/// </summary>
public static class GameParameters{

    public enum Team { Knight = 0, Samurai }

    #region Health
    public const int MAX_HEALTH = 3;
    public const int KNIGHT_HEALTH = 3;
    public const int SAMURAI_HEALTH = 3;
    #endregion 

    #region Velocities
    public const float MIN_VELOCITY = 0.01f;
    public const float MAX_VELOCITY = 0.1f;

    /// <summary>
    /// Mininum ratio by which samurai are superior in velocity.
    /// </summary>
    public const float MIN_SAMURAI_VELOCITY_RATIO = 1.25f;

    /// <summary>
    /// Maximum ratio by which samurai are superior in velocity.
    /// </summary>
    public const float MAX_SAMURAI_VELOCITY_RATIO = 1.75f;

    #endregion

    #region NPC amounts
    public const int MAX_KNIGHT_AMOUNT = 6;
    public const int MIN_KNIGHT_AMOUNT = 1;

    /// <summary>
    /// Minimum ratio by which samurai are superior in number.
    /// </summary>
    public const float MIN_SAMURAI_AMOUNT_RATIO = 1.25f;

    /// <summary>
    /// Maximum ratio by which samurai are superior in number.
    /// </summary>
    public const float MAX_SAMURAI_AMOUNT_RATIO = 2f;
    #endregion

    #region Spawn
    public const int CHARACTERS_PER_COLUMN = 5;

    public static Vector2 KNIGHT_SPAWN_POSITION = new Vector2(11, 0);
    public static Vector2 SAMURAI_SPAWN_POSITION = new Vector2(-11, 0);

    /// <summary>
    /// Default ordinate offset between spawning characters.
    /// </summary>
    public const float CHARACTER_SPAWN_OFFSET_Y = 1.25f;
    #endregion

    #region Death and Respawn
    /// <summary>
    /// Treshold after which respawn time is increased by 1 sec.
    /// </summary>
    public const int DEATH_COUNT_THRESHOLD = 10;

    public const int MAX_RESPAWN_TIME = 5;
    #endregion

    #region Score
    public const int MIN_WINNING_SCORE = 3;
    public const int MAX_WINNING_SCORE = 10;
    #endregion
}
