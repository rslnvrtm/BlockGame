using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BlocksGame.MVC.UI.Interfaces
{
    public interface IBaseUIElement
    {
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont);
        public bool Contains(Point point);
    }
}
