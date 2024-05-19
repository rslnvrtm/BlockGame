using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.Views;
using BlocksGame.MVC.Abstract;
using BlocksGame.MVC.UI.Abstract;
using Microsoft.Xna.Framework.Input;

namespace BlocksGame.MVC.UI
{
    public class Button : UIHandlesClick
    {
        private string text;
        private Texture2D texture;
        private string image;
        private Rectangle rect;

        public Button(GameCore core, string imageName, string buttonText, Rectangle buttonRect)
        {
            image = imageName;
            text = buttonText;
            rect = buttonRect;
            core.OnLoad += Load;
        }

        public override bool Contains(Point point)
            => rect.Contains(point);

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, rect, Color.White);
            if (text is not null)
                spriteBatch.DrawString(spriteFont, text, new Vector2(rect.X + rect.Width / 4, rect.Y + rect.Height / 6), Color.White);
            spriteBatch.End();
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;
            texture = core.Content.Load<Texture2D>(image);
        }
    }
}
