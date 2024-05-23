using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlocksGame.MVC.UI.Abstract;

namespace BlocksGame.MVC.UI
{
    public class Button : UIHandlesClick
    {
        private string text;
        private Texture2D texture;
        private string image;
        private Rectangle rect;
        private Vector2? textSize;

        public Button(GameCore core, string imageName, string buttonText, Rectangle buttonRect)
        {
            image = imageName;
            text = buttonText;
            rect = buttonRect;
            textSize = null;
            core.OnLoad += Load;
        }

        public override bool Contains(Point point)
            => rect.Contains(point);

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (!textSize.HasValue && text is not null)
                textSize = spriteFont.MeasureString(text);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, rect, Color.White);
            if (text is not null)
                spriteBatch.DrawString(spriteFont, text, 
                                      new Vector2(rect.X + (rect.Width - textSize.Value.X) / 2, 
                                                  rect.Y + (rect.Height - textSize.Value.Y) / 2), 
                                      Color.White);
            spriteBatch.End();
        }

        private void Load(object sender, EventArgs args)
        {
            var core = (GameCore)sender;
            texture = core.Content.Load<Texture2D>(image);
        }
    }
}
