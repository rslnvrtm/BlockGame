using System;
using System.Collections.Generic;

namespace BlocksGame.MVC.Events
{
    public class GetHintEvent : EventArgs 
    {
        public readonly List<BlockType[,]> blocksToChoose;

        public GetHintEvent(List<BlockType[,]> blocks = null)
            => blocksToChoose = blocks;
    }
}