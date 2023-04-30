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
    public class IntroParcelController : MonoBehaviour
    {
        public Rigidbody body;

        private void Update()
        {
            if (transform.position.y < -50f) Destroy(gameObject);
        }
    }
}