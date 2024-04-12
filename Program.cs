namespace BlocksGame
{
    public class Program
    {
        public static void Main()
        {
            using var game = new BlocksGame.GameCore();
            game.Run();
        }
    }
}