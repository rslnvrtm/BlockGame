using System.Collections.Generic;

namespace BlocksGame
{
    public enum BlockType
    {
        None,
        Normal,
        Bomb
    }

    public static class BlockTemplates
    {
        // todo add more blocks
        public static readonly BlockType[,] Dot = { { BlockType.Normal } };
        public static readonly BlockType[,] LineTwo = { { BlockType.Normal, BlockType.Normal } };
        public static readonly BlockType[,] LineThree = { { BlockType.Normal, BlockType.Normal, BlockType.Normal } };
        public static readonly BlockType[,] LineFour = { { BlockType.Normal, BlockType.Normal, BlockType.Normal, BlockType.Normal } };
        public static readonly BlockType[,] LineFive = { { BlockType.Normal, BlockType.Normal, BlockType.Normal, BlockType.Normal, BlockType.Normal } };
        public static readonly BlockType[,] LineWithRightDot =
        {
            { BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.None },
            { BlockType.Normal, BlockType.None }
        };
        public static readonly BlockType[,] SmallCorner =
         {
            { BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.None }
        };
        public static readonly BlockType[,] BigCorner =
        {
            { BlockType.Normal, BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.None, BlockType.None },
            { BlockType.Normal, BlockType.None, BlockType.None },
        };
        public static readonly BlockType[,] SquareTwo =
        {
            { BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.Normal }
        }; 
        public static readonly BlockType[,] SquareThree =
         {
            { BlockType.Normal, BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.Normal, BlockType.Normal },
            { BlockType.Normal, BlockType.Normal, BlockType.Normal }
        };
        public static readonly BlockType[,] Bomb = { { BlockType.Bomb } };

        public static List<BlockType[,]> AllTemplates => new List<BlockType[,]> 
        { 
            Dot, 
            LineTwo, 
            LineThree, 
            LineFour, 
            LineFive, 
            LineWithRightDot,
            SmallCorner,
            BigCorner,
            SquareTwo,
            SquareThree,
            Bomb
        };
    }
}