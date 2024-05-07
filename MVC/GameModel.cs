using System;
using Microsoft.Xna.Framework;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace BlocksGame.MVC
{
    public class GameModel : DependsOnState
    {
        public event OnEventCallback OnUpdate;
        public bool[,] Map { get; private set; }
        public readonly int MapWidth;
        public readonly int MapHeight;
        private int score;

        public GameModel(StateManager stateManager, Controller controller, int mapWidth, int mapHeight) : base(stateManager)
        {
            controller.OnUpdate += Update;
            InitMap(mapWidth, mapHeight);
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            score = 0;
        }

        private void Update(object sender, EventArgs args)
        {
            if (OnUpdate == null)
                return;

            var controller = (Controller)sender;
            if (args is PlaceBlockEvent)
            {
                var blockPlaceEvent = (PlaceBlockEvent)args;
                if (blockPlaceEvent.BlockMatrix == null)
                    return;

                if (TryPlaceBlock(blockPlaceEvent.Position, blockPlaceEvent.BlockMatrix))
                {
                    OnUpdate(this, new UpdateMapEvent(Map));
                    blockPlaceEvent.OnSuccess();

                    var linesRemoved = RemoveLines();
                    if (linesRemoved != 0)
                    {
                        score += linesRemoved * 10;

                        OnUpdate(this, new UpdateMapEvent(Map));
                        OnUpdate(this, new UpdateScoreEvent(score));
                    }
                }
                else
                    OnUpdate(this, new PlaceFailEvent());
            }
            else // pass all other events through
                OnUpdate(this, args);
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
            if (matrix == null || position.X < 0 || position.Y < 0)
                return false;

            var yLength = matrix.GetLength(0);
            var xLength = matrix.GetLength(1);

            if (position.X + xLength - 1 >= MapWidth || position.X + xLength - 1 < 0
                || position.Y + yLength - 1 >= MapHeight || position.Y + yLength - 1 < 0)
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
            if (matrix == null)
                return;

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

        private IEnumerable<(int Index, IEnumerable<bool> Row)> GetMapRows()
        {
            for (var y = 0; y < MapHeight; y++)
            {
                var row = new List<bool>();
                for (var x = 0; x < MapWidth; x++)
                    row.Add(Map[y, x]);

                yield return (y, row);
            }
        }

        private IEnumerable<(int Index, IEnumerable<bool> Column)> GetMapColumns()
        {
            for (var x = 0; x < MapWidth; x++)
            {
                var column = new List<bool>();
                for (var y = 0; y < MapHeight; y++)
                    column.Add(Map[y, x]);

                yield return (x, column);
            }
        }

        private void RemoveLine(int index, bool y = true)
        {
            for (var i = 0; i < (y ? MapHeight : MapWidth); i++)
            {
                if (y)
                    Map[index, i] = false;
                else
                    Map[i, index] = false;
            }
        }

        private int RemoveLines()
        {
            var rows = GetMapRows();
            var columns = GetMapColumns();

            var removed = 0;
            // store indices in two separate lists because we need to handle cross of lines
            var rowsToRemove = new List<int>();
            var columnsToRemove = new List<int>();

            foreach (var row in rows)
            {
                if (row.Row.All(el => el))
                {
                    rowsToRemove.Add(row.Index);
                    removed++;
                }
            }

            foreach (var column in columns)
            {
                if (column.Column.All(el => el))
                {
                    columnsToRemove.Add(column.Index);
                    removed++;
                }
            }

            foreach (var index in rowsToRemove)
                RemoveLine(index);
            foreach (var index in columnsToRemove)
                RemoveLine(index, false);

            return removed;
        }
    }
}