namespace BlocksGame.MVC.Abstract
{
    public abstract class DependsOnState
    {
        protected readonly StateManager StateManager;

        protected DependsOnState(StateManager stateManager)
            => StateManager = stateManager;
    }
}