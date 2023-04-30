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
    public class OfficeBuildingController : BaseCellController
    {
        [SerializeField]
        protected BoxCollider physicsCollider;
        [SerializeField]
        protected Transform officeTop;

        public void Build(int floors)
        {
            officeTop.localScale = new Vector3(1, floors * 2f, 1);
            physicsCollider.center = new(0, floors, 0);
            physicsCollider.size = new(physicsCollider.size.x, floors * 2f, physicsCollider.size.z);
        }
    }
}