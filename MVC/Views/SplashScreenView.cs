using System;
using System.Collections.Generic;
using BlocksGame.MVC.UI.Interfaces;
using BlocksGame.MVC.Views;

namespace BlocksGame.MVC
{
    public class SplashScreenView : StateDependentView
    {
        public SplashScreenView(List<IBaseUIElement> drawList)
            : base(GameState.SplashScreen, drawList) {}

        public override void Draw(object sender, EventArgs args) {}

        public override void Reset() {}

        public override void DrawUi()
        {
            foreach (var el in UiElements)
                el.Draw(ViewManager.SpriteBatch, ViewManager.ButtonFont);
        }

        public override void Update(object sender, EventArgs args) {}

        public override void Load(object sender, EventArgs args) {}
    }
}
