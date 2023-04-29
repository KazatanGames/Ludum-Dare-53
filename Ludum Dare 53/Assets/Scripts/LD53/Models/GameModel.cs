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
        public bool playerFiring;
        public bool playerFired;
        public float firePower;
        public float firePowerRatio;
        public Vector2 aimInput;

        public CellData[,] cells;

        public event Action<Vector3, Vector3> OnFire;

        public void Reset()
        {
            dronePosition = new((LD53AppManager.INSTANCE.AppConfig.playAreaSize.x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f, LD53AppManager.INSTANCE.AppConfig.droneHeight, (LD53AppManager.INSTANCE.AppConfig.playAreaSize.y * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize) / 2f);
            droneVelocity = Vector3.zero;
            droneAcceleration = Vector3.zero;
            dronePlayerAccel = Vector3.zero;
            droneWindAccel = Vector3.zero;
            droneSpeed = 0;
            playerFiring = false;
            firePower = 0;
            firePowerRatio = 0;
            aimInput = Vector2.zero;
            cells = WorldGen.Generate(LD53AppManager.INSTANCE.AppConfig.playAreaSize.x, LD53AppManager.INSTANCE.AppConfig.playAreaSize.y);
        }

        public void Tick(float tickTime)
        {
            if (playerFired)
            {
                if (!playerFiring) playerFired = false;
            }
            else
            {
                Vector2 fireAngle = new(aimInput.x - dronePosition.x, aimInput.y - dronePosition.z);
                if (playerFiring)
                {
                    firePower += LD53AppManager.INSTANCE.AppConfig.firePowerIncrease * tickTime;
                    if (firePower > LD53AppManager.INSTANCE.AppConfig.maxFirePower)
                    {
                        OnFire?.Invoke(dronePosition, new Vector3(fireAngle.normalized.x * firePower, LD53AppManager.INSTANCE.AppConfig.parcelYFireSpeed, fireAngle.normalized.y * firePower) + droneVelocity * LD53AppManager.INSTANCE.AppConfig.parcelDroneVelocityMulti);
                        firePower = 0;
                        playerFired = true;
                    }
                }
                else if (firePower > 0)
                {
                    OnFire?.Invoke(dronePosition, new Vector3(fireAngle.normalized.x * firePower, LD53AppManager.INSTANCE.AppConfig.parcelYFireSpeed, fireAngle.normalized.y * firePower) + droneVelocity * LD53AppManager.INSTANCE.AppConfig.parcelDroneVelocityMulti);
                    firePower = 0;
                    playerFired = true;
                }
            }
            firePowerRatio = firePower / LD53AppManager.INSTANCE.AppConfig.maxFirePower;
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