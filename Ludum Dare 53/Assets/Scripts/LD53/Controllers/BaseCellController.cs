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
    public class BaseCellController : MonoBehaviour
    {
        private void Awake()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            transform.localScale = new Vector3(
                transform.localScale.x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize,
                transform.localScale.y,
                transform.localScale.z * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize
            );
        }
    }
}