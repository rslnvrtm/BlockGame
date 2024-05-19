using System;
using System.Collections.Generic;

namespace BlocksGame.MVC.Events
{
    public class GameOverCheckEvent : EventArgs 
    {
        public readonly List<bool[,]> ChooseList;

        public GameOverCheckEvent(List<bool[,]> chooseList)
            => ChooseList = chooseList;
    }
}
