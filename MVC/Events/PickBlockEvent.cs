using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class PickBlockEvent : EventArgs
    {
        public readonly bool[,] BlockMatrix;

        public PickBlockEvent(bool[,] blockMatrix)
        {
            BlockMatrix = blockMatrix;
        }
    }
}