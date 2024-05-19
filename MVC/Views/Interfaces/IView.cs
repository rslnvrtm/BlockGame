using System;

namespace BlocksGame.MVC.Views
{
    public interface IView
    {
        public void Draw(object sender, EventArgs args);
        public void Update(object sender, EventArgs args);
        public void Load(object sender, EventArgs args);
    }
}
