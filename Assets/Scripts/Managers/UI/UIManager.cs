using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the UI.
/// </summary>
public class UIManager : Singleton<UIManager> {

    #region Serialized attributes
    [SerializeField]
    private Text KnightScoreText = null;
    [SerializeField]
    private Text SamuraiScoreText = null;
    [SerializeField]
    private Text KnightAmountText = null;
    [SerializeField]
    private Text SamuraiAmountText = null;
    [SerializeField]
    private Text WinningScoreText = null;
    [SerializeField]
    private Text WinningTeamText = null;
    [SerializeField]
    private Button IncreaseKnightAmountButton = null;
    [SerializeField]
    private Button DecreaseKnightAmountButton = null;
    [SerializeField]
    private Button IncreaseWinningScoreButton = null;
    [SerializeField]
    private Button DecreaseWinningScoreButton = null;
    [SerializeField]
    private CanvasGroup HUD;
    [SerializeField]
    private CanvasGroup StartMenu;
    [SerializeField]
    private CanvasGroup EndMenu;
    #endregion

    private void Start()
    {
        EventManager.Instance.AddEndGameListener(OnGameEnded);
        EventManager.Instance.AddStartGameListener(OnGameStart);
        GameManager myGameManager = GameManager.Instance;
        KnightAmountText.text = myGameManager.KnightAmount.ToString();
        SamuraiAmountText.text = myGameManager.SamuraiAmount.ToString();
        WinningScoreText.text = myGameManager.WinningScore.ToString();
        CheckAllButtonsAvailability();
    }

    public void OnGameEnded()
    {
        UpdateMenuValue(WinningTeamText, GameManager.Instance.Winner); //Update winner team in UI.
        HUD.alpha = 0; //Hide HUD.
        ShowEndMenu();
    }

    public void OnGameStart()
    {
        ResetKnightScoreDisplay();
        ResetSamuraiScoreDisplay();
    }

    /// <summary>
    /// Safely updates the value of a given text.
    /// </summary>
    /// <param name="textToUpdate"></param>
    /// <param name="newValue"></param>
    private void UpdateMenuValue(Text textToUpdate, string newValue)
    {
        if (newValue != "-1")
        {
            textToUpdate.text = newValue;
        }
    }

    /// <summary>
    /// Updates character amount texts with given values.
    /// </summary>
    /// <param name="newKnightAmount"></param>
    /// <param name="newSamuraiAmount"></param>
    private void UpdateNPCText(string newKnightAmount, string newSamuraiAmount)
    {
        UpdateMenuValue(KnightAmountText, newKnightAmount);
        UpdateMenuValue(SamuraiAmountText, newSamuraiAmount);
    }


    #region HUD values management

    /// <summary>
    /// Retrieves scores and updates HUD.
    /// </summary>
    public void UpdateScoreDisplay()
    {
        GameManager myGameManager = GameManager.Instance;
        KnightScoreText.text = myGameManager.KnightPlayer.Score.ToString();
        SamuraiScoreText.text = myGameManager.SamuraiPlayer.Score.ToString();
        
    }

    private void ResetKnightScoreDisplay()
    {
        KnightScoreText.text = "0";
    }

    private void ResetSamuraiScoreDisplay()
    {
        SamuraiScoreText.text = "0";
    }
    #endregion

    #region Menu display management

    private void ShowMenu(CanvasGroup menu)
    {
        menu.alpha = 1;
        menu.blocksRaycasts = true;
        menu.interactable = true;
    }

    private void HideMenu(CanvasGroup menu)
    {
        menu.alpha = 0;
        menu.blocksRaycasts = false;
        menu.interactable = false;
    }

    private void ShowStartMenu()
    {
        ShowMenu(StartMenu);
    }

    private void HideStartMenu()
    {
        HideMenu(StartMenu);
    }

    private void ShowEndMenu()
    {
        ShowMenu(EndMenu);
    }

    private void HideEndMenu()
    {
        HideMenu(EndMenu);
    }
    #endregion

    #region Buttons handlers
    public void IncreaseKnightAmount()
    {
        GameManager myGameManager = GameManager.Instance;
        if(myGameManager.TryIncreaseKnightAmount())
        {
            CheckKnightButtonsAvailability();
            UpdateNPCText(myGameManager.KnightAmount.ToString(), myGameManager.SamuraiAmount.ToString());
        }
    }

    public void DecreaseKnightAmount()
    {
        GameManager myGameManager = GameManager.Instance;
        if (myGameManager.TryDecreaseKnightAmount())
        {
            CheckKnightButtonsAvailability();
            UpdateNPCText(myGameManager.KnightAmount.ToString(), myGameManager.SamuraiAmount.ToString());
        }
    }

    public void IncreaseWinningScore()
    {
        GameManager myGameManager = GameManager.Instance;
        if (myGameManager.TryIncreaseWinningScore())
        {
            CheckWinningScoreButtonsAvailability();
            UpdateMenuValue(WinningScoreText, myGameManager.WinningScore.ToString());
        }
    }

    public void DecreaseWinningScore()
    {
        GameManager myGameManager = GameManager.Instance;
        if (myGameManager.TryDecreaseWinningScore())
        {
            CheckWinningScoreButtonsAvailability();
            UpdateMenuValue(WinningScoreText, myGameManager.WinningScore.ToString());
        }
    }

    public void PlayGame()
    {
        HUD.alpha = 1;
        HideStartMenu();
        GameManager.Instance.MyGameStateMachine.NextState();
    }


    public void RestartGame()
    {
        HideEndMenu();
        ShowStartMenu();
        GameManager.Instance.MyGameStateMachine.NextState();
    }
    #endregion

    #region Check button availability

    /// <summary>
    /// Checks if a button should be made not interactable. Does so if necessary.
    /// </summary>
    /// <param name="currentValue">Current value for this button.</param>
    /// <param name="limitValue">Limit value for this button.</param>
    /// <param name="button">Button to modify.</param>
    private void CheckButtonAvailability(int currentValue, int limitValue, Button button)
    {
        if (currentValue == limitValue)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    /// <summary>
    /// Checks if winning score buttons should be made not interactable. Does so if necessary.
    /// </summary>
    private void CheckWinningScoreButtonsAvailability()
    {
        GameManager myGameManager = GameManager.Instance;
        CheckButtonAvailability(myGameManager.WinningScore, GameParameters.MAX_WINNING_SCORE,
            IncreaseWinningScoreButton);
        CheckButtonAvailability(myGameManager.WinningScore, GameParameters.MIN_WINNING_SCORE,
            DecreaseWinningScoreButton);
    }

    /// <summary>
    /// Checks if knight amount buttons should be made not interactable. Does so if necessary.
    /// </summary>
    private void CheckKnightButtonsAvailability()
    {
        GameManager myGameManager = GameManager.Instance;
        CheckButtonAvailability(myGameManager.KnightAmount, GameParameters.MAX_KNIGHT_AMOUNT,
            IncreaseKnightAmountButton);
        CheckButtonAvailability(myGameManager.KnightAmount, GameParameters.MIN_KNIGHT_AMOUNT,
            DecreaseKnightAmountButton);
    }

    /// <summary>
    /// Checks if any UI button should be made not interactable. Does so if necessary.
    /// </summary>
    private void CheckAllButtonsAvailability()
    {
        CheckKnightButtonsAvailability();
        CheckWinningScoreButtonsAvailability();
    }
    #endregion
}
