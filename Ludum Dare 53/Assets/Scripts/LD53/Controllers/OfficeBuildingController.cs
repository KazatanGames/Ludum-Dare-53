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
    public class OfficeBuildingController : MonoBehaviour
    {
        [SerializeField]
        protected int minMidFloors = 3;
        [SerializeField]
        protected int maxMidFloors = 12;
        [SerializeField]
        protected float interestChance = 0.75f;
        [SerializeField]
        protected float floorSpacing = 0.25f;

        private void Awake()
        {
            int floors = Random.Range(minMidFloors, maxMidFloors);
            for(int i = 0; i < floors; i++)
            {
                Transform t = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuildingMid, transform);
                t.localPosition = new(0, i * floorSpacing, 0f);
            }
            {
                Transform t = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuildingTop, transform);
                t.localPosition = new(0, floors * floorSpacing, 0f);
            }
            if (Random.value <= interestChance) {
                Transform t = Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.officeBuildingInterest, transform);
                t.localPosition = new((Random.value * 16f) - 8f, (floors + 1) * floorSpacing, (Random.value * 16f) - 8f);
            }
        }

    }
}