using UnityEngine;

/// <summary>
/// Handles game states.
/// </summary>
public class GameStateMachine {

    public enum State { StartMenu = 0, Paused, Playing, EndMenu}
    [HideInInspector]
    public State CurrentState { get; private set; }

    public GameStateMachine()
    {
        CurrentState = State.StartMenu;
    }

    public void NextState()
    {
        switch(CurrentState)
        {
            case State.StartMenu:
                StartGame();
                break;
            case State.Paused:
                ResumeGame();
                break;
            case State.Playing:
                EndGame();
                break;
            case State.EndMenu:
                RestartGame();
                break;
        }
    }

    public void PauseGame()
    {
        CurrentState = State.Paused;
        EventManager.Instance.InvokePauseGame();
    }

    private void StartGame()
    {
        CurrentState = State.Playing;
        EventManager.Instance.InvokeStartGame();
    }

    private void ResumeGame()
    {
        CurrentState = State.Playing;
        EventManager.Instance.InvokeResumeGame();
    }

    private void EndGame()
    {
        CurrentState = State.EndMenu;
        EventManager.Instance.InvokeEndGame();
    }

    private void RestartGame()
    {
        CurrentState = State.StartMenu;
    }
}
