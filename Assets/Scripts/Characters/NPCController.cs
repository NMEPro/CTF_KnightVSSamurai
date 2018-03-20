using UnityEngine;

/// <summary>
/// Manages a character.
/// </summary>
public abstract class NPCController {

    protected readonly EventForwarder MyEventForwarder;
    /// <summary>
    /// NPC instantiation in the Unity scene.
    /// </summary>
    public CharacterView View { get; private set; }

    protected NPCController(CharacterView view)
    {
        View = view;
        View.HasFlag = false;
        View.IsAlive = true;

        MyEventForwarder = view.MyEventForwarder;
        MyEventForwarder.FixedUpdateEvent += HandleFixedUpdate;
        MyEventForwarder.OnTriggerEnter2DEvent += HandleTriggerEnter;
        MyEventForwarder.OnTakeFlagEvent += HandleTakingFlag;
    }

    #region Event management

    /// <summary>
    /// Handles collision between this character's collider and a trigger collider
    /// </summary>
    /// <param name="otherCollider">other collider hit.</param>
    protected abstract void HandleTriggerEnter(Collider2D otherCollider);

    protected void HandleFixedUpdate()
    {
        Vector2 newMovement = new Vector2();
        Vector2 newPosition = new Vector2();
        GameStateMachine.State currentState = GameManager.Instance.MyGameStateMachine.CurrentState;
        if (currentState != GameStateMachine.State.Paused)
        {
            //Compute NPC movement at a fixed interval.
            newMovement = View.MyCharacterMovement.ComputeMovement(View.transform.position);
            View.transform.Translate(newMovement);
            newPosition = (Vector2)View.transform.position + newMovement;
            //Move NPC view according to computations.
            View.GetComponent<Rigidbody2D>().MovePosition(newPosition);
        }
    }

    private void HandleTakingFlag()
    {
        View.HasFlag = true;
        View.MyCharacterMovement.SetCurrentMovement(CharacterMovement.DefaultMovementState.GoToOwnFlag);
        View.MySpriteRenderer.sprite = View.CharacterWithFlagSprite;
    }

    protected void HandleEnemyFlagCaptured()
    {
        if(!View.HasFlag)
        {
            View.MyCharacterMovement.ChooseMovementEnemyFlagCaptured();
        }
    }

    protected void HandleEnemyFlagReleased()
    {
        View.MyCharacterMovement.ChooseMovementEnemyFlagReleased();
    }
    #endregion

    protected abstract void ReleaseFlag();
    protected abstract void Die();

    /// <summary>
    /// Syncrhonizes character sprite alpha value with health percentage.
    /// </summary>
    /// <remarks>This allows to visualize remaining health on a character</remarks>
    protected void SyncSpriteAlphaToHealth()
    {
        Color spriteColor = View.MySpriteRenderer.color;
        float newAlpha = View.Health / (float)GameParameters.MAX_HEALTH;
        Color newColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, newAlpha);
        View.MySpriteRenderer.color = newColor;
    }
}
