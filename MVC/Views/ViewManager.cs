using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Abstract;

namespace BlocksGame.MVC.Views
{
    public class ViewManager : DependsOnState
    {
        // common resources
        public static SpriteBatch SpriteBatch { get; private set; }
        public static SpriteFont TextFont { get; private set; }
        public static SpriteFont ButtonFont { get; private set; }

        private Texture2D backgroundTexture;
        private readonly List<StateDependentView> views;

        public ViewManager(StateManager stateManager, GameModel game, GameCore core, List<StateDependentView> viewList) : base(stateManager)
        {
            views = viewList;
            game.OnUpdate += Update;
            core.OnLoad += Load;
            core.OnDraw += Draw;
        }

        public void Reset()
        {
            foreach (var view in views)
                view.Reset();
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;
            if (SpriteBatch is null)
                SpriteBatch = new SpriteBatch(core.GraphicsDevice);
            if (TextFont is null)
                TextFont = core.Content.Load<SpriteFont>("text_font");
            if (ButtonFont is null)
                ButtonFont = core.Content.Load<SpriteFont>("button_font");
            backgroundTexture = core.Content.Load<Texture2D>("background");

            foreach (var view in views)
                view.Load(sender, args);
        }

        private void Draw(object sender, EventArgs args)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GameCore.WindowWidth, GameCore.WindowHeight), Color.White);
            SpriteBatch.End();

            foreach (var view in views)
            {
                if (view.ActiveState == StateManager.State)
                {
                    view.Draw(sender, args);
                    view.DrawUi();
                }
            }
        }

        private void Update(object sender, EventArgs args)
        {
            foreach (var view in views)
            {
                if (view.ActiveState == StateManager.State)
                    view.Update(sender, args);
            }
        }
    }
}
