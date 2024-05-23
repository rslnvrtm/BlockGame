using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BlocksGame.MVC.Helpers.Interfaces;

namespace BlocksGame.MVC.Helpers
{
    public class BasicHintProvider : IHintProvider
    {
        public (Point position, BlockType[,] block)? GetHint(BlockType[,] map, List<BlockType[,]> blocks)
        {
            var height = map.GetLength(0);
            var width = map.GetLength(1);

            foreach (var block in blocks)
            {
                if (block is null)
                    continue;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var position = new Point(x, y);
                        if (CanPlace(map, position, block))
                            return (position, block);
                    }
                }
            }

            return null;
        }

        private bool CanPlace(BlockType[,] map, Point position, BlockType[,] matrix)
        {
            if (matrix == null || position.X < 0 || position.Y < 0)
                return false;

            var height = map.GetLength(0);
            var width = map.GetLength(1);
            var yLength = matrix.GetLength(0);
            var xLength = matrix.GetLength(1);

            if (position.X + xLength - 1 >= width || position.X + xLength - 1 < 0
                || position.Y + yLength - 1 >= height || position.Y + yLength - 1 < 0)
                return false;

            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    if (matrix[y, x] == BlockType.None)
                        continue;

                    if (map[position.Y + y, position.X + x] != BlockType.None)
                        return false;
                }
            }

            return true;
        }
    }
}