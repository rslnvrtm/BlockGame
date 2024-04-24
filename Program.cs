namespace BlocksGame
{
    public class Program
    {
        public static void Main()
        {
            using var game = new GameCore();
            game.Run();
        }
    }
}