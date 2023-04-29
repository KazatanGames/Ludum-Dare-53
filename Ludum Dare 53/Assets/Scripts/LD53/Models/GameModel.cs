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

        public CellData[,] cells;

        public void Reset()
        {
            dronePosition = new((LD53AppManager.INSTANCE.AppConfig.playAreaSize.x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f, LD53AppManager.INSTANCE.AppConfig.droneHeight, (LD53AppManager.INSTANCE.AppConfig.playAreaSize.y * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f);
            droneVelocity = Vector3.zero;
            droneAcceleration = Vector3.zero;
            dronePlayerAccel = Vector3.zero;
            droneWindAccel = Vector3.zero;
            droneSpeed = 0;
            cells = WorldGen.Generate(LD53AppManager.INSTANCE.AppConfig.playAreaSize.x, LD53AppManager.INSTANCE.AppConfig.playAreaSize.y);
        }

        public void Tick(float tickTime)
        {

        }

        public void Frame(float frameTime)
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
            Vector3 drag = newDroneVelocity * LD53AppManager.INSTANCE.AppConfig.droneDragCoeff;
            newDroneVelocity -= drag * frameTime;

            // calc new speed
            float newDroneSpeed = newDroneVelocity.magnitude;

            // clamp speed
            if (newDroneSpeed <= 0.001f)
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