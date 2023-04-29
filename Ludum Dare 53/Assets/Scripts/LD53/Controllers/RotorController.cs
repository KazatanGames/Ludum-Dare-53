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
    public class RotorController : MonoBehaviour
    {
        [SerializeField]
        protected Transform bearing;
        [SerializeField]
        protected Transform blades;

        private void Update()
        {
            float leanX = (GameModel.Current.dronePlayerAccel.normalized.x / 2f) + 0.5f;
            float leanZ = (GameModel.Current.dronePlayerAccel.normalized.z / 2f) + 0.5f;

            float leanAngleX = Mathf.Lerp(-LD53AppManager.INSTANCE.AppConfig.rotorMaxLeanAngle, LD53AppManager.INSTANCE.AppConfig.rotorMaxLeanAngle, leanZ);
            float leanAngleZ = -Mathf.Lerp(-LD53AppManager.INSTANCE.AppConfig.rotorMaxLeanAngle, LD53AppManager.INSTANCE.AppConfig.rotorMaxLeanAngle, leanX);

            bearing.localRotation = Quaternion.Euler(leanAngleX, 0f, leanAngleZ);
            blades.Rotate(Vector3.back, LD53AppManager.INSTANCE.AppConfig.rotorRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}