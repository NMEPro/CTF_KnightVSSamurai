using UnityEngine;

public class SamuraiController : NPCController {

    public SamuraiController(CharacterView view, float velocity,
        bool isEnemyFlagCaptured) : base(view)
    {
        View.MyCharacterMovement = new SamuraiMovement(velocity, isEnemyFlagCaptured);
        View.Health = GameParameters.SAMURAI_HEALTH;
        View.Damage = GameManager.Instance.SamuraiDamage;
        View.DefaultSprite = SpriteLoader.Instance.DefaultSamuraiSprite;
        View.CharacterWithFlagSprite = SpriteLoader.Instance.SamuraiSpriteWithFlag;
        MyEventForwarder.OnKnightFlagCapturedEvent += HandleEnemyFlagCaptured;
        MyEventForwarder.OnKnightFlagReleasedEvent += HandleEnemyFlagReleased;
    }

    /// <summary>
    /// Handles collision between this character's collider and a trigger collider
    /// </summary>
    /// <param name="otherCollider">other collider hit.</param>
    protected override void HandleTriggerEnter(Collider2D otherCollider)
    {
        GameManager myGameManager = GameManager.Instance;
        if (otherCollider.gameObject == myGameManager.KnightPlayer.Flag) //Collided with other player's flag.
        {
            EventManager.Instance.InvokeTryFlagCaptureEvent(View, GameParameters.Team.Samurai);
        }
        else if (otherCollider.gameObject == myGameManager.SamuraiPlayer.Flag) //Collided with own flag.
        {
            if(View.HasFlag)
            {
                EventManager.Instance.InvokeSamuraiScoredEvent();
                ReleaseFlag();
            }
        }
        else //Collided with an enemy character.
        {
            int collisionDamage = otherCollider.transform.GetComponent<CharacterView>().Damage;
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
            EventManager.Instance.InvokeSamuraiDeathEvent(this);
        }
    }

    protected override void ReleaseFlag()
    {
        View.HasFlag = false;
        View.MySpriteRenderer.sprite = View.DefaultSprite;
        EventManager.Instance.InvokeKnightFlagReleasedEvent();
    }
}
