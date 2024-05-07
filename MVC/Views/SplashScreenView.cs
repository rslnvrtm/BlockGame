using System;
using System.Collections.Generic;
using BlocksGame.MVC.UI.Interfaces;
using BlocksGame.MVC.Views;

namespace BlocksGame.MVC
{
    public class SplashScreenView
    {
        private readonly MainView mainView;
        private List<IBaseUIElement> ui;

        public SplashScreenView(GameModel game, GameCore core, MainView view, List<IBaseUIElement> splashScreenUi)
        {
            mainView = view;
            ui = splashScreenUi;
            core.OnLoad += Load;
        }

        public void Draw(object sender, EventArgs args)
        {
            DrawUi(ui);
        }

        public void Update(object sender, EventArgs args)
        {
            var game = (GameModel)sender;
            // nothing to dispatch yet...
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;
            // nothing to load yet...
        }

        private void DrawUi(List<IBaseUIElement> ui)
        {
            foreach (var el in ui)
                el.Draw(mainView.SpriteBatch, mainView.ButtonFont);
        }
    }
}
