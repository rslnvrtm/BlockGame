using System;

namespace BlocksGame.MVC.Events
{
    public class UpdateMapEvent : EventArgs
    {
        public readonly bool[,] Map;
        
        public UpdateMapEvent(bool[,] map)
            => Map = map;
    }
}
