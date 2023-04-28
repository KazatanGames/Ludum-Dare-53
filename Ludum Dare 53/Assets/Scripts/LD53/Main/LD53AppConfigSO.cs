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
    [CreateAssetMenu(fileName = "LD53 App Config", menuName = "LD53/App Config", order = 99999)]
    public class LD53AppConfigSO : AppConfigSO
    {
        [Header("Scenes")]
        public Utilities.SceneField titleScene;
        public Utilities.SceneField gameScene;
    }
}