using UnityEngine;

public class SamuraiMovement : CharacterMovement {

    public enum SpecialSamuraiMove { FollowFlag = 0}
    private SpecialSamuraiMove CurrentSpecialSamuraiMove = SpecialSamuraiMove.FollowFlag;
    private Transform specialTarget;

    public SamuraiMovement(float givenVelocity, bool isEnemyFlagCaptured) : base(givenVelocity)
    {
        specialTarget = null;
        IsUsingSpecialMove = isEnemyFlagCaptured;
    }

    /// <summary>
    /// Computes a movement vector for a default movement state.
    /// </summary>
    /// <param name="currentPosition">Current position of the character view.</param>
    /// <returns></returns>
    protected override Vector2 ComputeDefaultMovement(Vector2 currentSamuraiPosition)
    {
        GameManager myGameManager = GameManager.Instance;
        Vector2 defaultMove = new Vector2(0, 0);
        switch (CurrentDefaultMovementState)
        {
            case DefaultMovementState.NoMovement:
                //Nothing to do, character stays immobile.
                break;
            case DefaultMovementState.GoForEnemyFlag:
                defaultMove = ComputeVectorToTarget(currentSamuraiPosition,
                    myGameManager.KnightPlayer.Flag.transform.position,
                    Velocity);
                break;
            case DefaultMovementState.GoToOwnFlag:
                defaultMove = ComputeVectorToTarget(currentSamuraiPosition,
                    myGameManager.SamuraiPlayer.Flag.transform.position,
                    Velocity);
                break;
            default:
                break;
        }
        return defaultMove;
        
    }

    /// <summary>
    /// Computes a movement vector for a special movement state.
    /// </summary>
    /// <param name="curentPosition">Current position of the character view.</param>
    /// <returns></returns>
    protected override Vector2 ComputeSpecialMovement(Vector2 currentSamuraiPosition)
    {
        GameManager myGameManager = GameManager.Instance;
        Vector2 specialMove = new Vector2(0, 0);
        if(specialTarget == null)
        {
            specialTarget = myGameManager.SamuraiPlayer.GetFlagBearer();
        }
        switch (CurrentSpecialSamuraiMove)
        {
            case SpecialSamuraiMove.FollowFlag:
                if (specialTarget != null)
                {
                    specialMove = ComputeVectorToTarget(currentSamuraiPosition,
                        specialTarget.position, Velocity);
                }
                break;
            default:
                break;
        }
        return specialMove;
    }

    /// <summary>
    /// Chooses movement state when enemy flag is captured.
    /// </summary>
    public override void ChooseMovementEnemyFlagCaptured()
    {
        CurrentSpecialSamuraiMove = SpecialSamuraiMove.FollowFlag;
        IsUsingSpecialMove = true;
    }

    /// <summary>
    /// Chooses movement state when enemy flag is released.
    /// </summary>
    public override void ChooseMovementEnemyFlagReleased()
    {
        IsUsingSpecialMove = false;
        specialTarget = null;
        SetCurrentMovement(DefaultMovementState.GoForEnemyFlag);
    }
}
