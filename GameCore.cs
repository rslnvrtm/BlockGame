using BlocksGame.MVC;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using BlocksGame.MVC.Models;

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
        public static Point MousePos;

        public event OnEventCallback OnInit;
        public event OnEventCallback OnLoad;
        public event OnEventCallback OnUpdate;
        public event OnEventCallback OnDraw;

        private GraphicsDeviceManager deviceManager;
        private readonly GameModel gameModel;
        private readonly View view;
        private readonly Controller controller;
        private ButtonState previousMouseState;
        public GameCore()
        {
            deviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            previousMouseState = ButtonState.Released;

            controller = new Controller(this);
            gameModel = new GameModel(controller, MapWidth, MapHeight);
            view = new View(gameModel, this);
        }

        protected override void Initialize()
        {
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

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState == ButtonState.Pressed)
                controller.HandleClick(mouseState.Position);

            previousMouseState = mouseState.LeftButton;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            if (OnDraw != null)
                OnDraw(this, null);

            base.Draw(gameTime);
        }
    }
}