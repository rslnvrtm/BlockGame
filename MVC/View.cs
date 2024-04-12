using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Models;
using BlocksGame.MVC.Events;

namespace BlocksGame.MVC
{
    public class View
    {
        private Texture2D blockTexture;
        private SpriteBatch blockSprite;
        private bool[,] renderMap;
        private readonly int renderMapWidth;
        private readonly int renderMapHeight;

        public View(GameModel game, GameCore core)
        {
            renderMap = new bool[game.MapHeight, game.MapWidth];
            renderMapWidth = game.MapWidth;
            renderMapHeight = game.MapHeight;
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
        }
        private void Draw(object sender, EventArgs args)
        {
            for (var x = 0; x < renderMapWidth; x++)
            {
                for (var y = 0; y < renderMapHeight; y++)
                {
                    if (!renderMap[y, x])
                        continue;
                    
                    blockSprite.Begin();
                    blockSprite.Draw(blockTexture, new Vector2(x * 50, y * 50), Color.White);
                    blockSprite.End();
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
                var updateMap = (UpdateMapEvent)args;
                for (var x = 0; x < renderMapWidth; x++)
                {
                    for (var y = 0; y < renderMapHeight; y++)
                        renderMap[y, x] = updateMap.Map[y, x];
                }
            }
        }
    }
}
