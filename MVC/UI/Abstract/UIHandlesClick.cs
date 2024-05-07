using System;
using BlocksGame.MVC.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlocksGame.MVC.UI.Abstract
{
    public abstract class UIHandlesClick : IBaseUIElement
    {
        public event OnEventCallback OnClick;

        public abstract bool Contains(Point point);

        public virtual bool HandleClick(Point point)
        {
            if (Contains(point))
            {
                OnClick(this, null);
                return true;
            }

            return false;
        }

        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont);
    }
}
