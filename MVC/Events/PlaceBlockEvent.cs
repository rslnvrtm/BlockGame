using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class PlaceBlockEvent : EventArgs
    {
        public readonly BlockType[,] BlockMatrix;
        // position of upper left corner of BlockMatrix(in blocks)
        public readonly Point Position;
        public readonly Action OnSuccess;

        public PlaceBlockEvent(BlockType[,] blockMatrix, Point position, Action onSuccess)
        {
            BlockMatrix = blockMatrix;
            Position = position;
            OnSuccess = onSuccess;
        }
    }
}