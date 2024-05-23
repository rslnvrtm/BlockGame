using Microsoft.Xna.Framework;
using System;

namespace BlocksGame.MVC.Events
{
    public class DisplayHintEvent : EventArgs 
    {
        public readonly Point position;
        public readonly BlockType[,] matrix;

        public DisplayHintEvent(Point position, BlockType[,] matrix)
        {
            this.position = position;
            this.matrix = matrix;
        }
    }
}