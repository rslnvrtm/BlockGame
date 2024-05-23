using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class PickBlockEvent : EventArgs
    {
        public readonly BlockType[,] BlockMatrix;

        public PickBlockEvent(BlockType[,] blockMatrix)
            => BlockMatrix = blockMatrix;
    }
}