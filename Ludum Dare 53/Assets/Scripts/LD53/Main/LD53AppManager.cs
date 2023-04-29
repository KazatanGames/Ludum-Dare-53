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
    public class LD53AppManager : AppManager<LD53AppManager, LD53AppConfigSO>
    {
        public CommonData Common { get; private set; }

        protected override void Initialise()
        {
            base.Initialise();

            Common = new();
        }
    }
}