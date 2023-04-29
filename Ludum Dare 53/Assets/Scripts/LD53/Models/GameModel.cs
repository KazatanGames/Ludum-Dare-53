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
    public class GameModel : Singleton<GameModel>
    {
        public Vector3 dronePosition;
        public Vector3 droneVelocity;
        public Vector3 droneAcceleration;
        public Vector3 dronePlayerAccel;
        public Vector3 droneWindAccel;
        public float droneSpeed;

        public void Reset()
        {
            dronePosition = LD53AppManager.INSTANCE.AppConfig.droneStartPosition;
            droneVelocity = Vector3.zero;
            droneAcceleration = Vector3.zero;
            dronePlayerAccel = Vector3.zero;
            droneWindAccel = Vector3.zero;
            droneSpeed = 0;
        }

        public void Tick(float frameTime)
        {
            droneAcceleration = dronePlayerAccel + droneWindAccel;

            DroneMovement(frameTime);
        }

        protected void DroneMovement(float frameTime)
        {
            Vector3 newDronePosition = dronePosition;
            Vector3 newDroneVelocity = droneVelocity;

            // accel
            newDroneVelocity += droneAcceleration * frameTime;

            // drag
            Vector3 drag = newDroneVelocity *= LD53AppManager.INSTANCE.AppConfig.droneDragCoeff;
            newDroneVelocity -= drag;

            // calc new speed
            float newDroneSpeed = newDroneVelocity.magnitude;

            // clamp speed
            if (newDroneSpeed <= 0.005f)
            {
                newDroneVelocity = Vector3.zero;
                newDroneSpeed = 0f;
            } else if (newDroneSpeed > LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed)
            {
                newDroneVelocity = newDroneVelocity.normalized * LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed;
                newDroneSpeed = LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed;
            }

            // movement
            newDronePosition += newDroneVelocity * frameTime;

            dronePosition = newDronePosition;
            droneVelocity = newDroneVelocity;
            droneSpeed = newDroneSpeed;
        }
    }
}