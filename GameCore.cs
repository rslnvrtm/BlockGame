using BlocksGame.MVC;
using BlocksGame.MVC.UI;
using BlocksGame.MVC.UI.Interfaces;
using BlocksGame.MVC.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BlocksGame
{
    public class GameCore : Game
    {
        public const int MaxCountToChoose = 3;
        public const int BlockWidth = 50; // px
        public const int PreviewBlockWitdh = 25; // px
        public const int MaxPreviewElementHeight = 5; // in preview blocks
        public const int MapWidth = 8; // in blocks
        public const int MapHeight = 8; // in blocks
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;
        public const int DrawOffsetX = WindowWidth / 4;
        public const int DrawOffsetY = WindowHeight / 4;
        public static Point MousePos;

        public event OnEventCallback OnInit;
        public event OnEventCallback OnLoad;
        public event OnEventCallback OnUpdate;
        public event OnEventCallback OnDraw;

        private GraphicsDeviceManager deviceManager;
        private readonly StateManager stateManager;
        private readonly GameModel gameModel;
        private readonly ViewManager view;
        private readonly Controller controller;
        private ButtonState previousMouseState;

        public GameCore()
        {
            deviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            previousMouseState = ButtonState.Released;

            var viewList = CreateUi();

            stateManager = new StateManager();
            controller = new Controller(stateManager, this, viewList);
            gameModel = new GameModel(stateManager, controller, this, MapWidth, MapHeight);
            view = new ViewManager(stateManager, gameModel, this, viewList);
        }

        private List<StateDependentView> CreateUi()
        {
            var splashScreenUi = new List<IBaseUIElement>();
            var inGameUi = new List<IBaseUIElement>();

            // splashscreen
            var playButton = new Button(this, "button", "PLAY", new Rectangle(WindowWidth / 2 - 100, WindowHeight / 2, 200, 60));
            var exitButton = new Button(this, "button", "EXIT", new Rectangle(WindowWidth / 2 - 100, WindowHeight / 2 + 80, 200, 60));
            playButton.OnClick += (_, _) => stateManager.State = GameState.InGame;
            exitButton.OnClick += (_, _) => Exit();
            splashScreenUi.Add(playButton);
            splashScreenUi.Add(exitButton);

            // ingame
            var restartButton = new Button(this, "restart", null, new Rectangle(WindowWidth - 50, 10, 40, 40));
            restartButton.OnClick += (_, _) => Restart();
            inGameUi.Add(restartButton);

            return new List<StateDependentView>
            {
                new SplashScreenView(splashScreenUi),
                new InGameView(inGameUi)
            };
        }

        public void Restart()
        {
            view.Reset();
            controller.Reset();
            gameModel.Reset();
        }

        protected override void Initialize()
        {
            deviceManager.IsFullScreen = false;
            deviceManager.PreferredBackBufferWidth = 1280;
            deviceManager.PreferredBackBufferHeight = 720;
            deviceManager.ApplyChanges();

            base.Initialize();
            if (OnInit != null)
                OnInit(this, null); 
        }

        protected override void LoadContent()
        {
            if (OnLoad != null)
                OnLoad(this, null);
        }

        protected override void Update(GameTime gameTime)
        {
            if (OnUpdate != null)
                OnUpdate(this, null);

            var mouseState = Mouse.GetState();
            MousePos = mouseState.Position;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            
            if (OnDraw != null)
                OnDraw(this, null);

            base.Draw(gameTime);
        }
    }
}