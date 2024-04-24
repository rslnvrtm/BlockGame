using BlocksGame.MVC.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BlocksGame.MVC
{
    public delegate void OnEventCallback(object sender, EventArgs args);

    public class Controller
    {
        public event OnEventCallback OnUpdate;
        
        private List<bool[,]> blocksToChoose;
        private bool[,] pickedBlock;
        private Random random;

        public Controller(GameCore core)
        {
            blocksToChoose = new List<bool[,]>();
            random = new Random();
            core.OnInit += Initialize;
        }

        public void HandleClick(Point position)
        {
            if (position.X < GameCore.MapWidth * GameCore.BlockWidth)
                PlaceBlock();
            else
            {
                var blockIndex = position.Y / (GameCore.PreviewBlockWitdh * GameCore.MaxPreviewElementHeight);
                PickBlock(blockIndex);
            }
        }

        private void Initialize(object sender, EventArgs args) => UpdateBlocks();

        private void PickBlock(int index)
        {
            if (pickedBlock != null || index < 0 || index > GameCore.MaxCountToChoose - 1)
                return;

            pickedBlock = blocksToChoose[index];
            blocksToChoose[index] = null;
            OnUpdate(this, new PickBlockEvent(pickedBlock));

            if (blocksToChoose.All(el => el == null))
                UpdateBlocks();
        }

        private void PlaceBlock()
        {
            OnUpdate(this, new PlaceBlockEvent(
                pickedBlock, 
                new Point(GameCore.MousePos.X / GameCore.BlockWidth,
                          GameCore.MousePos.Y / GameCore.BlockWidth),
                () => pickedBlock = null)
            );
        }

        private void UpdateBlocks()
        {
            blocksToChoose.Clear();
            for (var i = 0; i < GameCore.MaxCountToChoose; i++)
                blocksToChoose.Add(BlockTemplates.AllLines[random.Next() % BlockTemplates.AllLines.Count]);

            OnUpdate(this, new UpdateChooseList(blocksToChoose));
        }
    }
}
