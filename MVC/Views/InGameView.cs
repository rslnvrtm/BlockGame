using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.Abstract;
using Microsoft.Xna.Framework.Input;
using BlocksGame.MVC.UI.Interfaces;

namespace BlocksGame.MVC.Views
{
    public class InGameView
    {
        private readonly MainView mainView;
        private Texture2D blockTexture;
        private Texture2D cellTexture;
        private bool[,] renderMap;
        private readonly int renderMapWidth;
        private readonly int renderMapHeight;
        private bool[,] pickedBlockMatrix;
        private List<bool[,]> chooseList;
        private int score;
        private Vector2 offset;

        public InGameView(GameModel game, GameCore core, MainView view)
        {
            renderMap = new bool[game.MapHeight, game.MapWidth];
            renderMapWidth = game.MapWidth;
            renderMapHeight = game.MapHeight;
            pickedBlockMatrix = null;
            chooseList = null;
            score = 0;
            mainView = view;
            offset = new Vector2(GameCore.DrawOffsetX, GameCore.DrawOffsetY);
            core.OnLoad += Load;
        }

        public void Draw(object sender, EventArgs args)
        {
            mainView.SpriteBatch.Begin();
            mainView.SpriteBatch.DrawString(mainView.TextFont, score.ToString(), offset + new Vector2((GameCore.MapWidth / 2) * GameCore.BlockWidth, GameCore.MapHeight * GameCore.BlockWidth), Color.Red);
            mainView.SpriteBatch.End();
            
            DrawMap();
            DrawChooseList();
            DrawBlock(pickedBlockMatrix, (x, y) => new Vector2(GameCore.MousePos.X + x * GameCore.BlockWidth, GameCore.MousePos.Y + y * GameCore.BlockWidth));
        }

        public void Update(object sender, EventArgs args)
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

            if (args is UnpickBlockEvent)
                pickedBlockMatrix = null;

            if (args is UpdateChooseListEvent)
                chooseList = ((UpdateChooseListEvent)args).NewList;

            if (args is UpdateScoreEvent)
                score = ((UpdateScoreEvent)args).Score;
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;

            blockTexture = core.Content.Load<Texture2D>("block2");
            cellTexture = core.Content.Load<Texture2D>("empty_cell");
        }

        private void DrawMap()
        {
            for (var x = 0; x < renderMapWidth; x++)
            {
                for (var y = 0; y < renderMapHeight; y++)
                {
                    if (!renderMap[y, x])
                    {
                        mainView.SpriteBatch.Begin();
                        mainView.SpriteBatch.Draw(cellTexture, offset + new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                        mainView.SpriteBatch.End();
                        continue;
                    }

                    mainView.SpriteBatch.Begin();
                    mainView.SpriteBatch.Draw(blockTexture, offset + new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                    mainView.SpriteBatch.End();
                }
            }
        }

        private void DrawChooseList()
        {
            for (var i = 0; i < chooseList.Count; i++)
            {
                if (chooseList[i] == null)
                    continue;

                DrawBlock(chooseList[i], null, (x, y) => (
                    new Rectangle(
                        (int)offset.X + (GameCore.MapWidth + 1) * GameCore.BlockWidth + x * GameCore.PreviewBlockWitdh,
                        (int)offset.Y + GameCore.BlockWidth + GameCore.PreviewBlockWitdh * GameCore.MaxPreviewElementHeight * i + y * GameCore.PreviewBlockWitdh,
                        GameCore.PreviewBlockWitdh, GameCore.PreviewBlockWitdh),
                    new Rectangle(0, 0, 50, 50)
                ));
            }
        }

        private void DrawBlock(bool[,] blockMatrix,
                               Func<int, int, Vector2> calculatePosition,
                               Func<int, int, (Rectangle dst, Rectangle src)> calculateSize = null)
        {
            if (blockMatrix is null)
                return;

            if (calculatePosition is null && calculateSize is null)
                return;

            var yLength = blockMatrix.GetLength(0);
            var xLength = blockMatrix.GetLength(1);
            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    if (!blockMatrix[y, x])
                        continue;

                    mainView.SpriteBatch.Begin();
                    if (calculateSize is null)
                        mainView.SpriteBatch.Draw(blockTexture, calculatePosition(x, y), Color.White);
                    else
                    {
                        var (dst, src) = calculateSize(x, y);
                        mainView.SpriteBatch.Draw(blockTexture, dst, src, Color.White);
                    }

                    mainView.SpriteBatch.End();
                }
            }
        }
    }
}
