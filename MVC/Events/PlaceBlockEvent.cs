using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class PlaceBlockEvent : EventArgs
    {
        public readonly bool[,] BlockMatrix;
        // position of upper left corner of BlockMatrix
        public readonly Point Position;
        public readonly Action OnSuccess;

        public PlaceBlockEvent(bool[,] blockMatrix, Point position, Action onSuccess)
        {
            BlockMatrix = blockMatrix;
            Position = position;
            OnSuccess = onSuccess;
        }
    }
}