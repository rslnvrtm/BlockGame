using System.Collections.Generic;

namespace BlocksGame
{
    public static class BlockTemplates
    {
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

        public static List<bool[,]> AllLines => new List<bool[,]> { Dot, LineTwo, LineThree, LineFour, LineFive, LineWithRightDot };
    }
}