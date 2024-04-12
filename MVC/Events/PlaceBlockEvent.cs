using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class PlaceBlockEvent : EventArgs
    {
        public readonly bool[,] BlockMatrix;
        // position of upper left corner of BlockMatrix
        public readonly Point Position;

        public PlaceBlockEvent(bool[,] blockMatrix, Point position)
        {
            BlockMatrix = blockMatrix;
            Position = position;
        }
    }
}