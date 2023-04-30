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
        [Header("Player")]
        public DroneController dronePrefab;

        [Header("Parcel")]
        public ParcelController parcelPrefab;
        public Transform parcelDestroyPrefab;

        [Header("Target")]
        public TargetController targetPrefab;
        public Transform targetHitPrefab;

        [Header("Cell Types")]
        public OfficeBuildingController officeBuilding;
        public Transform roadStraight;
        public Transform roadCorner;
        public Transform roadTJunc;
        public Transform roadXJunc;
        public Transform roadDeadEnd;
        public Transform concreteGround;
        public Transform grassGround;
        public Transform landingPad;

        [Header("UI")]
        public CountdownPanel countdown;
        public GameoverPanel gameOver;
    }
}