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
        protected int minFloors = 3;
        [SerializeField]
        protected int maxFloors = 12;
        [SerializeField]
        protected float floorSpacing = 2f;

        protected override void Initialise()
        {
            int floors = Random.Range(minFloors, maxFloors);
            for (int i = 0; i < floors; i++)
            {
                Transform t = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuildingFloor, transform);
                t.localPosition = new(0, i * floorSpacing, 0f);
            }
            {
                Transform t = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuildingTop, transform);
                t.localPosition = new(0, floors * floorSpacing, 0f);
            }

            physicsCollider.center = new(0, (floors + 1) * floorSpacing / 2f, 0);
            physicsCollider.size = new(physicsCollider.size.x, (floors + 1) * floorSpacing, physicsCollider.size.z);

            base.Initialise();
        }
    }
}