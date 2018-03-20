using UnityEngine;

public class KnightController : NPCController {

    public KnightController(CharacterView view, float velocity,
        bool isEnemyFlagCaptured) : base(view)
    {
        View.MyCharacterMovement = new KnightMovement(velocity, isEnemyFlagCaptured);
        View.Health = GameParameters.KNIGHT_HEALTH;
        View.Damage = GameManager.Instance.KnightDamage;
        View.DefaultSprite = SpriteLoader.Instance.DefaultKnightSprite;
        View.CharacterWithFlagSprite = SpriteLoader.Instance.KnightSpriteWithFlag;
        MyEventForwarder.OnSamuraiFlagCapturedEvent += HandleEnemyFlagCaptured;
        MyEventForwarder.OnSamuraiFlagReleasedEvent += HandleEnemyFlagReleased;
    }

    /// <summary>
    /// Handles collision between this character's collider and a trigger collider
    /// </summary>
    /// <param name="otherCollider">other collider hit.</param>
    protected override void HandleTriggerEnter(Collider2D collider)
    {
        GameManager myGameManager = GameManager.Instance;
        if (collider.gameObject == myGameManager.SamuraiPlayer.Flag) //Collided with other player's flag.
        {
            EventManager.Instance.InvokeTryFlagCaptureEvent(View, GameParameters.Team.Knight);
        }
        else if (collider.gameObject == myGameManager.KnightPlayer.Flag) //Collided with own flag.
        {
            if (View.HasFlag)
            {
                EventManager.Instance.InvokeKnightScoredEvent();
                ReleaseFlag();
            }
        }
        else //Collided with an enemy character.
        {
            int collisionDamage = collider.transform.GetComponent<CharacterView>().Damage;
            View.Health = Mathf.Clamp(View.Health - collisionDamage, 0, int.MaxValue);
            if (View.Health == 0)
            {
                View.GetComponent<Collider2D>().enabled = false;
                Die();
            }
            else
            {
                SyncSpriteAlphaToHealth();
            }
        }
    }

    protected override void Die()
    {
        if (View.IsAlive) //Make sure that character is not already dead.
        {
            View.IsAlive = false;
            if (View.HasFlag)
            {
                ReleaseFlag();
            }
            EventManager.Instance.InvokeKnightDeathEvent(this);
        }
    }

    protected override void ReleaseFlag()
    {
        View.HasFlag = false;
        View.MySpriteRenderer.sprite = View.DefaultSprite;
        EventManager.Instance.InvokeSamuraiFlagReleasedEvent();
    }
}
