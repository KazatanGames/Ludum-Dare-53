using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Pew Pew Pew 2023
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.Framework
{
    public interface INoiseProvider
    {
        float GetPerlinValue(int x, int y, float scale, float offsetX, float offsetY);

        float GetVoronoiValue(int x, int y, float scale, float offsetX, float offsetY);

        float GetSimplexValue(int x, int y, float scale, float offsetX, float offsetY);
    }
}