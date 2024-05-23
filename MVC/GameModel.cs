using System;
using Microsoft.Xna.Framework;
using BlocksGame.MVC.Events;
using BlocksGame.MVC.Abstract;
using System.Collections.Generic;
using System.Linq;
using BlocksGame.MVC.Helpers.Interfaces;

namespace BlocksGame.MVC
{
    public class GameModel : DependsOnState
    {
        public event OnEventCallback OnUpdate;
        public BlockType[,] Map { get; private set; }
        public readonly int MapWidth;
        public readonly int MapHeight;
        private int score;
        private GameCore core;
        private IHintProvider hintProvider;
        
        public GameModel(StateManager stateManager, Controller controller, GameCore core, IHintProvider hintProvider, int mapWidth, int mapHeight) : base(stateManager)
        {
            controller.OnUpdate += Update;
            InitMap(mapWidth, mapHeight);
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            score = 0;
            this.core = core;
            this.hintProvider = hintProvider;
        }

        public void Reset()
        {
            InitMap(MapWidth, MapHeight);
            score = 0;
        }

        private void Update(object sender, EventArgs args)
        {
            if (OnUpdate == null)
                return;

            var controller = (Controller)sender;
            if (args is PlaceBlockEvent blockPlaceEvent)
            {
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
            else if (args is GameOverCheckEvent gameOverCheckEvent)
            {
                if (IsGameOver(gameOverCheckEvent.ChooseList))
                {
                    OnUpdate(this, new GameOverEvent(score));
                    core.Restart();
                }
            }
            else if (args is GetHintEvent getHintEvent)
            {
                var hint = hintProvider.GetHint(Map, getHintEvent.blocksToChoose);
                if (hint.HasValue)
                    OnUpdate(this, new DisplayHintEvent(hint.Value.position, hint.Value.block));
            }
            else // pass all other events through
                OnUpdate(this, args);
        }

        private bool IsGameOver(List<BlockType[,]> chooseList)
        {
            if (chooseList.All(el => el is null))
                return false;

            for (var x = 0; x < MapWidth; x++)
            {
                for (var y = 0; y < MapHeight; y++)
                {
                    if (Map[y, x] != BlockType.None)
                        continue;

                    foreach (var matrix in chooseList)
                    {
                        if (CanPlace(new Point(x, y), matrix))
                            return false;
                    }
                }
            }

            return true;
        }

        private void InitMap(int width, int height)
        {
            Map = new BlockType[height, width];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                    Map[y, x] = BlockType.None;
            }
        }

        private bool CanPlace(Point position, BlockType[,] matrix)
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
                    if (matrix[y, x] == BlockType.None)
                        continue;

                    if (Map[position.Y + y, position.X + x] != BlockType.None)
                        return false;
                }
            }

            return true;
        }

        private void PlaceBlock(Point position, BlockType[,] matrix)
        {
            if (matrix == null)
                return;

            var yLength = matrix.GetLength(0);
            var xLength = matrix.GetLength(1);

            for (var x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    if (matrix[y, x] == BlockType.None)
                        continue;

                    Map[position.Y + y, position.X + x] = matrix[y, x];
                }
            }
        }

        private bool TryPlaceBlock(Point position, BlockType[,] matrix)
        {
            if (!CanPlace(position, matrix))
                return false;

            PlaceBlock(position, matrix);
            return true;
        }

        private IEnumerable<(int Index, IEnumerable<BlockType> Row)> GetMapRows()
        {
            for (var y = 0; y < MapHeight; y++)
            {
                var row = new List<BlockType>();
                for (var x = 0; x < MapWidth; x++)
                    row.Add(Map[y, x]);

                yield return (y, row);
            }
        }

        private IEnumerable<(int Index, IEnumerable<BlockType> Column)> GetMapColumns()
        {
            for (var x = 0; x < MapWidth; x++)
            {
                var column = new List<BlockType>();
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
                    Map[index, i] = BlockType.None;
                else
                    Map[i, index] = BlockType.None;
            }
        }

        private void RemoveExplosionArea(int x, int y)
        {
            for (var dx = -1; dx < 2; dx++)
            {
                for (var dy = -1; dy < 2; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    if (dx + x >= MapWidth || dx + x < 0 || dy + y >= MapHeight || dy + y < 0)
                        continue;

                    Map[dy + y, dx + x] = BlockType.None;
                }
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
            // 3x3 area around each exploded bomb will be removed
            var explodedBombs = new List<(int x, int y)>();
            
            foreach (var row in rows)
            {
                if (row.Row.All(el => el != BlockType.None))
                {
                    rowsToRemove.Add(row.Index);
                    removed++;

                    if (row.Row.Contains(BlockType.Bomb))
                    {
                        var bombs = row.Row.Select((el, i) => (type: el, x: i)).Where(el => el.type == BlockType.Bomb);
                        foreach (var bomb in bombs)
                            explodedBombs.Add((bomb.x, row.Index));
                    }
                }
            }

            foreach (var column in columns)
            {
                if (column.Column.All(el => el != BlockType.None))
                {
                    columnsToRemove.Add(column.Index);
                    removed++;

                    if (column.Column.Contains(BlockType.Bomb))
                    {
                        var bombs = column.Column.Select((el, i) => (type: el, y: i)).Where(el => el.type == BlockType.Bomb);
                        foreach (var bomb in bombs)
                            explodedBombs.Add((column.Index, bomb.y));
                    }
                }
            }

            foreach (var index in rowsToRemove)
                RemoveLine(index);
            foreach (var index in columnsToRemove)
                RemoveLine(index, false);
            foreach (var bomb in explodedBombs)
                RemoveExplosionArea(bomb.x, bomb.y);

            return removed;
        }
    }
}