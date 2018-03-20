using UnityEngine;

public class SamuraiPlayer : Player
{
    public SamuraiPlayer(CharacterSpawner characterSpawner, EventForwarder playerEventForwarder)
        : base(characterSpawner, playerEventForwarder)
    {
        Team = GameParameters.Team.Samurai;
        Enemy = GameParameters.Team.Knight;
        MyEventForwarder.OnSamuraiDeathEvent += OnDeathEvent;
        MyEventForwarder.OnSamuraiFlagReleasedEvent += OnFlagReleased;
        MyEventForwarder.OnSamuraiScoredEvent += OnScore;
    }

    /// <summary>
    /// Handles despawning and respawning a character.
    /// </summary>
    /// <param name="deadCharController">Character to despawn.</param>
    protected override void OnDeathEvent(NPCController deadSamuraiController)
    {
        GameObject samuraiObject = null;
        //Despawn
        samuraiObject = deadSamuraiController.View.gameObject;
        RemoveCharacter(deadSamuraiController);
        MyCharacterSpawner.DespawnSamurai(samuraiObject);
        //Respawn
        MyCharacterSpawner.RespawnSamurai(TeamList);
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
            EventManager.Instance.InvokeSamuraiFlagCapturedEvent();
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
        tmpInstantiatedObject = MyCharacterSpawner.SpawnSamurai(true);
        TeamList.Add(new SamuraiController(tmpInstantiatedObject.GetComponent<CharacterView>(),
            myGameManager.SamuraiVelocity, HasEnemyFlag()));
    }

    /// <summary>
    /// Creates all the characters of the team.
    /// </summary>
    protected override void CreateTeam()
    {
        int samuraiAmount = GameManager.Instance.SamuraiAmount;

        for(int i = 0; i < samuraiAmount; i++)
        {
            CreateCharacter();
        }
    }

}