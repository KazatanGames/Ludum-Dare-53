﻿using KazatanGames.Framework;
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
        public static WorldData Generate(int w, int h)
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
            HashSet<GridPos> validCellsHash = new();

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

                    if (IsCenter(x, y, w, h))
                    {
                        cell.cellType = CellTypeEnum.LandingPad;
                    } else if (IsEdge(x, y, w, h))
                    {
                        // edge of world - add an office
                        cell.cellType = CellTypeEnum.Office;
                        cell.officeFloors = Random.Range(LD53AppManager.INSTANCE.AppConfig.minOfficeFloors, LD53AppManager.INSTANCE.AppConfig.maxOfficeFloors);
                    } else if (IsEdge(x, y, w, h, 1))
                    {
                        // one in from edge of world - add a road
                        cell.cellType = CellTypeEnum.Road;
                    }
                    else if (n.GetPerlinValue(x, y, LD53AppManager.INSTANCE.AppConfig.grassScale, 1.2781f * Time.realtimeSinceStartup, 3.8382f * Time.realtimeSinceStartup) > grassInvChance)
                    {
                        cell.cellType = CellTypeEnum.Grass;
                    }

                    if (cell.cellType == CellTypeEnum.Concrete)
                    {
                        GridPos cellPos = new(x, y);
                        validCells.Add(cellPos);
                        validCellsHash.Add(cellPos);
                    }
                }
            }

            for (int i = 0; i < LD53AppManager.INSTANCE.AppConfig.genLoops; i++)
            {
                RandomOffices(cells, LD53AppManager.INSTANCE.AppConfig.genOffices, ref validCells, ref validCellsHash);
                RandomRoads(cells, LD53AppManager.INSTANCE.AppConfig.genRoads, ref validCells, ref validCellsHash, w, h);
                RandomGrass(cells, LD53AppManager.INSTANCE.AppConfig.genGrass, ref validCells, ref validCellsHash);
            }

            int targetCount = GenerateTargetHuntTargets(cells, w, h);

            return new WorldData()
            {
                cells = cells,
                targetCount = targetCount
            };
        }

        private static bool IsEdge(int x, int y, int w, int h, int offset = 0)
        {
            return x == offset || y == offset || x == (w - (1 + offset)) || y == (h - (1 + offset));
        }

        private static bool IsCenter(int x, int y, int w, int h)
        {
            return x == w / 2 && y == h / 2;
        }

        private static void RandomOffices(CellData[,] cells, int num, ref List<GridPos> validCells, ref HashSet<GridPos> validCellsHash)
        {
            for(int i = 0; i < num; i++)
            {
                if (validCells.Count == 0) return;
                int rnd = Random.Range(0, validCells.Count);
                GridPos pos = validCells[rnd];
                validCells.RemoveAt(rnd);
                validCellsHash.Remove(pos);
                cells[pos.x, pos.z].cellType = CellTypeEnum.Office;
                cells[pos.x, pos.z].officeFloors = Random.Range(LD53AppManager.INSTANCE.AppConfig.minOfficeFloors, LD53AppManager.INSTANCE.AppConfig.maxOfficeFloors);
            }
        }

        private static void RandomRoads(CellData[,] cells, int num, ref List<GridPos> validCells, ref HashSet<GridPos> validCellsHash, int w, int h)
        {
            for (int i = 0; i < num; i++)
            {
                if (validCells.Count == 0) return;
                int rnd = Random.Range(0, validCells.Count);
                GridPos pos = validCells[rnd];
                validCells.RemoveAt(rnd);
                validCellsHash.Remove(pos);

                cells[pos.x, pos.z].cellType = CellTypeEnum.Road;

                bool roadComplete = false;
                bool firstCheck = true;
                GameDirection potentialNeighbours = GameDirectionHelper.All;
                GameDirection favouredDirection = GameDirectionHelper.RandomSingle;
                GameDirection sourceDirection = GameDirectionHelper.Nil;
                while (!roadComplete && potentialNeighbours.Count() > 0)
                {
                    GameDirection testDir = favouredDirection;

                    if (!potentialNeighbours.Test(favouredDirection) || Random.value > LD53AppManager.INSTANCE.AppConfig.roadStraightChance)
                    {
                        testDir = potentialNeighbours.RandomSinglePreferAdjacent(favouredDirection);
                    }

                    potentialNeighbours &= ~testDir;

                    GridPos testPos = pos.Adjacent(testDir);
                    if (validCellsHash.Contains(testPos))
                    {
                        pos = testPos;
                        validCells.RemoveAt(validCells.IndexOf(pos));
                        validCellsHash.Remove(pos);

                        cells[pos.x, pos.z].cellType = CellTypeEnum.Road;
                        potentialNeighbours = GameDirectionHelper.All;
                    } else if (firstCheck)
                    {
                        favouredDirection = favouredDirection.Opposite();
                    }

                    firstCheck = false;

                    if (IsEdge(pos.x, pos.z, w, h, 2)) roadComplete = true;

                    if (sourceDirection != GameDirectionHelper.Nil) {
                        foreach (GameDirection futureDirection in (~sourceDirection).ToList())
                        {
                            GridPos futurePos = pos.Adjacent(futureDirection);
                            if (cells[futurePos.x, futurePos.z].cellType == CellTypeEnum.Road) roadComplete = true;
                        }
                    }

                    sourceDirection = testDir.Opposite();
                }
            }
        }

        private static void RandomGrass(CellData[,] cells, int num, ref List<GridPos> validCells, ref HashSet<GridPos> validCellsHash)
        {
            for (int i = 0; i < num; i++)
            {
                if (validCells.Count == 0) return;
                int rnd = Random.Range(0, validCells.Count);
                GridPos pos = validCells[rnd];
                validCells.RemoveAt(rnd);
                validCellsHash.Remove(pos);
                cells[pos.x, pos.z].cellType = CellTypeEnum.Grass;
            }
        }

        private static int GenerateTargetHuntTargets(CellData[,] cells, int w, int h)
        {
            int colCells = Mathf.CeilToInt(w / LD53AppManager.INSTANCE.AppConfig.targetSpreadColumns);
            int rowCells = Mathf.CeilToInt(h / LD53AppManager.INSTANCE.AppConfig.targetSpreadRows);

            Dictionary<GridPos, List<GridPos>> validCellsByGenerationGrid = new();
            List<GridPos> gridSquares = AllGridSquares(LD53AppManager.INSTANCE.AppConfig.targetSpreadColumns, LD53AppManager.INSTANCE.AppConfig.targetSpreadRows);
            foreach (GridPos gpos in gridSquares)
            {
                validCellsByGenerationGrid.Add(gpos, new());
            }

            int totalValidCells = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!IsEdge(x, y, w, h) && !IsCenter(x, y, w, h))
                    {
                        validCellsByGenerationGrid[new(Mathf.FloorToInt(x / colCells), Mathf.FloorToInt(y / rowCells))].Add(new(x, y));
                        totalValidCells++;
                    }
                }
            }

            int targetsGenerated = 0;

            while(targetsGenerated < LD53AppManager.INSTANCE.AppConfig.targetsToHunt && totalValidCells > 0)
            {
                if (gridSquares.Count == 0)
                {
                    gridSquares = AllGridSquares(LD53AppManager.INSTANCE.AppConfig.targetSpreadColumns, LD53AppManager.INSTANCE.AppConfig.targetSpreadRows);
                }
                int index = Random.Range(0, gridSquares.Count);
                GridPos pos = gridSquares[index];
                gridSquares.RemoveAt(index);

                if (validCellsByGenerationGrid[pos].Count > 0)
                {
                    int cIndex = Random.Range(0, validCellsByGenerationGrid[pos].Count);
                    GridPos candidate = validCellsByGenerationGrid[pos][cIndex];
                    validCellsByGenerationGrid[pos].RemoveAt(cIndex);
                    cells[candidate.x, candidate.z].targetHuntTarget = true;
                    targetsGenerated++;
                }
            }

            return targetsGenerated;
        }

        private static List<GridPos> AllGridSquares(int w, int h)
        {
            List<GridPos> gSquares = new();
            for (int gX = 0; gX < w; gX++)
            {
                for (int gY = 0; gY < h; gY++)
                {
                    gSquares.Add(new(gX, gY));
                }
            }
            return gSquares;
        }
    }
}