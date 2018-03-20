/// <summary>
/// Manages respawn delays.
/// </summary>
public class SpawnTimer {

    public int RespawnDelayKnight { get; private set; }
    public int RespawnDelaySamurai { get; private set; }

    private int deathCountKnight;
    private int deathCountSamurai;

	public SpawnTimer() {
        deathCountKnight = deathCountSamurai = 0;
        RespawnDelayKnight = RespawnDelaySamurai = 1;
	}

    /// <summary>
    /// Checks whether given respawn delay should be increased.
    /// </summary>
    /// <param name="deathCount">Current death count</param>
    /// <param name="respawnDelay">Current respawn delay</param>
    /// <returns>True if respawn delay should be increased, false otherwise.</returns>
    private bool HasToIncreaseRespawnDelay(int deathCount, int respawnDelay)
    {
        return (deathCount % GameParameters.DEATH_COUNT_THRESHOLD == 0
            && respawnDelay < GameParameters.MAX_RESPAWN_TIME);
    }

    /// <summary>
    /// Increases knight death count and updates respawn delay accordingly.
    /// </summary>
    public void IncreaseDeathCountKnight()
    {
        deathCountKnight++;
        if(HasToIncreaseRespawnDelay(deathCountKnight, RespawnDelayKnight))
        {
            RespawnDelayKnight++;
        }
    }

    /// <summary>
    /// Increases samurai death count and updates respawn delay accordingly.
    /// </summary>
    public void IncreaseDeathCountSamurai()
    {
        deathCountSamurai++;
        if (HasToIncreaseRespawnDelay(deathCountSamurai, RespawnDelaySamurai))
        {
            RespawnDelaySamurai++;
        }
    }

    public void ResetSpawnCounters()
    {
        deathCountKnight = 0;
        deathCountSamurai = 0;
        RespawnDelayKnight = 1;
        RespawnDelaySamurai = 1;
    }
}
