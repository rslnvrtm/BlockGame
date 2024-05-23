using System;

namespace BlocksGame.MVC.Events
{
    public class UpdateMapEvent : EventArgs
    {
        public readonly BlockType[,] Map;
        
        public UpdateMapEvent(BlockType[,] map)
            => Map = map;
    }
}
