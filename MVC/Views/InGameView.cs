using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.UI.Interfaces;

namespace BlocksGame.MVC.Views
{
    public class InGameView : StateDependentView
    {
        private Texture2D blockTexture;
        private Texture2D cellTexture;
        private bool[,] renderMap;
        private readonly int renderMapWidth;
        private readonly int renderMapHeight;
        private bool[,] pickedBlockMatrix;
        private List<bool[,]> chooseList;
        private int score;
        private Vector2 offset;
        private Vector2 pickedDrawOffset;
        private int highScore;

        public InGameView(List<IBaseUIElement> drawList)
            : base(GameState.InGame, drawList)
        {
            renderMap = new bool[GameCore.MapHeight, GameCore.MapWidth];
            renderMapWidth = GameCore.MapWidth;
            renderMapHeight = GameCore.MapHeight;
            pickedBlockMatrix = null;
            chooseList = null;
            score = 0;
            offset = new Vector2(GameCore.DrawOffsetX, GameCore.DrawOffsetY);
            pickedDrawOffset = Vector2.Zero;
            highScore = 0;
        }
        
        public override void Reset()
        {
            renderMap = new bool[GameCore.MapHeight, GameCore.MapWidth];
            pickedBlockMatrix = null;
            chooseList = null;
            score = 0;
            pickedDrawOffset = Vector2.Zero;
        }

        public override void Draw(object sender, EventArgs args)
        {
            ViewManager.SpriteBatch.Begin();
            ViewManager.SpriteBatch.DrawString(ViewManager.TextFont, score.ToString(), 
                                                offset + new Vector2(GameCore.MapWidth / 2 * GameCore.BlockWidth, GameCore.MapHeight * GameCore.BlockWidth), 
                                                Color.Red);
            ViewManager.SpriteBatch.DrawString(ViewManager.TextFont, highScore.ToString(), 
                                                offset + new Vector2(GameCore.MapWidth / 2 * GameCore.BlockWidth, GameCore.MapHeight * (GameCore.BlockWidth + 4)), 
                                                Color.Green);
            ViewManager.SpriteBatch.End();
            
            DrawMap();
            DrawChooseList();
            DrawBlock(pickedBlockMatrix,
                      (x, y) => new Vector2(
                          GameCore.MousePos.X + x * GameCore.BlockWidth + pickedDrawOffset.X, 
                          GameCore.MousePos.Y + y * GameCore.BlockWidth + pickedDrawOffset.Y));
        }

        public override void DrawUi()
        {
            foreach (var el in UiElements)
                el.Draw(ViewManager.SpriteBatch, ViewManager.ButtonFont);
        }

        public override void Update(object sender, EventArgs args)
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

            if (args is PickBlockEvent pickBlockEvent)
            {
                pickedBlockMatrix = pickBlockEvent.BlockMatrix;
                var yLength = pickedBlockMatrix.GetLength(0) * GameCore.BlockWidth;
                var xLength = pickedBlockMatrix.GetLength(1) * GameCore.BlockWidth;
                pickedDrawOffset = new Vector2(-xLength / 2, -yLength / 2);
            }

            if (args is UnpickBlockEvent)
                pickedBlockMatrix = null;

            if (args is UpdateChooseListEvent  updateChooseListEvent)
                chooseList = updateChooseListEvent.NewList;

            if (args is UpdateScoreEvent updateScoreEvent)
                score = updateScoreEvent.Score;

            if (args is GameOverEvent gameOverEvent)
                highScore = Math.Max(highScore, gameOverEvent.Score);
        }

        public override void Load(object sender, EventArgs args)
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
                        ViewManager.SpriteBatch.Begin();
                        ViewManager.SpriteBatch.Draw(cellTexture, offset + new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                        ViewManager.SpriteBatch.End();
                        continue;
                    }

                    ViewManager.SpriteBatch.Begin();
                    ViewManager.SpriteBatch.Draw(blockTexture, offset + new Vector2(x * GameCore.BlockWidth, y * GameCore.BlockWidth), Color.White);
                    ViewManager.SpriteBatch.End();
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

                    ViewManager.SpriteBatch.Begin();
                    if (calculateSize is null)
                        ViewManager.SpriteBatch.Draw(blockTexture, calculatePosition(x, y), Color.White);
                    else
                    {
                        var (dst, src) = calculateSize(x, y);
                        ViewManager.SpriteBatch.Draw(blockTexture, dst, src, Color.White);
                    }

                    ViewManager.SpriteBatch.End();
                }
            }
        }
    }
}
