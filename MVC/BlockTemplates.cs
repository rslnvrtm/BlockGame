using System.Collections.Generic;

namespace BlocksGame
{
    public static class BlockTemplates
    {
        // todo add more blocks
        public static readonly bool[,] Dot = { { true } };
        public static readonly bool[,] LineTwo = { { true, true } };
        public static readonly bool[,] LineThree = { { true, true, true } };
        public static readonly bool[,] LineFour = { { true, true, true, true } };
        public static readonly bool[,] LineFive = { { true, true, true, true, true } };
        public static readonly bool[,] LineWithRightDot =
        {
            { true, true },
            { true, false },
            { true, false }
        };
        public static readonly bool[,] SmallCorner =
         {
            { true, true },
            { true, false }
        };
        public static readonly bool[,] BigCorner =
        {
            { true, true, true },
            { true, false, false },
            { true, false, false },
        };
        public static readonly bool[,] SquareTwo =
        {
            { true, true },
            { true, true }
        }; 
        public static readonly bool[,] SquareThree =
         {
            { true, true, true },
            { true, true, true },
            { true, true, true }
        };

        public static List<bool[,]> AllTemplates => new List<bool[,]> 
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
            SquareThree
        };
    }
}