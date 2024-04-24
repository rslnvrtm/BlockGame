using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Models;
using BlocksGame.MVC.Events;
using Microsoft.Xna.Framework.Input;

namespace BlocksGame.MVC
{
    public class View
    {
        private Texture2D blockTexture;
        private SpriteBatch blockSprite;
        private Texture2D cellTexture;
        private SpriteBatch cellSprite;
        private bool[,] renderMap;
        private readonly int renderMapWidth;
        private readonly int renderMapHeight;
        private bool[,] pickedBlockMatrix;
        private List<bool[,]> chooseList;

        public View(GameModel game, GameCore core)
        {
            renderMap = new bool[game.MapHeight, game.MapWidth];
            renderMapWidth = game.MapWidth;
            renderMapHeight = game.MapHeight;
            pickedBlockMatrix = null;
            chooseList = null;
            game.OnUpdate += Update;
            core.OnLoad += Load;
            core.OnDraw += Draw;
            core.OnInit += Init;
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;

            blockSprite = new SpriteBatch(core.GraphicsDevice);
            blockTexture = core.Content.Load<Texture2D>("block");
            cellSprite = new SpriteBatch(core.GraphicsDevice);
            cellTexture = core.Content.Load<Texture2D>("empty_cell");
        }

        private void Draw(object sender, EventArgs args)
        {
            // todo place in DrawMap
            for (var x = 0; x < renderMapWidth; x++)
            {
                for (var y = 0; y < renderMapHeight; y++)
                {
                    if (!renderMap[y, x])
                    {
                        cellSprite.Begin();
                        cellSprite.Draw(cellTexture, new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                        cellSprite.End();
                        continue;
                    }
                    
                    blockSprite.Begin();
                    blockSprite.Draw(blockTexture, new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                    blockSprite.End();
                }
            }

            // todo place in DrawChooseList
            for (var i = 0; i < chooseList.Count; i++)
            {
                if (chooseList[i] == null)
                    continue;

                var yLength = chooseList[i].GetLength(0);
                var xLength = chooseList[i].GetLength(1);
                for (var x = 0; x < xLength; x++)
                {
                    for (var y = 0; y < yLength; y++)
                    {
                        if (!chooseList[i][y, x])
                            continue;

                        blockSprite.Begin();
                        blockSprite.Draw(
                            blockTexture,
                            new Rectangle(
                                (GameCore.MapWidth + 1) * GameCore.BlockWidth + x * GameCore.PreviewBlockWitdh, 
                                GameCore.BlockWidth + GameCore.PreviewBlockWitdh * GameCore.MaxPreviewElementHeight * i + y * GameCore.PreviewBlockWitdh,
                                GameCore.PreviewBlockWitdh, GameCore.PreviewBlockWitdh
                            ), 
                            new Rectangle(0, 0, 50, 50),
                            Color.White
                        );
                        blockSprite.End();
                    }
                }
            }

            // todo place in DrawPickedBlock
            if (pickedBlockMatrix != null)
            {
                var yLength = pickedBlockMatrix.GetLength(0);
                var xLength = pickedBlockMatrix.GetLength(1);
                for (var x = 0; x < xLength; x++)
                {
                    for (var y = 0; y < yLength; y++)
                    {
                        if (!pickedBlockMatrix[y, x])
                            continue;

                        blockSprite.Begin();
                        blockSprite.Draw(blockTexture, new Vector2(GameCore.MousePos.X + x * GameCore.BlockWidth, GameCore.MousePos.Y + y * GameCore.BlockWidth), Color.White);
                        blockSprite.End();
                    }
                }
            }
        }

        private void Init(object sender, EventArgs args)
        {
            // ...
        }
        private void Update(object sender, EventArgs args)
        {
            var game = (GameModel)sender;
            if (args is UpdateMapEvent)
            {
                pickedBlockMatrix = null;
                var updateMap = (UpdateMapEvent)args;
                for (var x = 0; x < renderMapWidth; x++)
                {
                    for (var y = 0; y < renderMapHeight; y++)
                        renderMap[y, x] = updateMap.Map[y, x];
                }
            }

            if (args is PickBlockEvent)
                pickedBlockMatrix = ((PickBlockEvent)args).BlockMatrix;

            if (args is UpdateChooseList)
                chooseList = ((UpdateChooseList)args).NewList;
        }
    }
}
