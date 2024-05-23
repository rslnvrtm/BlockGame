using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BlocksGame.MVC.Events
{
    public class UpdateChooseListEvent : EventArgs
    {
        public readonly List<BlockType[,]> NewList;

        public UpdateChooseListEvent(List<BlockType[,]> newList)
        {
            NewList = newList;
        }
    }
}