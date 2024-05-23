using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BlocksGame.MVC.Helpers.Interfaces
{
    public interface IHintProvider
    {
        public (Point position, BlockType[,] block)? GetHint(BlockType[,] map, List<BlockType[,]> blocks);
    }
}