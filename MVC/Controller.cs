using BlocksGame.MVC.Events;
using BlocksGame.MVC.Abstract;
using BlocksGame.MVC.UI.Abstract;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BlocksGame.MVC.UI.Interfaces;
using BlocksGame.MVC.Views;

namespace BlocksGame.MVC
{
    public delegate void OnEventCallback(object sender, EventArgs args);

    public class Controller : DependsOnState
    {
        public event OnEventCallback OnUpdate;
        
        private List<BlockType[,]> blocksToChoose;
        private BlockType[,] pickedBlock;
        private int pickedBlockIndex;
        private Random random;
        private ButtonState previousMouseState;
        private readonly List<StateDependentView> views;

        public Controller(StateManager stateManager, GameCore core, List<StateDependentView> viewList) 
            : base(stateManager)
        {
            blocksToChoose = new List<BlockType[,]>();
            random = new Random();
            previousMouseState = ButtonState.Released;
            views = viewList;
            StateManager.OnStateChange += Initialize;
            core.OnUpdate += Update;
        }

        public void Reset()
        {
            blocksToChoose = new List<BlockType[,]>();
            previousMouseState = ButtonState.Released;
            UpdateBlocks();
        }

        private void Update(object sender, EventArgs args)
        {
            // GetHintEvent initially goes w/o blocks list since fired through button callback
            if (args is GetHintEvent getHintEvent)
            {
                OnUpdate(this, new GetHintEvent(blocksToChoose));
                return;
            }
            
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Released && previousMouseState == ButtonState.Pressed)
                HandleClick(mouseState.Position);

            previousMouseState = mouseState.LeftButton;
        }

        public void HandleClick(Point position)
        {
            foreach (var view in views)
            {
                if (view.ActiveState == StateManager.State)
                {
                    if (HandleUiClick(position, view.UiElements))
                        return;
                    break;
                }
            }

            if (StateManager.State != GameState.InGame)
                return;

            if (position.X < GameCore.MapWidth * GameCore.BlockWidth + GameCore.DrawOffsetX)
            {
                if (pickedBlock is not null)
                    PlaceBlock();
            }
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
                if (el is UIHandlesClick handlesClick)
                {
                    if (handlesClick.HandleClick(position))
                        return true;
                }
            }

            return false;
        }

        private void Initialize(object sender, EventArgs args) => UpdateBlocks();

        private void PickBlock(int index)
        {
            if (pickedBlock is not null)
            {
                blocksToChoose[pickedBlockIndex] = pickedBlock;
                pickedBlock = null;
                pickedBlockIndex = -1;
                OnUpdate(this, new UnpickBlockEvent());
                return;
            }

            if (index < 0 || index > GameCore.MaxCountToChoose - 1 || blocksToChoose[index] is null)
                return;

            pickedBlock = blocksToChoose[index];
            blocksToChoose[index] = null;
            pickedBlockIndex = index;
            OnUpdate(this, new PickBlockEvent(pickedBlock));
        }

        private void PlaceBlock()
        {
            var yOffset = -pickedBlock.GetLength(0) * GameCore.BlockWidth / 2;
            var xOffset = -pickedBlock.GetLength(1) * GameCore.BlockWidth / 2;
            
            var xPosition = (int)Math.Round((GameCore.MousePos.X - GameCore.DrawOffsetX + xOffset) / (float)GameCore.BlockWidth, 0);
            var yPosition = (int)Math.Round((GameCore.MousePos.Y - GameCore.DrawOffsetY + yOffset) / (float)GameCore.BlockWidth, 0);


            OnUpdate(this, new PlaceBlockEvent(
                pickedBlock, 
                new Point(xPosition, yPosition),
                () => 
                {
                    pickedBlock = null;
                    UpdateBlocks();
                })
            );

            OnUpdate(this, new GameOverCheckEvent(blocksToChoose));
        }

        public void UpdateBlocks()
        {
            if (!blocksToChoose.All(el => el == null))
                return;

            blocksToChoose.Clear();
            for (var i = 0; i < GameCore.MaxCountToChoose; i++)
                blocksToChoose.Add(BlockTemplates.AllTemplates[random.Next() % BlockTemplates.AllTemplates.Count]);

            OnUpdate(this, new UpdateChooseListEvent(blocksToChoose));
            OnUpdate(this, new GameOverCheckEvent(blocksToChoose));
        }
    }
}
