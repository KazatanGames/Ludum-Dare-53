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

        [Header("Prefabs")]
        public PrefabRegisterSO prefabRegister;

        [Header("Game Settings")]
        public float droneHeight = 20f;
        public float droneIntroLength = 3f;
        public float droneDragCoeff = 2f;
        public float maxDroneSpeed = 10f;
        public float rotorRotationSpeed = 720f;
        public float droneAcceleration = 5f;
        public float rotorMaxLeanAngle = 25f;
        public float droneMaxLeanAngle = 15f;
        public float fireDelay = 0.25f;
        public float parcelDropOffset = 2f;
        public float parcelYFireSpeed = -2f;
        public float parcelTorqueMax = 0.1f;
        public float cameraMinHeight = 15f;
        public float cameraMaxHeight = 45f;
        public float cameraHeightSmoothTime = 0.5f;
        public int targetsToHunt = 100;

        [Header("Generation")]
        public Vector2Int playAreaSize = new(128, 128);
        public int playAreaGridSize = 8;
        public float grassChance = 0.35f;
        [Range(1f, 10f)]
        public float grassScale = 3.15f;
        public int genOffices = 10;
        public int genRoads = 10;
        public int genGrass = 1;
        public int genLoops = 8;
        public float roadStraightChance = 0.667f;
        public int targetSpreadColumns = 4;
        public int targetSpreadRows = 3;
        public int minOfficeFloors = 2;
        public int maxOfficeFloors = 12;
    }
}