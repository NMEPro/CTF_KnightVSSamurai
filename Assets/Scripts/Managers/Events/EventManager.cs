using UnityEngine.Events;

/// <summary>
/// Manages event calls and registrations.
/// </summary>
public class EventManager : Singleton<EventManager> {

    //Death events
    private DeathEvent KnightDeath = null;
    private DeathEvent SamuraiDeath = null;

    //Flag capture events
    private TryCaptureFlagEvent TryCaptureFlag = null;
    private UnityEvent KnightFlagCaptured = null;
    private UnityEvent SamuraiFlagCaptured = null;
    private UnityEvent KnightFlagReleased = null;
    private UnityEvent SamuraiFlagReleased = null;

    //Score events
    private UnityEvent KnightScored = null;
    private UnityEvent SamuraiScored = null;

    //Game state events
    private UnityEvent StartGame = null;
    private UnityEvent PauseGame = null;
    private UnityEvent ResumeGame = null;
    private UnityEvent EndGame = null;

    // Use this for initialization
    void Awake () {
        KnightDeath = new DeathEvent();
        SamuraiDeath = new DeathEvent();

        TryCaptureFlag = new TryCaptureFlagEvent();
        KnightFlagCaptured = new UnityEvent();
        SamuraiFlagCaptured = new UnityEvent();
        KnightFlagReleased = new UnityEvent();
        SamuraiFlagReleased = new UnityEvent();

        KnightScored = new UnityEvent();
        SamuraiScored = new UnityEvent();

        StartGame = new UnityEvent();
        PauseGame = new UnityEvent();
        ResumeGame = new UnityEvent();
        EndGame = new UnityEvent();
    }

    #region Death event
    public void AddSamuraiDeathEventListener(UnityAction<NPCController> listener)
    {
        SamuraiDeath.AddListener(listener);
    }

    public void AddKnightDeathEventListener(UnityAction<NPCController> listener)
    {
        KnightDeath.AddListener(listener);
    }

    public void InvokeKnightDeathEvent(NPCController deadKnightController)
    {
        if (KnightDeath != null)
        {
            KnightDeath.Invoke(deadKnightController);
        }
    }

    public void InvokeSamuraiDeathEvent(NPCController deadSamuraiController)
    {
        if (SamuraiDeath != null)
        {
            SamuraiDeath.Invoke(deadSamuraiController);
        }
    }
    #endregion

    #region Flag capture events

    #region Try capture

    public void AddTryFlagCaptureListener(UnityAction<CharacterView, GameParameters.Team> listener)
    {
        TryCaptureFlag.AddListener(listener);
    }

    public void InvokeTryFlagCaptureEvent(CharacterView captor, GameParameters.Team captorTeam)
    {
        TryCaptureFlag.Invoke(captor, captorTeam);
    }
    #endregion

    #region Flag captured
    public void AddKnightFlagCapturedListener(UnityAction listener)
    {

        KnightFlagCaptured.AddListener(listener);
    }

    public void InvokeKnightFlagCapturedEvent()
    {
        KnightFlagCaptured.Invoke();
    }

    public void AddSamuraiFlagCapturedListener(UnityAction listener)
    {

        SamuraiFlagCaptured.AddListener(listener);
    }

    public void InvokeSamuraiFlagCapturedEvent()
    {
        SamuraiFlagCaptured.Invoke();
    }
    #endregion

    #region Flag release

    public void AddKnightFlagReleasedListener(UnityAction listener)
    {

        KnightFlagReleased.AddListener(listener);
    }

    public void InvokeKnightFlagReleasedEvent()
    {
        KnightFlagReleased.Invoke();
    }

    public void AddSamuraiFlagReleasedListener(UnityAction listener)
    {

        SamuraiFlagReleased.AddListener(listener);
    }

    public void InvokeSamuraiFlagReleasedEvent()
    {
        SamuraiFlagReleased.Invoke();
    }
    #endregion

    #endregion

    #region Score events
    public void AddKnightScoredEventListener(UnityAction listener)
    {
        KnightScored.AddListener(listener);
    }

    public void AddSamuraiScoredEventListener(UnityAction listener)
    {
        SamuraiScored.AddListener(listener);
    }

    public void InvokeKnightScoredEvent()
    {
        KnightScored.Invoke();
    }

    public void InvokeSamuraiScoredEvent()
    {
        SamuraiScored.Invoke();
    }
    #endregion

    #region Game state events
    public void AddStartGameListener(UnityAction listener)
    {
        StartGame.AddListener(listener);
    }

    public void AddPauseGameListener(UnityAction listener)
    {
        PauseGame.AddListener(listener);
    }

    public void AddResumeGameListener(UnityAction listener)
    {
        ResumeGame.AddListener(listener);
    }

    public void AddEndGameListener(UnityAction listener)
    {
        EndGame.AddListener(listener);
    }

    public void InvokeStartGame()
    {
        StartGame.Invoke();
    }

    public void InvokePauseGame()
    {
        PauseGame.Invoke();
    }

    public void InvokeResumeGame()
    {
        ResumeGame.Invoke();
    }

    public void InvokeEndGame()
    {
        EndGame.Invoke();
    }

    #endregion

}
