/// <summary>
/// Keeps track of the score for a team.
/// </summary>
public class ScoreKeeper {

    public int CurrentScore { get; private set; }
    private int winningScore;
    private bool scoreLocked;
    
    public ScoreKeeper(int givenWinningScore)
    {
        scoreLocked = false;
        CurrentScore = 0;
        winningScore = givenWinningScore;
    }

    /// <summary>
    /// Increases current score by 1 point and checks if score is now equal to winning score.
    /// </summary>
    /// <returns>Returns true if current score equals winning score. False otherwise.</returns>
    public bool IncrementScore()
    {
        if(!scoreLocked)
        {
            CurrentScore++;
        }
        return CurrentScore == winningScore;
    }

    /// <summary>
    /// Prevents score from being updated.
    /// </summary>
    public void LockScore()
    {
        scoreLocked = true;
    }



}
