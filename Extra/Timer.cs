using System;
using System.Timers;

namespace CookingGame.Extra
{
    public class Timer
    {
        private System.Timers.Timer _timer;
        private int _duration;
        private Action _method;

        public Timer(int duration, Action method)
        {
            _duration = duration;
            _method = method;
            _timer = new System.Timers.Timer(_duration);
            _timer.Elapsed += TimerElapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _method.Invoke();
        }
    }
}

