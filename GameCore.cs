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
        public event OnEventCallback OnInit;
        public event OnEventCallback OnLoad;
        public event OnEventCallback OnUpdate;
        public event OnEventCallback OnDraw;

        private GraphicsDeviceManager deviceManager;
        private readonly GameModel gameModel;
        private readonly View view;
        private readonly Controller controller;
        public GameCore()
        {
            deviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            controller = new Controller();
            gameModel = new GameModel(controller, 8, 8);
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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