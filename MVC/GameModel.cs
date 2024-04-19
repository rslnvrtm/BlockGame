using System;
using Microsoft.Xna.Framework;
using BlocksGame.MVC.Events;

namespace BlocksGame.MVC.Models
{
    public class GameModel
    {
        public event OnEventCallback OnUpdate;
        public bool[,] Map { get; private set; }
        public readonly int MapWidth;
        public readonly int MapHeight;

        public GameModel(Controller controller, int mapWidth, int mapHeight)
        {
            controller.OnUpdate += Update;
            InitMap(mapWidth, mapHeight);
            MapWidth = mapWidth;
            MapHeight = mapHeight;
        }

        private void Update(object sender, EventArgs args)
        {
            var controller = (Controller)sender;
            if (OnUpdate != null && args is PlaceBlockEvent)
            {
                var blockPlaceEvent = ((PlaceBlockEvent)args);
                if (TryPlaceBlock(blockPlaceEvent.Position, blockPlaceEvent.BlockMatrix))
                    OnUpdate(this, new UpdateMapEvent(Map));
                else
                    OnUpdate(this, new PlaceFailEvent());
            }
        }

        private void InitMap(int width, int height)
        {
            Map = new bool[height, width];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                    Map[y, x] = false;
            }
        }
        private bool CanPlace(Point position, bool[,] matrix)
        {
            var yLength = matrix.GetLength(0);
            var xLength = matrix.GetLength(1);

            if (position.X + xLength - 1 >= MapWidth 
                || position.Y + yLength - 1 >= MapHeight)
                return false;

            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    if (!matrix[y, x])
                        continue;

                    if (Map[position.Y + y, position.X + x])
                        return false;
                }
            }

            return true;
        }

        private void PlaceBlock(Point position, bool[,] matrix)
        {
            var yLength = matrix.GetLength(0);
            var xLength = matrix.GetLength(1);

            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    if (!matrix[y, x])
                        continue;

                    Map[position.Y + y, position.X + x] = true;
                }
            }
        }

        private bool TryPlaceBlock(Point position, bool[,] matrix)
        {
            if (!CanPlace(position, matrix))
                return false;

            PlaceBlock(position, matrix);
            return true;
        }
    }
}