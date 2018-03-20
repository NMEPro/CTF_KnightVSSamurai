using UnityEngine;

/// <summary>
/// Utility component to add to game objects whose events you want forwarded from Unity's message
/// system to standard C# events. Handles all events as of Unity 4.6.1.
/// </summary>
/// <remarks>Original author: Jackson Dunstan - http://jacksondunstan.com/articles/2922
/// I have modified and adapted this class for the needs of the CTF project.</remarks>
public class EventForwarder : MonoBehaviour
{
    public delegate void EventHandler0();
    public delegate void EventHandler1<TParam>(TParam param);
    public delegate void EventHandler2<TParam1, TParam2>(TParam1 param1, TParam2 param2);

    #region Default Unity events
    public event EventHandler0 AwakeEvent = () => { };
    public event EventHandler0 FixedUpdateEvent = () => { };
    public event EventHandler0 LateUpdateEvent = () => { };
    public event EventHandler1<Collision> OnCollisionEnterEvent = collision => { };
    public event EventHandler1<Collision2D> OnCollisionEnter2DEvent = collision => { };
    public event EventHandler1<Collision> OnCollisionExitEvent = collision => { };
    public event EventHandler1<Collision2D> OnCollisionExit2DEvent = collision => { };
    public event EventHandler1<Collision> OnCollisionStayEvent = collision => { };
    public event EventHandler1<Collision2D> OnCollisionStay2DEvent = collision => { };
    public event EventHandler0 OnDestroyEvent = () => { };
    public event EventHandler0 OnDisableEvent = () => { };
    public event EventHandler0 OnEnableEvent = () => { };
    public event EventHandler0 OnGUIEvent = () => { };
    public event EventHandler0 OnMouseDownEvent = () => { };
    public event EventHandler0 OnMouseDragEvent = () => { };
    public event EventHandler0 OnMouseEnterEvent = () => { };
    public event EventHandler0 OnMouseExitEvent = () => { };
    public event EventHandler0 OnMouseOverEvent = () => { };
    public event EventHandler0 OnMouseUpEvent = () => { };
    public event EventHandler0 OnMouseUpAsButtonEvent = () => { };
    public event EventHandler1<Collider> OnTriggerEnterEvent = other => { };
    public event EventHandler1<Collider2D> OnTriggerEnter2DEvent = other => { };
    public event EventHandler1<Collider> OnTriggerExitEvent = other => { };
    public event EventHandler1<Collider2D> OnTriggerExit2DEvent = other => { };
    public event EventHandler1<Collider> OnTriggerStayEvent = other => { };
    public event EventHandler1<Collider2D> OnTriggerStay2DEvent = other => { };
    public event EventHandler0 ResetEvent = () => { };
    public event EventHandler0 StartEvent = () => { };
    public event EventHandler0 UpdateEvent = () => { };
    #endregion

    #region Personalized Unity events
    public event EventHandler0 OnKnightFlagCapturedEvent = () => { };
    public event EventHandler0 OnSamuraiFlagCapturedEvent = () => { };
    public event EventHandler0 OnKnightFlagReleasedEvent = () => { };
    public event EventHandler0 OnSamuraiFlagReleasedEvent = () => { };
    public event EventHandler0 OnTakeFlagEvent = () => { };
    public event EventHandler0 OnKnightScoredEvent = () => { };
    public event EventHandler0 OnSamuraiScoredEvent = () => { };
    public event EventHandler0 OnGameStartedEvent = () => { };
    public event EventHandler0 OnGameResumedEvent = () => { };
    public event EventHandler0 OnGameEndedEvent = () => { };
    public event EventHandler1<NPCController> OnKnightDeathEvent = (deadKnightController) => { };
    public event EventHandler1<NPCController> OnSamuraiDeathEvent = (deadKnightController) => { };
    public event EventHandler2<CharacterView, GameParameters.Team> OnTryCaptureFlagEvent = (captor, captorTeam) => { };
    #endregion

    #region Default Unity event invokes
    public void Awake()
    {
        AwakeEvent();
    }

    public void FixedUpdate()
    {
        FixedUpdateEvent();
    }

    public void LateUpdate()
    {
        LateUpdateEvent();
    }

    public void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent(collision);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2DEvent(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2DEvent(collision);
    }

    public void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent(collision);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionStay2DEvent(collision);
    }

    public void OnDestroy()
    {
        OnDestroyEvent();
    }

    public void OnDisable()
    {
        OnDisableEvent();
    }

    public void OnEnable()
    {
        OnEnableEvent();
    }

    public void OnGUI()
    {
        OnGUIEvent();
    }

   public void OnMouseDown()
    {
        OnMouseDownEvent();
    }

    public void OnMouseDrag()
    {
        OnMouseDragEvent();
    }

    public void OnMouseEnter()
    {
        OnMouseEnterEvent();
    }

    public void OnMouseExit()
    {
        OnMouseExitEvent();
    }

    public void OnMouseOver()
    {
        OnMouseOverEvent();
    }

    public void OnMouseUp()
    {
        OnMouseUpEvent();
    }

    public void OnMouseUpAsButton()
    {
        OnMouseUpAsButtonEvent();
    }

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent(other);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter2DEvent(other);
    }

    public void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent(other);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit2DEvent(other);
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent(other);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStay2DEvent(other);
    }

    public void Reset()
    {
        ResetEvent();
    }

    public void Start()
    {
        RegisterPersonalizedEvents();
        StartEvent();
    }

    public void Update()
    {
        UpdateEvent();
    }
    #endregion

    #region Personalized Unity event invokes
    public void OnKnightFlagCaptured()
    {
        OnKnightFlagCapturedEvent();
    }

    public void OnSamuraiFlagCaptured()
    {
        OnSamuraiFlagCapturedEvent();
    }

    public void OnKnightFlagReleased()
    {
        OnKnightFlagReleasedEvent();
    }

    public void OnSamuraiFlagReleased()
    {
        OnSamuraiFlagReleasedEvent();
    }

    public void OnTakeFlag()
    {
        OnTakeFlagEvent();
    }

    public void OnKnightScored()
    {
        OnKnightScoredEvent();
    }

    public void OnSamuraiScored()
    {
        OnSamuraiScoredEvent();
    }
    
    public void OnGameStarted()
    {
        OnGameStartedEvent();
    }

    public void OnGameResumed()
    {
        OnGameResumedEvent();
    }

    public void OnGameEnded()
    {
        OnGameEndedEvent();
    }

    public void OnKnightDeath(NPCController deadKnightController)
    {
        OnKnightDeathEvent(deadKnightController);
    }

    public void OnSamuraiDeath(NPCController deadSamuraiController)
    {
        OnSamuraiDeathEvent(deadSamuraiController);
    }

    public void OnTryCaptureFlag(CharacterView captor, GameParameters.Team captorTeam)
    {
        OnTryCaptureFlagEvent(captor, captorTeam);
    }

    #endregion

    private void RegisterPersonalizedEvents()
    {
        EventManager gameEventManager = EventManager.Instance;
        gameEventManager.AddStartGameListener(OnGameStarted);
        gameEventManager.AddResumeGameListener(OnGameResumed);
        gameEventManager.AddEndGameListener(OnGameEnded);
        gameEventManager.AddKnightDeathEventListener(OnKnightDeath);
        gameEventManager.AddKnightFlagCapturedListener(OnKnightFlagCaptured);
        gameEventManager.AddKnightFlagReleasedListener(OnKnightFlagReleased);
        gameEventManager.AddKnightScoredEventListener(OnKnightScored);
        gameEventManager.AddSamuraiDeathEventListener(OnSamuraiDeath);
        gameEventManager.AddSamuraiFlagCapturedListener(OnSamuraiFlagCaptured);
        gameEventManager.AddSamuraiFlagReleasedListener(OnSamuraiFlagReleased);
        gameEventManager.AddSamuraiScoredEventListener(OnSamuraiScored);
        gameEventManager.AddTryFlagCaptureListener(OnTryCaptureFlag);
    }
}