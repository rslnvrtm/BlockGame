using System;

namespace BlocksGame.MVC.Events
{
    public class UpdateScoreEvent : EventArgs
    {
        public readonly int Score;

        public UpdateScoreEvent(int score)
            => Score = score;
    }
}
