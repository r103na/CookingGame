using System;

namespace CookingGame.Managers
{
    public class ScoreManager
    {
        public int Score;
        public int MaxScore = 30;

        public event EventHandler ScoreIncreased;
        public event EventHandler ScoreDecreased;

        public void IncreaseScore()
        {
            Score += 10;
            OnScoreIncreased();
        }

        public void DecreaseScore()
        {
            Score -= 10;
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
