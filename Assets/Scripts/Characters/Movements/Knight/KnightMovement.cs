using UnityEngine;

public class KnightMovement : CharacterMovement {

    public enum SpecialKnightMove { KillRandom = 0}
    private SpecialKnightMove CurrentSpecialKnightMove = SpecialKnightMove.KillRandom;
    private Transform specialTarget;

    public KnightMovement(float givenVelocity, bool isEnemyFlagCaptured) : base(givenVelocity)
    {
        specialTarget = null;
        IsUsingSpecialMove = isEnemyFlagCaptured;
    }

    /// <summary>
    /// Computes a movement vector for a default movement state.
    /// </summary>
    /// <param name="currentPosition">Current position of the character view.</param>
    /// <returns></returns>
    protected override Vector2 ComputeDefaultMovement(Vector2 currentKnightPosition)
    {
        GameManager myGameManager = GameManager.Instance;
        Vector2 defaultMove = new Vector2(0, 0);
        switch (CurrentDefaultMovementState)
        {
            case DefaultMovementState.NoMovement:
                //Nothing to do, character stays immobile.
                break;
            case DefaultMovementState.GoForEnemyFlag:
                defaultMove = ComputeVectorToTarget(currentKnightPosition,
                    myGameManager.SamuraiPlayer.Flag.transform.position,
                    Velocity);
                break;
            case DefaultMovementState.GoToOwnFlag:
                defaultMove = ComputeVectorToTarget(currentKnightPosition,
                    myGameManager.KnightPlayer.Flag.transform.position,
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
    protected override Vector2 ComputeSpecialMovement(Vector2 currentKnightPosition)
    {
        GameManager myGameManager = GameManager.Instance;
        Vector2 specialMove = new Vector2(0, 0);
        if (specialTarget == null
            || specialTarget.GetComponent<CharacterView>().IsAlive == false)
        {
            specialTarget = ChooseRandomTarget(myGameManager.SamuraiPlayer);
        }
        switch (CurrentSpecialKnightMove)
        {
            case SpecialKnightMove.KillRandom:
                if (specialTarget != null)
                {
                    specialMove = ComputeVectorToTarget(currentKnightPosition,
                        specialTarget.position, Velocity);
                }
                break;
            default:
                break;
        }
        return specialMove;
    }

    /// <summary>
    /// Chooses a random target among enemy team list.
    /// </summary>
    /// <param name="enemyPlayer">Player from which target should be chosen.</param>
    /// <returns>Target transform.</returns>
    private Transform ChooseRandomTarget(Player enemyPlayer)
    {
        Transform target = null;
        int randomIndex = -1;
        int enemyAmount = enemyPlayer.TeamList.Count;
        if (enemyAmount > 0)
        {
            randomIndex = Random.Range(0, enemyAmount - 1);
            target = enemyPlayer.TeamList[randomIndex].View.transform;
        }
        return target;
    }

    /// <summary>
    /// Chooses movement state when enemy flag is captured.
    /// </summary>
    public override void ChooseMovementEnemyFlagCaptured()
    {
        IsUsingSpecialMove = true;
        CurrentSpecialKnightMove = SpecialKnightMove.KillRandom;
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