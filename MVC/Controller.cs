using BlocksGame.MVC.Events;
using BlocksGame.MVC.Abstract;
using BlocksGame.MVC.UI.Abstract;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BlocksGame.MVC.UI.Interfaces;

namespace BlocksGame.MVC
{
    public delegate void OnEventCallback(object sender, EventArgs args);

    public class Controller : DependsOnState
    {
        public event OnEventCallback OnUpdate;
        
        private List<bool[,]> blocksToChoose;
        private bool[,] pickedBlock;
        private int pickedBlockIndex;
        private Random random;
        private ButtonState previousMouseState;
        private readonly List<IBaseUIElement> splashScreenUi;
        private readonly List<IBaseUIElement> inGameUi;

        public Controller(StateManager stateManager, GameCore core, 
                          List<IBaseUIElement> splashScreenUi, List<IBaseUIElement> inGameUi) : base(stateManager)
        {
            blocksToChoose = new List<bool[,]>();
            random = new Random();
            previousMouseState = ButtonState.Released;
            this.splashScreenUi = splashScreenUi;
            this.inGameUi = inGameUi;
            StateManager.OnStateChange += Initialize;
            core.OnUpdate += Update;
        }

        private void Update(object sender, EventArgs args)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState == ButtonState.Pressed)
                HandleClick(mouseState.Position);

            previousMouseState = mouseState.LeftButton;
        }

        public void HandleClick(Point position)
        {
            switch (StateManager.State)
            {
                case GameState.InGame:
                    HandleClickInGame(position);
                    break;

                case GameState.SplashScreen:
                    HandleClickSplashScreen(position);
                    break;

                default:
                    throw new NotSupportedException($"Current state({StateManager.State}) is not supported by controller or invalid!");
            }
        }

        private void HandleClickSplashScreen(Point position)
        {
            if (HandleUiClick(position, splashScreenUi))
                return;
        }

        private void HandleClickInGame(Point position)
        {
            if (HandleUiClick(position, inGameUi))
                return;

            if (position.X < GameCore.MapWidth * GameCore.BlockWidth + GameCore.DrawOffsetX)
                PlaceBlock();
            else
            {
                var blockIndex = (position.Y - GameCore.DrawOffsetY) / (GameCore.PreviewBlockWitdh * GameCore.MaxPreviewElementHeight);
                PickBlock(blockIndex);
            }
        }

        private bool HandleUiClick(Point position, List<IBaseUIElement> ui)
        {
            foreach (var el in ui)
            {
                if (el is not UIHandlesClick)
                    continue;
                
                if (((UIHandlesClick)el).HandleClick(position))
                    return true;
            }

            return false;
        }

        private void Initialize(object sender, EventArgs args) => UpdateBlocks();

        private void PickBlock(int index)
        {
            if (pickedBlock != null)
            {
                blocksToChoose[pickedBlockIndex] = pickedBlock;
                pickedBlock = null;
                pickedBlockIndex = -1;
                OnUpdate(this, new UnpickBlockEvent());
                return;
            }

            if (index < 0 || index > GameCore.MaxCountToChoose - 1)
                return;

            pickedBlock = blocksToChoose[index];
            blocksToChoose[index] = null;
            pickedBlockIndex = index;
            OnUpdate(this, new PickBlockEvent(pickedBlock));
        }

        private void PlaceBlock()
        {
            OnUpdate(this, new PlaceBlockEvent(
                pickedBlock, 
                new Point((GameCore.MousePos.X - GameCore.DrawOffsetX) / GameCore.BlockWidth,
                          (GameCore.MousePos.Y - GameCore.DrawOffsetY) / GameCore.BlockWidth),
                () => 
                {
                    pickedBlock = null;
                    UpdateBlocks();
                })
            );
        }

        public void UpdateBlocks()
        {
            if (!blocksToChoose.All(el => el == null))
                return;

            blocksToChoose.Clear();
            for (var i = 0; i < GameCore.MaxCountToChoose; i++)
                blocksToChoose.Add(BlockTemplates.AllTemplates[random.Next() % BlockTemplates.AllTemplates.Count]);

            OnUpdate(this, new UpdateChooseListEvent(blocksToChoose));
        }
    }
}
