using UnityEngine;

/// <summary>
/// Handles character movement computations and choices.
/// </summary>
public abstract class CharacterMovement {

    public enum DefaultMovementState {NoMovement = 0, GoForEnemyFlag, GoToOwnFlag};
    public DefaultMovementState CurrentDefaultMovementState { get; private set; }
    protected bool IsUsingSpecialMove = false;
    protected float Velocity;

    public CharacterMovement(float givenVelocity)
    {
        Velocity = givenVelocity;
        CurrentDefaultMovementState = DefaultMovementState.GoForEnemyFlag;
    }

    /// <summary>
    /// Chooses movement state when enemy flag is captured.
    /// </summary>
    public abstract void ChooseMovementEnemyFlagCaptured();

    /// <summary>
    /// Chooses movement state when enemy flag is released.
    /// </summary>
    public abstract void ChooseMovementEnemyFlagReleased();

    #region Movement computations
    /// <summary>
    /// Computes a movement vector for a default movement state.
    /// </summary>
    /// <param name="currentPosition">Current position of the character view.</param>
    /// <returns></returns>
    protected abstract Vector2 ComputeDefaultMovement(Vector2 currentPosition);
    
    /// <summary>
    /// Computes a movement vector for a special movement state.
    /// </summary>
    /// <param name="curentPosition">Current position of the character view.</param>
    /// <returns></returns>
    protected abstract Vector2 ComputeSpecialMovement(Vector2 curentPosition);

    /// <summary>
    /// Computes movement according to current special move use.
    /// </summary>
    /// <param name="currentPosition">Current position of the character view.</param>
    /// <returns></returns>
    public Vector2 ComputeMovement(Vector2 currentPosition)
    {
        Vector2 newMovement = new Vector2(0, 0);

        if (IsUsingSpecialMove)
        {
            newMovement = ComputeSpecialMovement(currentPosition);
        }
        else
        {
            newMovement = ComputeDefaultMovement(currentPosition);
        }
        return newMovement;   
    }

    /// <summary>
    /// Computes the movement vector from postion to target according to velocity.
    /// </summary>
    /// <param name="currentPosition">Current position of the character view.</param>
    /// <param name="targetPosition">Target position to reach.</param>
    /// <param name="velocity">Velocity of the character.</param>
    /// <returns>Movement vector.</returns>
    protected Vector2 ComputeVectorToTarget(Vector2 currentPosition,
        Vector2 targetPosition, float velocity)
    {
        Vector2 CharacterToPosition = targetPosition - currentPosition;
        return (CharacterToPosition.normalized) * velocity;
    }
    #endregion

    #region Accessors
    public void SetCurrentMovement(DefaultMovementState move)
    {
        CurrentDefaultMovementState = move;
    }
    #endregion
}
