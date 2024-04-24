using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BlocksGame.MVC.Events
{
    public class UpdateChooseList : EventArgs
    {
        public readonly List<bool[,]> NewList;

        public UpdateChooseList(List<bool[,]> newList)
        {
            NewList = newList;
        }
    }
}