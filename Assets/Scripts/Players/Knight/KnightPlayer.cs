using UnityEngine;

public class KnightPlayer : Player {

    public KnightPlayer(CharacterSpawner characterSpawner, EventForwarder playerEventForwarder)
        : base(characterSpawner, playerEventForwarder)
    {
        Team = GameParameters.Team.Knight;
        Enemy = GameParameters.Team.Samurai;
        MyEventForwarder.OnKnightDeathEvent += OnDeathEvent;
        MyEventForwarder.OnKnightFlagReleasedEvent += OnFlagReleased;
        MyEventForwarder.OnKnightScoredEvent += OnScore;
    }

    /// <summary>
    /// Handles despawning and respawning a character.
    /// </summary>
    /// <param name="deadCharController">Character to despawn.</param>
    protected override void OnDeathEvent(NPCController deadKnightController)
    {
        GameObject knightObject = null;
        //Despawn
        knightObject = deadKnightController.View.gameObject;
        RemoveCharacter(deadKnightController);
        MyCharacterSpawner.DespawnKnight(knightObject);
        //Respawn
        MyCharacterSpawner.RespawnKnight(TeamList);
    }

    /// <summary>
    /// Handles the own player's flag capture.
    /// </summary>
    /// <param name="captor">Captor which tried to capture player's flag.</param>
    /// <param name="captorType">Type of captor</param>
    protected override void OnTryCaptureFlag(CharacterView captor, GameParameters.Team captorTeam)
    {
        if (captorTeam == Enemy
            && IsFlagCapturable)
        {
            //Give flag to captor.
            captor.TakeFlag();
            //Prevent flag from being captured.
            IsFlagCapturable = false;
            Flag.GetComponent<Collider2D>().enabled = false;
            FlagManager.Instance.SetTakenFlagSprite(Team);
            //Warn that this player's flag has been captured.
            EventManager.Instance.InvokeKnightFlagCapturedEvent();
        }
    }

    protected override void OnFlagReleased()
    {
        IsFlagCapturable = true;
        Flag.GetComponent<Collider2D>().enabled = true;
        FlagManager.Instance.SetDefaultFlagSprite(Team);
    }

    /// <summary>
    /// Creates a character controller and its object. Adds it to the TeamList attribute.
    /// </summary>
    protected override void CreateCharacter()
    {
        GameManager myGameManager = GameManager.Instance;
        GameObject tmpInstantiatedObject = null;
        tmpInstantiatedObject = MyCharacterSpawner.SpawnKnight(true);
        TeamList.Add(new KnightController(tmpInstantiatedObject.GetComponent<CharacterView>(),
            myGameManager.KnightVelocity, HasEnemyFlag()));
    }

    /// <summary>
    /// Creates all the characters of the team.
    /// </summary>
    protected override void CreateTeam()
    {
        int knightAmount = GameManager.Instance.KnightAmount;

        for (int i = 0; i < knightAmount; i++)
        {
            CreateCharacter();
        }
    }
}