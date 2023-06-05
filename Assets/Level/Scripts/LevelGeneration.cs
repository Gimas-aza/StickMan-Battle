using System.Collections.Generic;
using UnityEngine;

namespace Assets.Level
{
    public class LevelGeneration
    {
        private int _width = 4;
        private int _height = 9;

        public LevelGeneratorCell[,] Generate()
        {
            var cells = new LevelGeneratorCell[_width, _height];

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int z = 0; z < cells.GetLength(1); z++)
                {
                    cells[x, z] = new LevelGeneratorCell {X = x, Z = z};
                }
            }

            RemoveWallsWithBacktracker(cells);

            return cells;
        }

        private void RemoveWallsWithBacktracker(LevelGeneratorCell[,] cells)
        {
            LevelGeneratorCell currentCell = cells[0, 0];
            Stack<LevelGeneratorCell> stack = new();
            currentCell.Visited = true;

            do
            {
                List<LevelGeneratorCell> unvisitedNeighbours = GetUnvisitedNeighbours(cells, currentCell);

                if (unvisitedNeighbours.Count > 0)
                {
                    LevelGeneratorCell nextCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                    RemoveWall(currentCell, nextCell);

                    nextCell.Visited = true;
                    stack.Push(nextCell);
                    currentCell = nextCell;
                }
                else
                {
                    currentCell = stack.Pop();
                }
            } while (stack.Count > 0);
        }

        private List<LevelGeneratorCell> GetUnvisitedNeighbours(LevelGeneratorCell[,] cells, LevelGeneratorCell currentCell)
        {
            List<LevelGeneratorCell> unvisitedNeighbours = new();

            int x = currentCell.X;
            int z = currentCell.Z;

            if (x > 0 && !cells[x - 1, z].Visited) unvisitedNeighbours.Add(cells[x - 1, z]);
            if (z > 0 && !cells[x, z - 1].Visited) unvisitedNeighbours.Add(cells[x, z - 1]);
            if (x < _width - 1 && !cells[x + 1, z].Visited) unvisitedNeighbours.Add(cells[x + 1, z]);
            if (z < _height - 1 && !cells[x, z + 1].Visited) unvisitedNeighbours.Add(cells[x, z + 1]);

            return unvisitedNeighbours;
        }

        private void RemoveWall(LevelGeneratorCell currentCell, LevelGeneratorCell nextCell)
        {
            if (currentCell.X == nextCell.X)
            {
                if (nextCell.Z > currentCell.Z) currentCell.WallBottom = false;
                else nextCell.WallBottom = false;
            }
            else
            {
                if (nextCell.X > currentCell.X) currentCell.WallLeft = false;
                else nextCell.WallLeft = false;
            }
        }
    }
}
