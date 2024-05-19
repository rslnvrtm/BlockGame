using System;

namespace BlocksGame.MVC.Events
{
    public class GameOverEvent : EventArgs 
    {
        public readonly int Score;

        public GameOverEvent(int score)
            => Score = score;
    }
}
