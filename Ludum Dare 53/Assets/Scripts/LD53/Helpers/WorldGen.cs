using KazatanGames.Framework;
using System.Collections.Generic;
using UnityEngine;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public static class WorldGen
    {
        public static CellData[,] Generate(int w, int h)
        {
            CellData[,] cells = new CellData[w, h];

            // create all cells
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    CellData cell = new();
                    cells[x, y] = cell;
                }
            }

            Simple2DNoise n = new(w, h);

            List<GridPos> validCells = new();

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    CellData cell = cells[x, y];

                    cell.cellType = CellTypeEnum.Concrete;

                    float grassInvChance = LD53AppManager.INSTANCE.AppConfig.grassChance;
                    // we want grass near the center
                    float xEdgeProx = Easing.Quadratic.InOut(Mathf.Abs((((float)x / w) - 0.5f) * 2f));
                    float yEdgeProx = Easing.Quadratic.InOut(Mathf.Abs((((float)y / h) - 0.5f) * 2f));
                    grassInvChance += (1.05f - LD53AppManager.INSTANCE.AppConfig.grassChance) * Mathf.Max(xEdgeProx, yEdgeProx);

                    if (IsEdge(x, y, w, h))
                    {
                        // edge of world - add an office
                        cell.cellType = CellTypeEnum.Office;
                    } else if (IsEdge(x, y, w, h, 1))
                    {
                        // one in from edge of world - add a road
                        cell.cellType = CellTypeEnum.Road;
                    }
                    else if (n.GetPerlinValue(x, y, LD53AppManager.INSTANCE.AppConfig.grassScale, 1.2781f * Time.realtimeSinceStartup, 3.8382f * Time.realtimeSinceStartup) > grassInvChance)
                    {
                        cell.cellType = CellTypeEnum.Grass;
                    }

                    if (cell.cellType == CellTypeEnum.Concrete) validCells.Add(new(x, y));
                }
            }

            validCells = RandomOffices(cells, LD53AppManager.INSTANCE.AppConfig.initialOffices, validCells);
            validCells = RandomRoads(cells, LD53AppManager.INSTANCE.AppConfig.initialRoads, validCells);

            return cells;
        }

        private static bool IsEdge(int x, int y, int w, int h, int offset = 0)
        {
            return x == offset || y == offset || x == (w - (1 + offset)) || y == (h - (1 + offset));
        }

        private static List<GridPos> RandomOffices(CellData[,] cells, int num, List<GridPos> validCells)
        {
            for(int i = 0; i < num; i++)
            {
                if (validCells.Count == 0) return validCells;
                int rnd = Random.Range(0, validCells.Count);
                GridPos pos = validCells[rnd];
                validCells.RemoveAt(rnd);

                cells[pos.x, pos.z].cellType = CellTypeEnum.Office;
            }

            return validCells;
        }

        private static List<GridPos> RandomRoads(CellData[,] cells, int num, List<GridPos> validCells)
        {
            for (int i = 0; i < num; i++)
            {
                if (validCells.Count == 0) return validCells;
                int rnd = Random.Range(0, validCells.Count);
                GridPos pos = validCells[rnd];
                validCells.RemoveAt(rnd);

                cells[pos.x, pos.z].cellType = CellTypeEnum.Road;
            }

            return validCells;
        }
    }
}