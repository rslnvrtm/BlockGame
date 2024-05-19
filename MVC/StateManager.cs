namespace BlocksGame.MVC
{
    public enum GameState
    {
        SplashScreen,
        InGame
    }

    public class StateManager
    {
        public event OnEventCallback OnStateChange;

        private readonly object locker = new object();
        private GameState state = GameState.SplashScreen;

        public GameState State
        { 
            get
            {
                lock (locker)
                    return state;
            }
            set
            {
                lock (locker)
                {
                    if (state == value)
                        return;

                    state = value;
                    OnStateChange(this, null);
                }
            }
        }
    }
}
