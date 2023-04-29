using KazatanGames.Framework;
using System;
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
    public static class PositionHelpers
    {
        public static Vector3 CenterOfGridPosInWorld(int x, int y, float h = 0f)
        {
            float hg = LD53AppManager.INSTANCE.AppConfig.playAreaGridSize / 2f;
            return new(
                (x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) + hg,
                h,
                (y * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) + hg
            );
        }

        public static Vector2Int WorldToGridPos(Vector3 worldPos)
        {
            return new(
                Mathf.FloorToInt(worldPos.x / LD53AppManager.INSTANCE.AppConfig.playAreaGridSize),
                Mathf.FloorToInt(worldPos.z / LD53AppManager.INSTANCE.AppConfig.playAreaGridSize)
            );
        }
    }
}