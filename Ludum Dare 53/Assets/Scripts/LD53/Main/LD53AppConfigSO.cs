using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Ludum Dare 53
 * 
 * A game made in 2 days.
 *
 * � Kazatan Games Ltd, 2023
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
        public Vector3 droneStartPosition = new(0, 20, 0);
        [Range(0f, 1f)]
        public float droneDragCoeff = 0.05f;
        public float maxDroneSpeed = 10f;
        public float maxRotorRotationSpeed = 720f;
        public float droneAcceleration = 5f;
    }
}