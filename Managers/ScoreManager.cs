using System;

namespace CookingGame.Managers
{
    public class ScoreManager
    {
        public int Score;
        public int MaxScore = 1000;

        public event EventHandler ScoreIncreased;
        public event EventHandler ScoreDecreased;

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

        protected virtual void OnScoreIncreased()
        {
            ScoreIncreased?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnScoreDecreased()
        {
            ScoreDecreased?.Invoke(this, EventArgs.Empty);
        }
    }
}
