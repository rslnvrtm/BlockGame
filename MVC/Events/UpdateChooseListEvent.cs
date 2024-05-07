using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BlocksGame.MVC.Events
{
    public class UpdateChooseListEvent : EventArgs
    {
        public readonly List<bool[,]> NewList;

        public UpdateChooseListEvent(List<bool[,]> newList)
        {
            NewList = newList;
        }
    }
}