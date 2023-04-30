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
    public class CellData
    {
        public GridPos pos;
        public CellTypeEnum cellType;
        public int officeFloors;
        public bool targetHuntTarget;
    }
}