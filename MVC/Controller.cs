using BlocksGame.MVC.Events;
using System;
using Microsoft.Xna.Framework;

namespace BlocksGame.MVC
{
    public delegate void OnEventCallback(object sender, EventArgs args);

    public class Controller
    {
        public event OnEventCallback OnUpdate;
    }
}
