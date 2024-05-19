using System;
using System.Collections.Generic;
using BlocksGame.MVC.UI.Interfaces;

namespace BlocksGame.MVC.Views
{
    public abstract class StateDependentView : IView
    {
        public readonly GameState ActiveState;
        public readonly List<IBaseUIElement> UiElements;

        public StateDependentView(GameState state, List<IBaseUIElement> uiElements)
        {
            ActiveState = state;
            UiElements = uiElements;
        }

        public abstract void Reset();
        public abstract void Draw(object sender, EventArgs args);
        public abstract void DrawUi();
        public abstract void Update(object sender, EventArgs args);
        public abstract void Load(object sender, EventArgs args);
    }
}
