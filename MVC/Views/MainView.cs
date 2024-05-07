using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.Views;
using BlocksGame.MVC.Abstract;
using Microsoft.Xna.Framework.Input;
using BlocksGame.MVC.UI.Interfaces;

namespace BlocksGame.MVC.Views
{
    public class MainView : DependsOnState
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont TextFont { get; private set; }
        public SpriteFont ButtonFont { get; private set; }

        private Texture2D backgroundTexture;
        private readonly SplashScreenView splashScreenView;
        private readonly InGameView inGameView;
        private readonly List<IBaseUIElement> splashScreenUi;
        private readonly List<IBaseUIElement> inGameUi;

        public MainView(StateManager stateManager, GameModel game, GameCore core, 
                        List<IBaseUIElement> splashScreenUi, List<IBaseUIElement> inGameUi) : base(stateManager)
        {
            splashScreenView = new SplashScreenView(game, core, this, splashScreenUi);
            inGameView = new InGameView(game, core, this);
            this.splashScreenUi = splashScreenUi;
            this.inGameUi = inGameUi;

            game.OnUpdate += Update;
            core.OnLoad += Load;
            core.OnDraw += Draw;
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;
            SpriteBatch = new SpriteBatch(core.GraphicsDevice);
            TextFont = core.Content.Load<SpriteFont>("text_font");
            ButtonFont = core.Content.Load<SpriteFont>("button_font");
            backgroundTexture = core.Content.Load<Texture2D>("background");
        }

        private void Draw(object sender, EventArgs args)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GameCore.WindowWidth, GameCore.WindowHeight), Color.White);
            SpriteBatch.End();

            switch (StateManager.State)
            {
                case GameState.InGame:
                    inGameView.Draw(sender, args);
                    break;

                case GameState.SplashScreen:
                    splashScreenView.Draw(sender, args);
                    break;

                default:
                    throw new NotSupportedException($"Current state({StateManager.State}) is not supported by MainView or invalid!");
            }
        }

        private void Update(object sender, EventArgs args)
        {
            switch (StateManager.State)
            {
                case GameState.InGame:
                    inGameView.Update(sender, args);
                    break;

                case GameState.SplashScreen:
                    splashScreenView.Update(sender, args);
                    break;

                default:
                    throw new NotSupportedException($"Current state({StateManager.State}) is not supported or invalid!");
            }
        }
    }
}
