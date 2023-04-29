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

    [CreateAssetMenu(fileName = "LD53 Prefab Register", menuName = "LD53/Prefab Register", order = 99999)]
    public class PrefabRegisterSO : ScriptableObject
    {
        public Transform dronePrefab;
    }
}