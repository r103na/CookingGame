using System;

namespace CookingGame.Managers;

public class ScoreManager
{
    #region VARIABLES
    public int Score { get; set; }
    public int OrderCount { get; set; }
    public int MaxScore = 2000;
    public int MinScore = -100;

    public event EventHandler ScoreIncreased;
    public event EventHandler ScoreDecreased;
    #endregion

    #region METHODS
    public void IncreaseScore(int score)
    {
        Score += score;
        OnScoreIncreased();
    }

    public void DecreaseScore(int score)
    {
        Score -= score;
        OnScoreDecreased();
    }
    #endregion

    #region EVENT INVOCATORS
    protected virtual void OnScoreIncreased()
    {
        ScoreIncreased?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnScoreDecreased()
    {
        ScoreDecreased?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}